#include "PKG_class.h"

struct Peer : EP::TcpPeerEx {
	// 和用户双向 bind. 以便于在收到数据时调用其处理函数
	std::weak_ptr<PKG::Chat::User> user;
};

struct Listener : EP::TcpListenerEx {
protected:
	// 直接创建派生类 Peer
	virtual EP::TcpPeer_u OnCreatePeer() override;
};

struct Server {
	EP::Context& ec;

	// 网络监听器
	EP::Ref<Listener> listener;

	// 通过了 Login, 没有进入房间的用户在此
	std::vector<std::shared_ptr<PKG::Chat::User>> users;

	// 房间列表. 用户会移动到具体房间. 每个房间里面也有 users 容器
	std::vector<std::shared_ptr<PKG::Chat::Room>> rooms;

	Server(EP::Context& ec);
	Server(Server const&) = delete;
	Server& operator=(Server const&) = delete;

	//
};



inline EP::TcpPeer_u Listener::OnCreatePeer() {
	return xx::TryMakeU<Peer>();
}

inline Server::Server(EP::Context& ec) : ec(ec) {
	// 弄几个房间出来
	{
		auto&& room = rooms.emplace_back();
		xx::MakeTo(room);
		room->name = "room_1";
	}
	{
		auto&& room = rooms.emplace_back();
		xx::MakeTo(room);
		room->name = "room_2";
	}

	// 初始化监听器
	if ((listener = ec.CreateTcpListener<Listener>(12345))) {

		// 建立连接之初, 状态为 匿名, 没有 User bind
		listener->onAccept = [this](EP::TcpPeerEx_r const& peer) {
			// 还原类型
			auto p = peer.As<Peer>();

			// 绑定匿名状态下的处理
			p->onReceiveRequest = [this, p](int const& serial, xx::Object_s&& msg) {
				// 根据 typeId 路由并处理
				switch (xx::TryGetTypeId(msg)) {
				case xx::TypeId_v<PKG::Client_To_ChatServer::Login>: {
					auto&& o = xx::As<PKG::Client_To_ChatServer::Login>(msg);
					// 参数检查. 0 长 不让登录
					if (!o->name.size()) {
						// 下发错误提示并延迟掐线
						auto&& m = xx::Make<PKG::ChatServer_To_Client::Login::Fail>();
						m->reason = "error: name is empty";
						p->SendResponse(serial, m);
						p->DelayDispose(3);
						return;
					}

					// 创建用户, 双向 bind
					auto&& u = users.emplace_back();
					xx::MakeTo(u);
					u->name = o->name;
					//u->room = nullptr;
					u->peer = p;
					p->user = u;

					// 绑定已登录状态下的处理
					p->onReceiveRequest = [u](int const& serial, xx::Object_s&& msg) {
						// todo
					};



					// todo: 下发 登录成功?
					//// 重设断线超时( 当收到合法数据时 )
					//p->SetTimeout(5);
					//// 数据发回
					//p->SendResponse(serial, msg);

					break;
				}
				default: {
					p->Dispose();
					return;
				}
				}
			};
			// 开启超时断线
			peer->SetTimeout(5);
		};
	}
}



int main(int argc, char** argv) {
	EP::Context ec;
	Server s(ec);
	if (!s.listener) return -1;
	int r = ec.Run(1);
	xx::CoutN("exit... r = ", r);
	std::cin.get();
	return r;
}





//// 模拟创建了一个房间
//auto&& room = std::make_shared<PKG::Chat::Room>();
//room->name = "r1";
//
//// 模拟进来一个用户
//auto&& user1 = room->users.emplace_back();
//xx::MakeTo(user1);
//user1->name = "u1";
//user1->room = room;
//
//// 模拟用户发言
//auto&& message = room->messages.emplace_back();
//xx::MakeTo(message);
//message->createTime = xx::NowEpoch10m();
//message->sender = user1;
//message->content = "m1";
//
//// 模拟又进来个用户
//auto&& user2 = room->users.emplace_back();
//xx::MakeTo(user2);
//user2->name = "u2";
//user2->room = room;
//
//// 模拟用户豪华发言
//auto&& messageEx = std::make_shared<PKG::Chat::MessageEx>();
//room->messages.emplace_back(messageEx);
//messageEx->createTime = xx::NowEpoch10m();
//messageEx->sender = user2;
//messageEx->content = "m2";
//messageEx->contents.emplace_back("asdf");
//messageEx->contents.emplace_back("qwer");
//
//// 看看输出
//xx::CoutN(room);
//
//// 看看序列化输出
//xx::Serializer ser;
//ser.WriteRoot(room);
//auto&& data = ser.GetData();
//xx::CoutN(data);
//
//// 反序列化对一下输出
//xx::Deserializer deser;
//deser.SetData(std::move(data));
//std::shared_ptr<xx::Object> o;
//int r = deser.ReadRoot(o);
//xx::CoutSN(o, r);
