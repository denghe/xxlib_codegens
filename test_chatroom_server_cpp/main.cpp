#include "xx_serializer.h"
#include "xx_epoll.h"
#include "xx_epoll_tcp_ex.h"
#include "PKG_class.h"

namespace EP = xx::Epoll;
int main(int argc, char** argv) {
	EP::Context ec;
	auto&& listener = ec.CreateTcpListener<EP::TcpListenerEx>(12345);
	if (!listener) return -1;
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
