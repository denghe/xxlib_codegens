#include "xx_serializer.h"
#include "xx_epoll.h"
#include "PKG_class.h"

int main(int argc, char** argv) {

	// ģ�ⴴ����һ������
	auto&& room = std::make_shared<PKG::Chat::Room>();
	room->name = "r1";

	// ģ�����һ���û�
	auto&& user1 = room->users.emplace_back();
	xx::MakeTo(user1);
	user1->name = "u1";
	user1->room = room;

	// ģ���û�����
	auto&& message = room->messages.emplace_back();
	xx::MakeTo(message);
	message->createTime = xx::NowEpoch10m();
	message->sender = user1;
	message->content = "m1";

	// ģ���ֽ������û�
	auto&& user2 = room->users.emplace_back();
	xx::MakeTo(user2);
	user2->name = "u2";
	user2->room = room;

	// ģ���û���������
	auto&& messageEx = std::make_shared<PKG::Chat::MessageEx>();
	room->messages.emplace_back(messageEx);
	messageEx->createTime = xx::NowEpoch10m();
	messageEx->sender = user2;
	messageEx->content = "m2";
	messageEx->contents.push_back("asdf");
	messageEx->contents.push_back("qwer");

	// �������
	xx::CoutN(room);

	// �������л����
	xx::Serializer ser;
	ser.WriteRoot(room);
	auto&& data = ser.GetData();
	xx::CoutN(data);

	// �����л���һ�����
	xx::Deserializer deser;
	deser.SetData(std::move(data));
	std::shared_ptr<xx::Object> o;
	int r = deser.ReadRoot(o);
	xx::CoutSN(o, r);

	std::cin.get();
	return 0;
}
