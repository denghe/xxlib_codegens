#include "PKG_class.h"

struct Peer : EP::TcpPeerEx {
	// ���û�˫�� bind. �Ա������յ�����ʱ�����䴦����
	std::weak_ptr<PKG::Chat::User> user;
};

struct Listener : EP::TcpListenerEx {
protected:
	virtual EP::TcpPeer_u OnCreatePeer() override;
};

struct Server {
	EP::Context& ec;
	EP::Ref<Listener> listener;
	std::vector<std::shared_ptr<PKG::Chat::User>> users;
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
	// Ū�����������
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

	// ��ʼ��������
	if ((listener = ec.CreateTcpListener<Listener>(12345))) {

		// ��������֮��, ״̬Ϊ ����, û�� User bind
		listener->onAccept = [this](EP::TcpPeerEx_r const& peer) {
			peer->onReceiveRequest = [this, peer](int const& serial, xx::Object_s&& msg) {
				// ���� typeId ·�ɲ�����
				switch (xx::TryGetTypeId(msg)) {
				case xx::TypeId_v<PKG::Client_To_ChatServer::Login>: {
					auto&& o = xx::As<PKG::Client_To_ChatServer::Login>(msg);
					// �������. 0 �� ���õ�¼
					if (!o->name.size()) {
						// �·�������ʾ���ӳ�����
						auto&& m = xx::Make<PKG::ChatServer_To_Client::Login::Fail>();
						m->reason = "error: name is empty";
						peer->SendResponse(serial, m);
						peer->DelayDispose(3);
						return;
					}

					// �����û�, ˫�� bind, ���� peer
					auto&& u = users.emplace_back();
					xx::MakeTo(u);
					//u->


					break;
				}
				default: {
					peer->Dispose();
					return;
				}
				}

				// ������߳�ʱ( ���յ��Ϸ�����ʱ )
				peer->SetTimeout(5);

				// ���ݷ���
				peer->SendResponse(serial, msg);
			};
			// ������ʱ����
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





//// ģ�ⴴ����һ������
//auto&& room = std::make_shared<PKG::Chat::Room>();
//room->name = "r1";
//
//// ģ�����һ���û�
//auto&& user1 = room->users.emplace_back();
//xx::MakeTo(user1);
//user1->name = "u1";
//user1->room = room;
//
//// ģ���û�����
//auto&& message = room->messages.emplace_back();
//xx::MakeTo(message);
//message->createTime = xx::NowEpoch10m();
//message->sender = user1;
//message->content = "m1";
//
//// ģ���ֽ������û�
//auto&& user2 = room->users.emplace_back();
//xx::MakeTo(user2);
//user2->name = "u2";
//user2->room = room;
//
//// ģ���û���������
//auto&& messageEx = std::make_shared<PKG::Chat::MessageEx>();
//room->messages.emplace_back(messageEx);
//messageEx->createTime = xx::NowEpoch10m();
//messageEx->sender = user2;
//messageEx->content = "m2";
//messageEx->contents.emplace_back("asdf");
//messageEx->contents.emplace_back("qwer");
//
//// �������
//xx::CoutN(room);
//
//// �������л����
//xx::Serializer ser;
//ser.WriteRoot(room);
//auto&& data = ser.GetData();
//xx::CoutN(data);
//
//// �����л���һ�����
//xx::Deserializer deser;
//deser.SetData(std::move(data));
//std::shared_ptr<xx::Object> o;
//int r = deser.ReadRoot(o);
//xx::CoutSN(o, r);
