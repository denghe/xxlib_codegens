#include "PKG_class.h"
#ifdef NEED_INCLUDE_PKG_class_hpp
#include "PKG_class.hpp"
#endif
#include "PKG_Chat_Room.hpp"
#include "PKG_Chat_User.hpp"


namespace PKG {
namespace Chat {
    uint16_t Message::GetTypeId() const noexcept {
        return 13;
    }
    Message::Message(Message&& o) {
        this->operator=(std::move(o));
    }
    Message& Message::operator=(Message&& o) {
        std::swap(this->createTime, o.createTime);
        std::swap(this->sender, o.sender);
        std::swap(this->content, o.content);
        return *this;
    }
    void Message::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->createTime);
        bb.Write(this->sender);
        bb.Write(this->content);
    }
    int Message::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->createTime)) return r;
        if (int r = bb.Read(this->sender)) return r;
        if (int r = bb.Read(this->content)) return r;
        return 0;
    }
    void Message::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.Chat.Message\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Message::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"createTime\":", this->createTime);
        xx::Append(s, ", \"sender\":", this->sender);
        xx::Append(s, ", \"content\":", this->content);
    }
#ifndef CUSTOM_INITCASCADE_PKG_Chat_Message
    int Message::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Message::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t Room::GetTypeId() const noexcept {
        return 1;
    }
    Room::Room(Room&& o) {
        this->operator=(std::move(o));
    }
    Room& Room::operator=(Room&& o) {
        std::swap(this->name, o.name);
        std::swap(this->users, o.users);
        std::swap(this->messages, o.messages);
        return *this;
    }
    void Room::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->name);
        bb.Write(this->users);
        bb.Write(this->messages);
    }
    int Room::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->name)) return r;
        if (int r = bb.Read(this->users)) return r;
        if (int r = bb.Read(this->messages)) return r;
        return 0;
    }
    void Room::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.Chat.Room\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Room::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"name\":", this->name);
        xx::Append(s, ", \"users\":", this->users);
        xx::Append(s, ", \"messages\":", this->messages);
    }
#ifndef CUSTOM_INITCASCADE_PKG_Chat_Room
    int Room::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Room::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t User::GetTypeId() const noexcept {
        return 2;
    }
    User::User(User&& o) {
        this->operator=(std::move(o));
    }
    User& User::operator=(User&& o) {
        std::swap(this->name, o.name);
        std::swap(this->room, o.room);
        return *this;
    }
    void User::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->name);
        bb.Write(this->room);
    }
    int User::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->name)) return r;
        if (int r = bb.Read(this->room)) return r;
        return 0;
    }
    void User::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.Chat.User\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void User::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"name\":", this->name);
        xx::Append(s, ", \"room\":", this->room);
    }
#ifndef CUSTOM_INITCASCADE_PKG_Chat_User
    int User::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int User::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t MessageEx::GetTypeId() const noexcept {
        return 14;
    }
    MessageEx::MessageEx(MessageEx&& o) {
        this->operator=(std::move(o));
    }
    MessageEx& MessageEx::operator=(MessageEx&& o) {
        this->BaseType::operator=(std::move(o));
        std::swap(this->contents, o.contents);
        return *this;
    }
    void MessageEx::Serialize(xx::Serializer& bb) const noexcept {
        this->BaseType::Serialize(bb);
        bb.Write(this->contents);
    }
    int MessageEx::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = this->BaseType::Deserialize(bb)) return r;
        if (int r = bb.Read(this->contents)) return r;
        return 0;
    }
    void MessageEx::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.Chat.MessageEx\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void MessageEx::ToStringCore(std::string& s) const noexcept {
        this->BaseType::ToStringCore(s);
        xx::Append(s, ", \"contents\":", this->contents);
    }
#ifndef CUSTOM_INITCASCADE_PKG_Chat_MessageEx
    int MessageEx::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int MessageEx::CascadeCore(void* const& o) noexcept {
        if (int r = this->BaseType::Cascade(o)) return r;
        return 0;
    }
}
namespace Client_To_ChatServer {
    uint16_t Login::GetTypeId() const noexcept {
        return 3;
    }
    Login::Login(Login&& o) {
        this->operator=(std::move(o));
    }
    Login& Login::operator=(Login&& o) {
        std::swap(this->name, o.name);
        return *this;
    }
    void Login::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->name);
    }
    int Login::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.ReadLimit<64>(this->name)) return r;
        return 0;
    }
    void Login::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.Client_To_ChatServer.Login\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Login::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"name\":", this->name);
    }
#ifndef CUSTOM_INITCASCADE_PKG_Client_To_ChatServer_Login
    int Login::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Login::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t Enter::GetTypeId() const noexcept {
        return 4;
    }
    Enter::Enter(Enter&& o) {
        this->operator=(std::move(o));
    }
    Enter& Enter::operator=(Enter&& o) {
        std::swap(this->roomName, o.roomName);
        return *this;
    }
    void Enter::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->roomName);
    }
    int Enter::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.ReadLimit<64>(this->roomName)) return r;
        return 0;
    }
    void Enter::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.Client_To_ChatServer.Enter\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Enter::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"roomName\":", this->roomName);
    }
#ifndef CUSTOM_INITCASCADE_PKG_Client_To_ChatServer_Enter
    int Enter::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Enter::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t Send::GetTypeId() const noexcept {
        return 5;
    }
    Send::Send(Send&& o) {
        this->operator=(std::move(o));
    }
    Send& Send::operator=(Send&& o) {
        std::swap(this->content, o.content);
        std::swap(this->contents, o.contents);
        return *this;
    }
    void Send::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->content);
        bb.Write(this->contents);
    }
    int Send::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.ReadLimit<1024>(this->content)) return r;
        if (int r = bb.ReadLimit<16, 1024>(this->contents)) return r;
        return 0;
    }
    void Send::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.Client_To_ChatServer.Send\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Send::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"content\":", this->content);
        xx::Append(s, ", \"contents\":", this->contents);
    }
#ifndef CUSTOM_INITCASCADE_PKG_Client_To_ChatServer_Send
    int Send::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Send::CascadeCore(void* const& o) noexcept {
        return 0;
    }
}
namespace ChatServer_To_Client::Login {
    uint16_t Success::GetTypeId() const noexcept {
        return 6;
    }
    Success::Success(Success&& o) {
        this->operator=(std::move(o));
    }
    Success& Success::operator=(Success&& o) {
        std::swap(this->roomNames, o.roomNames);
        return *this;
    }
    void Success::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->roomNames);
    }
    int Success::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->roomNames)) return r;
        return 0;
    }
    void Success::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.ChatServer_To_Client.Login.Success\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Success::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"roomNames\":", this->roomNames);
    }
#ifndef CUSTOM_INITCASCADE_PKG_ChatServer_To_Client_Login_Success
    int Success::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Success::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t Fail::GetTypeId() const noexcept {
        return 7;
    }
    Fail::Fail(Fail&& o) {
        this->operator=(std::move(o));
    }
    Fail& Fail::operator=(Fail&& o) {
        std::swap(this->reason, o.reason);
        return *this;
    }
    void Fail::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->reason);
    }
    int Fail::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->reason)) return r;
        return 0;
    }
    void Fail::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.ChatServer_To_Client.Login.Fail\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Fail::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"reason\":", this->reason);
    }
#ifndef CUSTOM_INITCASCADE_PKG_ChatServer_To_Client_Login_Fail
    int Fail::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Fail::CascadeCore(void* const& o) noexcept {
        return 0;
    }
}
namespace ChatServer_To_Client::Enter {
    uint16_t Success::GetTypeId() const noexcept {
        return 8;
    }
    Success::Success(Success&& o) {
        this->operator=(std::move(o));
    }
    Success& Success::operator=(Success&& o) {
        std::swap(this->room, o.room);
        return *this;
    }
    void Success::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->room);
    }
    int Success::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->room)) return r;
        return 0;
    }
    void Success::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.ChatServer_To_Client.Enter.Success\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Success::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"room\":", this->room);
    }
#ifndef CUSTOM_INITCASCADE_PKG_ChatServer_To_Client_Enter_Success
    int Success::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Success::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t Fail::GetTypeId() const noexcept {
        return 9;
    }
    Fail::Fail(Fail&& o) {
        this->operator=(std::move(o));
    }
    Fail& Fail::operator=(Fail&& o) {
        std::swap(this->reason, o.reason);
        return *this;
    }
    void Fail::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->reason);
    }
    int Fail::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->reason)) return r;
        return 0;
    }
    void Fail::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.ChatServer_To_Client.Enter.Fail\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Fail::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"reason\":", this->reason);
    }
#ifndef CUSTOM_INITCASCADE_PKG_ChatServer_To_Client_Enter_Fail
    int Fail::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Fail::CascadeCore(void* const& o) noexcept {
        return 0;
    }
}
namespace ChatServer_To_Client::Send {
    uint16_t Fail::GetTypeId() const noexcept {
        return 10;
    }
    Fail::Fail(Fail&& o) {
        this->operator=(std::move(o));
    }
    Fail& Fail::operator=(Fail&& o) {
        std::swap(this->reason, o.reason);
        return *this;
    }
    void Fail::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->reason);
    }
    int Fail::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->reason)) return r;
        return 0;
    }
    void Fail::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.ChatServer_To_Client.Send.Fail\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Fail::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"reason\":", this->reason);
    }
#ifndef CUSTOM_INITCASCADE_PKG_ChatServer_To_Client_Send_Fail
    int Fail::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Fail::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t Success::GetTypeId() const noexcept {
        return 11;
    }
    Success::Success(Success&& o) {
        this->operator=(std::move(o));
    }
    Success& Success::operator=(Success&& o) {
        std::swap(this->createTime, o.createTime);
        return *this;
    }
    void Success::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->createTime);
    }
    int Success::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->createTime)) return r;
        return 0;
    }
    void Success::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.ChatServer_To_Client.Send.Success\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Success::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"createTime\":", this->createTime);
    }
#ifndef CUSTOM_INITCASCADE_PKG_ChatServer_To_Client_Send_Success
    int Success::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Success::CascadeCore(void* const& o) noexcept {
        return 0;
    }
}
namespace ChatServer_To_Client::Broadcast {
    uint16_t Message::GetTypeId() const noexcept {
        return 12;
    }
    Message::Message(Message&& o) {
        this->operator=(std::move(o));
    }
    Message& Message::operator=(Message&& o) {
        std::swap(this->message, o.message);
        return *this;
    }
    void Message::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->message);
    }
    int Message::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->message)) return r;
        return 0;
    }
    void Message::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"PKG.ChatServer_To_Client.Broadcast.Message\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Message::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, ", \"message\":", this->message);
    }
#ifndef CUSTOM_INITCASCADE_PKG_ChatServer_To_Client_Broadcast_Message
    int Message::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Message::CascadeCore(void* const& o) noexcept {
        return 0;
    }
}
}
namespace PKG {
	AllTypesRegister::AllTypesRegister() {
	    xx::Deserializer::Register<PKG::Chat::Room>(1);
	    xx::Deserializer::Register<PKG::Chat::User>(2);
	    xx::Deserializer::Register<PKG::Chat::Message>(13);
	    xx::Deserializer::Register<PKG::Chat::MessageEx>(14);
	    xx::Deserializer::Register<PKG::Client_To_ChatServer::Login>(3);
	    xx::Deserializer::Register<PKG::Client_To_ChatServer::Enter>(4);
	    xx::Deserializer::Register<PKG::Client_To_ChatServer::Send>(5);
	    xx::Deserializer::Register<PKG::ChatServer_To_Client::Login::Success>(6);
	    xx::Deserializer::Register<PKG::ChatServer_To_Client::Login::Fail>(7);
	    xx::Deserializer::Register<PKG::ChatServer_To_Client::Enter::Success>(8);
	    xx::Deserializer::Register<PKG::ChatServer_To_Client::Enter::Fail>(9);
	    xx::Deserializer::Register<PKG::ChatServer_To_Client::Send::Fail>(10);
	    xx::Deserializer::Register<PKG::ChatServer_To_Client::Send::Success>(11);
	    xx::Deserializer::Register<PKG::ChatServer_To_Client::Broadcast::Message>(12);
	}
}
