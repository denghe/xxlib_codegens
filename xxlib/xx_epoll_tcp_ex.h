#include <xx_epoll.hpp>

namespace xx::Epoll {
	// 增强版( 仅支持直连 ). 支持 4字节 长度 包头拆包. 支持收发 xx::Objec 或 buf + len
	struct TcpPeerEx : TcpPeer {
		// 收到 push 包时触发( 4 字节正数长度 + 0serial + Object )
		std::function<void(xx::Object_s && msg)> onReceivePush;

		// 收到 request 包时触发( 4 字节正数长度 + 负数serial + Object )
		std::function<void(int const& serial, xx::Object_s && msg)> onReceiveRequest;

		// 断开时触发. 之后会自动 Dispose
		std::function<void(int const& reason)> onDisconnect;

		// 收到 push buf 包时触发( 4 字节负数长度 + 0serial + byte[] ). recvBB 来自 ep 共享
		std::function<void(xx::BBuffer* const& bb)> onReceivePushBuf;

		// 收到 request buf 包时触发( 4 字节负数长度 + 负数serial + byte[] ). recvBB 来自 ep 共享
		std::function<void(int const& serial, xx::BBuffer* const& bb)> onReceiveRequestBuf;

	protected:
		// SendRequest 的回调类型
		typedef std::function<int(Object_s && msg)> RequestCB;

		// 存放 SendRequest 的回调
		std::unordered_map<int, std::pair<RequestCB, Ref<Timer>>> requestCBs;

		// SendRequestBuf 的回调类型
		typedef std::function<int(xx::BBuffer* const& recvBB)> RequestBufCB;

		// 存放 SendRequestBuf 的回调
		std::unordered_map<int, std::pair<RequestBufCB, Ref<Timer>>> requestBufCBs;

		// 用于自增生成 SendRequest 的业务号
		int serial = 0;

	public:
		inline void SendPush(Object_s const& msg) noexcept {
			Send(0, msg);
		}

		inline void SendResponse(int const& serial, Object_s const& msg) noexcept {
			Send(serial, msg);
		}

		inline void SendRequest(RequestCB&& cb, int const& timeoutMS, Object_s const& msg) noexcept {
			// 循环使用正整数部分
			serial = (serial + 1) & 0x7FFFFFFF;

			// serial 取负值发出 以表达是 Request 行为
			Send(-serial, msg);

			// 创建超时检测 timer. 超时将由 timer 回调来发起对传入 cb 的调用，参数将为 nullptr. 该 timer 并不受 this 析构而自动销毁, 故需要手工析构
			auto&& waitFrames = ep->MsToFrames((timeoutMS ? timeoutMS : ep->defaultRequestTimeoutMS));
			auto&& timer = ep->CreateTimer(waitFrames, [this, serial = serial](Timer_r const& timer) {
				auto&& iter = requestCBs.find(serial);
				if (iter != requestCBs.end()) {
					auto&& a = std::move(iter->second.first);
					iter = requestCBs.erase(iter);
					a(nullptr);
				}
				});

			// 存入字典
			requestCBs[serial] = { std::move(cb), timer };
		}

		template<typename...Args>
		void SendPushBuf(Args const& ... args) {
			SendBuf(0, args...);
		}

		template<typename...Args>
		void SendResponseBuf(int const& serial, Args const& ... args) {
			SendBuf(serial, args...);
		}

		template<typename...Args>
		void SendRequestBuf(RequestBufCB&& cb, int const& timeoutMS, Args const& ... args) {
			// 循环使用正整数部分
			serial = (serial + 1) & 0x7FFFFFFF;

			// serial 取负值发出 以表达是 Request 行为
			SendBuf(-serial, args...);

			// 创建超时检测 timer. 超时将由 timer 回调来发起对传入 cb 的调用，参数将为 nullptr. 该 timer 并不受 this 析构而自动销毁, 故需要手工析构
			auto&& waitFrames = ep->MsToFrames((timeoutMS ? timeoutMS : ep->defaultRequestTimeoutMS));
			auto&& timer = ep->CreateTimer(waitFrames, [this, serial = serial](Timer_r const& timer) {
				auto&& iter = requestBufCBs.find(serial);
				if (iter != requestBufCBs.end()) {
					auto&& a = std::move(iter->second.first);
					iter = requestBufCBs.erase(iter);
					a(nullptr);
				}
			});

			// 存入字典
			requestBufCBs[serial] = { std::move(cb), timer };
		}

	protected:
		// 基础发包函数( 符合 header + data 的结构. 常见于直连通信 )
		inline void Send(int const& serial, Object_s const& msg) {
			auto&& bb = ep->sendBB;
			bb.Reserve(1024);
			bb.len = 4;	// 空出 长度 头部
			bb.Write(serial);
			bb.WriteRoot(msg);
			*(uint32_t*)bb.buf = (uint32_t)(bb.len - 4);	// 填充数据长度到包头
			this->TcpPeer::Send(bb);
		}

		// 基础发内部指令包函数( 符合 header + data 的结构. 常见于直连通信 )
		template<typename...Args>
		void SendBuf(int const& serial, Args const& ... cmdAndArgs) {
			auto&& bb = ep->sendBB;
			bb.Reserve(64);
			bb.len = 4;	// 空出 长度 头部
			bb.WriteFixed(0xFFFFFFFFu);
			bb.Write(serial);
			bb.Write(cmdAndArgs...);
			*(uint32_t*)bb.buf = -(int)(uint32_t)(bb.len - 4);	// 填充 负数 数据长度到包头
			this->TcpPeer::Send(bb);
		}

		// 注意：可能 Dispose. 调用者需要做 alive 检查( 该备注向上感染 )
		inline virtual void OnDisconnect(int const& reason) override {
			if (onDisconnect) {
				onDisconnect(reason);
			}
		}

		// 数据接收事件: 从 recv 拿数据, 根据 4字节 长度 包头拆包 进一步 call OnReceivePackage
		// 注意：可能 Dispose. 调用者需要做 alive 检查( 该备注向上感染 )
		inline virtual void OnReceive() override {
			// 取出指针备用
			auto buf = (uint8_t*)this->recv.buf;
			auto end = (uint8_t*)this->recv.buf + this->recv.len;

			// 死亡判断变量
			Ref<Item> alive(this);

			// 确保包头长度充足
			while (buf + 4 <= end) {
				// 标识数据类型是否为 buf
				bool isBuf = false;

				// 取出数据区长度
				auto dataLen = *(int*)buf;

				// 如果为负数长度则标识为 buf 数据类型
				if (dataLen < 0) {
					isBuf = true;
					dataLen = -dataLen;
				}

				// 判断 长度 合法性
				if (dataLen > (int)this->ep->maxPackageLength) {
					OnDisconnect(-5);
					// 如果当前类实例已自杀则退出
					if (!alive) return;
					Dispose();
					return;
				}

				// 数据区不完整就退出
				if (buf + 4 + dataLen > end) break;

				// 跳到数据区开始调用处理回调
				buf += 4;
				{
					if (isBuf) {
						// 借壳反序列化
						auto& bb = ep->recvBB;
						bb.Reset((uint8_t*)buf, dataLen);

						// 需要反序列化填充的数据
						int serial = 0;

						// 反序列化失败：断开并 return
						if (bb.Read(serial)) {
							OnDisconnect(-6);
							if (!alive) return;
							Dispose();
							return;
						}
						else if (serial == 0) {
							if (onReceivePushBuf) {
								onReceivePushBuf(&bb);
							}
						}
						else if (serial < 0) {
							if (onReceiveRequestBuf) {
								onReceiveRequestBuf(serial, &bb);
							}
						}
						else {
							auto&& iter = requestBufCBs.find(serial);
							if (iter == requestBufCBs.end()) return;
							auto&& a = std::move(iter->second.first);
							// 析构 超时检测 timer
							iter->second.second->Dispose();
							requestBufCBs.erase(iter);
							a(&bb);
						}
					}
					else {
						// 借壳反序列化
						auto& bb = ep->recvBB;
						bb.Reset((uint8_t*)buf, dataLen);

						// 需要反序列化填充的数据
						int serial = 0;
						Object_s msg;

						// 反序列化失败：断开并 return
						if (bb.Read(serial) || bb.ReadRoot(msg)) {
							OnDisconnect(-6);
							if (!alive) return;
							Dispose();
							return;
						}
						else if (serial == 0) {
							if (onReceivePush) {
								onReceivePush(std::move(msg));
							}
						}
						else if (serial < 0) {
							if (onReceiveRequest) {
								onReceiveRequest(serial, std::move(msg));
							}
						}
						else {
							auto&& iter = requestCBs.find(serial);
							if (iter == requestCBs.end()) return;
							auto&& a = std::move(iter->second.first);
							// 析构 超时检测 timer
							iter->second.second->Dispose();
							requestCBs.erase(iter);
							a(std::move(msg));
						}
					}
					// 如果当前类实例已自杀则退出
					if (!alive) return;
				}

				// 跳到下一个包的开头
				buf += dataLen;
			}

			// 移除掉已处理的数据
			this->recv.RemoveFront((char*)buf - this->recv.buf);
		}

	public:
		~TcpPeerEx() {
			// 析构所有 超时检测 timer
			for (auto&& iter : requestCBs) {
				if (iter.second.second) {
					iter.second.second->Dispose();
				}
			}
			for (auto&& iter : requestBufCBs) {
				if (iter.second.second) {
					iter.second.second->Dispose();
				}
			}
		}
	};
	using TcpPeerEx_r = Ref<TcpPeerEx>;


	// 支持增强版 TcpPeer 的监听器
	struct TcpListenerEx : TcpListener {
		// 连接接受时触发
		std::function<void(TcpPeerEx_r const& peer)> onAccept;

	protected:
		inline virtual TcpPeer_u OnCreatePeer() override {
			return xx::TryMakeU<TcpPeerEx>();
		}

		inline virtual void OnAccept(TcpPeer_r const& peer) override {
			if (onAccept) {
				onAccept(peer.As<TcpPeerEx>());
			}
		}
	};
	using TcpListenerEx_r = Ref<TcpListenerEx>;


	// 支持增强版 TcpPeer 的拨号器
	struct TcpDialerEx : Dialer {
		// 连接成功或超时触发
		std::function<void(TcpPeerEx_r const& peer)> onConnect;

		inline int AddAddress(std::string const& ip, int const& port) {
			return this->Dialer::AddAddress(ip, port, Protocol::Tcp);
		}

	protected:
		inline virtual Peer_u OnCreatePeer(Protocol const& protocol) override {
			return xx::TryMakeU<TcpPeerEx>();
		}

		inline virtual void OnConnect(Peer_r const& peer) override {
			if (onConnect) {
				onConnect(peer.As<TcpPeerEx>());
			}
		}
	};
	using TcpDialerEx_r = Ref<TcpDialerEx>;
}
