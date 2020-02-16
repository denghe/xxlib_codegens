#include <xx_epoll.hpp>

namespace xx::Epoll {
	// 增强版( 仅支持直连 ). 支持 4字节 长度 包头拆包. 支持收发 xx::Objec
	struct TcpPeerEx : TcpPeer {
		// 收到 push 包时触发( 4 字节正数长度 + 0serial + Object )
		std::function<void(xx::Object_s && msg)> onReceivePush;

		// 收到 request 包时触发( 4 字节正数长度 + 负数serial + Object )
		std::function<void(int const& serial, xx::Object_s && msg)> onReceiveRequest;

		// 断开时触发. 之后会自动 Dispose
		std::function<void(int const& reason)> onDisconnect;
	protected:
		// SendRequest 的回调类型
		typedef std::function<int(Object_s && msg)> RequestCB;

		// 存放 SendRequest 的回调
		std::unordered_map<int, std::pair<RequestCB, Ref<Timer>>> requestCBs;

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

	protected:
		// 基础发 msg 包函数( 符合 header + serial + msg 的结构. 常见于直连通信 )
		inline void Send(int const& serial, Object_s const& msg) {
			auto&& s = ep->serializer;
			s.Reserve(1024);
			s.len = 4;	// 空出 长度 头部
			s.Write(serial);
			s.WriteRoot(msg);
			*(uint32_t*)s.buf = (uint32_t)(s.len - 4);	// 填充数据长度到包头
			this->TcpPeer::Send(s.GetData());
		}

		// 注意：可能 Dispose. 调用者需要做 alive 检查( 该备注向上感染 )
		inline virtual void OnDisconnect(int const& reason) override {
			if (onDisconnect) {
				onDisconnect(reason);
			}
		}

		// 数据接收事件: 从 recv 拿数据, 根据 4字节 长度 包头拆包 进一步 call OnReceiveXxxxxx
		// 注意：可能 Dispose. 调用者需要做 alive 检查( 该备注向上感染 )
		inline virtual void OnReceive() override {
			// 死亡判断变量
			Ref<Item> alive(this);

			// 数据偏移
			size_t offset = 0;

			// 确保包头长度充足
			while (offset + 4 <= recv.len) {
				// 取出数据区长度
				auto dataLen = *(int*)recv.buf + offset;

				// 判断 长度 合法性
				if (dataLen < 0 || dataLen > this->ep->maxPackageLength) {
					OnDisconnect(-5);
					// 如果当前类实例已自杀则退出
					if (!alive) return;
					Dispose();
					return;
				}

				// 数据区不完整就退出
				if (offset + 4 + dataLen > recv.len) break;

				// 跳到数据区开始调用处理回调
				offset += 4;
				{
					// 借壳 反序列化
					auto& ds = ep->deserializer;
					ds.Reset(recv.buf + offset, dataLen);

					// 需要填充的 序列号
					int serial = 0;
					Object_s msg;

					// 反序列化失败：断开并 return. 非 buf 模式需要同时反序列化 serial 和 msg
					if (ds.Read(serial) || ds.ReadRoot(msg)) {
						OnDisconnect(-6);
						if (!alive) return;
						Dispose();
						return;
					}

					if (serial == 0) {
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

					// 如果当前类实例已自杀则退出
					if (!alive) return;
				}

				// 跳到下一个包的开头
				offset += dataLen;
			}

			// 移除掉已处理的数据
			recv.RemoveFront(offset);
		}

	public:
		~TcpPeerEx() {
			// 析构所有 超时检测 timer
			for (auto&& iter : requestCBs) {
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







	// 增强版( 仅支持直连 ). 支持 4字节 长度 包头拆包. 支持收发 xx::Objec 或 buf + len
	struct TcpPeerEx2 : TcpPeerEx {
		// 收到 push buf 包时触发( 4 字节负数长度 + 0serial + byte[] ). 数据在 ds.buf + ds.offset 的位置
		std::function<void(xx::Deserializer & ds)> onReceivePushBuf;

		// 收到 request buf 包时触发( 4 字节负数长度 + 负数serial + byte[] ). 数据在 ds.buf + ds.offset 的位置
		std::function<void(int const& serial, xx::Deserializer & ds)> onReceiveRequestBuf;

	protected:
		// SendRequestBuf 的回调类型
		typedef std::function<int(xx::Deserializer * ds)> RequestBufCB;

		// 存放 SendRequestBuf 的回调
		std::unordered_map<int, std::pair<RequestBufCB, Ref<Timer>>> requestBufCBs;

		// 用于自增生成 SendRequest 的业务号
		int serial = 0;

	public:
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
		// 基础发 buf 包函数( 符合 header + msg + buf 的结构. 常见于直连通信 )
		template<typename...Args>
		void SendBuf(int const& serial, Args const& ... args) {
			auto&& s = ep->serializer;
			s.Reserve(64);
			s.len = 4;	// 空出 长度 头部
			s.WriteFixed(0xFFFFFFFFu);
			s.Write(serial);
			s.Write(args...);
			*(uint32_t*)s.buf = -(int)(uint32_t)(s.len - 4);	// 填充 负数 数据长度到包头
			this->TcpPeer::Send(s.GetData());
		}

		// 数据接收事件: 从 recv 拿数据, 根据 4字节 长度 包头拆包 进一步 call OnReceiveXxxxxx
		// 注意：可能 Dispose. 调用者需要做 alive 检查( 该备注向上感染 )
		inline virtual void OnReceive() override {
			// 死亡判断变量
			Ref<Item> alive(this);

			// 数据偏移
			size_t offset = 0;

			// 确保包头长度充足
			while (offset + 4 <= recv.len) {
				// 标识数据类型是否为 buf
				bool isBuf = false;

				// 取出数据区长度
				auto dataLen = *(int*)recv.buf + offset;

				// 如果为负数长度则标识为 buf 数据类型
				if (dataLen < 0) {
					isBuf = true;
					dataLen = -dataLen;
				}

				// 判断 长度 合法性
				if (dataLen > this->ep->maxPackageLength) {
					OnDisconnect(-5);
					// 如果当前类实例已自杀则退出
					if (!alive) return;
					Dispose();
					return;
				}

				// 数据区不完整就退出
				if (offset + 4 + dataLen > recv.len) break;

				// 跳到数据区开始调用处理回调
				offset += 4;
				{
					// 借壳 反序列化
					auto& ds = ep->deserializer;
					ds.Reset(recv.buf + offset, dataLen);

					// 需要填充的 序列号
					int serial = 0;
					Object_s msg;

					// 反序列化失败：断开并 return. 非 buf 模式需要同时反序列化 serial 和 msg
					if (ds.Read(serial) || (!isBuf && ds.ReadRoot(msg))) {
						OnDisconnect(-6);
						if (!alive) return;
						Dispose();
						return;
					}

					// buf 模式路由
					if (isBuf) {
						// 令 ds 看上去只含有 buf 区域的东西, offset & len 没有 serial 干扰
						ds.Reset(ds.buf + ds.offset, dataLen - ds.offset);
						if (serial == 0) {
							if (onReceivePushBuf) {
								onReceivePushBuf(ds);
							}
						}
						else if (serial < 0) {
							if (onReceiveRequestBuf) {
								onReceiveRequestBuf(serial, ds);
							}
						}
						else {
							auto&& iter = requestBufCBs.find(serial);
							if (iter == requestBufCBs.end()) return;
							auto&& a = std::move(iter->second.first);
							// 析构 超时检测 timer
							iter->second.second->Dispose();
							requestBufCBs.erase(iter);
							a(&ds);
						}
					}
					// msg 模式路由
					else {
						if (serial == 0) {
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
				offset += dataLen;
			}

			// 移除掉已处理的数据
			recv.RemoveFront(offset);
		}

	public:
		~TcpPeerEx2() {
			// 析构所有 超时检测 timer
			for (auto&& iter : requestBufCBs) {
				if (iter.second.second) {
					iter.second.second->Dispose();
				}
			}
		}
	};
	using TcpPeerEx2_r = Ref<TcpPeerEx2>;


	// 支持增强版 TcpPeer 的监听器
	struct TcpListenerEx2 : TcpListener {
		// 连接接受时触发
		std::function<void(TcpPeerEx2_r const& peer)> onAccept;

	protected:
		inline virtual TcpPeer_u OnCreatePeer() override {
			return xx::TryMakeU<TcpPeerEx2>();
		}

		inline virtual void OnAccept(TcpPeer_r const& peer) override {
			if (onAccept) {
				onAccept(peer.As<TcpPeerEx2>());
			}
		}
	};
	using TcpListenerEx2_r = Ref<TcpListenerEx2>;


	// 支持增强版 TcpPeer 的拨号器
	struct TcpDialerEx2 : Dialer {
		// 连接成功或超时触发
		std::function<void(TcpPeerEx2_r const& peer)> onConnect;

		inline int AddAddress(std::string const& ip, int const& port) {
			return this->Dialer::AddAddress(ip, port, Protocol::Tcp);
		}

	protected:
		inline virtual Peer_u OnCreatePeer(Protocol const& protocol) override {
			return xx::TryMakeU<TcpPeerEx2>();
		}

		inline virtual void OnConnect(Peer_r const& peer) override {
			if (onConnect) {
				onConnect(peer.As<TcpPeerEx2>());
			}
		}
	};
	using TcpDialerEx2_r = Ref<TcpDialerEx2>;

}
