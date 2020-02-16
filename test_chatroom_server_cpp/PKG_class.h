#pragma once
#include "xx_serializer.h"
#include"PKG_class.inc"

namespace PKG {
	struct PkgGenMd5 {
		inline static const std::string value = "#*MD5<ed159871457f9d4421b3ba1fff1dc19a>*#";
    };
	struct AllTypesRegister {
        AllTypesRegister();
    };
    inline AllTypesRegister allTypesRegisterInstance;

namespace Chat {
    // 一条基础聊天信息
    struct Message;
    // 一个 房间. 可以被直接同步到 客户端
    struct Room;
    // 用户
    struct User;
    // 一条豪华聊天信息( 体现下继承关系 )
    struct MessageEx;
}
namespace Client_To_ChatServer {
    // 用户登录
    struct Login;
    // 进入房间
    struct Enter;
    // 发信息
    struct Send;
}
namespace ChatServer_To_Client::Login {
    // 用户登录 成功
    struct Success;
    // 用户登录 失败
    struct Fail;
}
namespace ChatServer_To_Client::Enter {
    // 进入房间 成功
    struct Success;
    // 进入房间 失败
    struct Fail;
}
namespace ChatServer_To_Client::Send {
    // 发信息 失败
    struct Fail;
    // 发信息 成功
    struct Success;
}
namespace ChatServer_To_Client::Broadcast {
    // 消息内容
    struct Message;
}
namespace Chat {
    // 一条基础聊天信息
    struct Message : ::xx::Object {
        // 服务器收到时间( 排序依据 )
        int64_t createTime = 0;
        // 谁发的
        std::weak_ptr<PKG::Chat::User> sender;
        // 内容
        std::string content;
        XX_CODEGEN_CLASS_HEADER(Message, ::xx::Object)
    };
    // 一个 房间. 可以被直接同步到 客户端
    struct Room : ::xx::Object {
#include"PKG_Chat_Room.inc"
        // 房间名( key )
        std::string name;
        // 用户列表
        std::vector<std::shared_ptr<PKG::Chat::User>> users;
        // 消息历史记录
        std::vector<std::shared_ptr<PKG::Chat::Message>> messages;
        XX_CODEGEN_CLASS_HEADER(Room, ::xx::Object)
    };
    // 用户
    struct User : ::xx::Object {
#include"PKG_Chat_User.inc"
        // 用户名( key )
        std::string name;
        // 当前所在 房间
        std::weak_ptr<PKG::Chat::Room> room;
        XX_CODEGEN_CLASS_HEADER(User, ::xx::Object)
    };
    // 一条豪华聊天信息( 体现下继承关系 )
    struct MessageEx : PKG::Chat::Message {
        std::vector<std::string> contents;
        XX_CODEGEN_CLASS_HEADER(MessageEx, PKG::Chat::Message)
    };
}
namespace Client_To_ChatServer {
    // 用户登录
    struct Login : ::xx::Object {
        // 用户名
        std::string name;
        XX_CODEGEN_CLASS_HEADER(Login, ::xx::Object)
    };
    // 进入房间
    struct Enter : ::xx::Object {
        // 房间名
        std::string roomName;
        XX_CODEGEN_CLASS_HEADER(Enter, ::xx::Object)
    };
    // 发信息
    struct Send : ::xx::Object {
        // 内容
        std::string content;
        // 豪华聊天内容( 可空 )
        std::optional<std::vector<std::string>> contents;
        XX_CODEGEN_CLASS_HEADER(Send, ::xx::Object)
    };
}
namespace ChatServer_To_Client::Login {
    // 用户登录 成功
    struct Success : ::xx::Object {
        // 房间名列表
        std::vector<std::string> roomNames;
        XX_CODEGEN_CLASS_HEADER(Success, ::xx::Object)
    };
    // 用户登录 失败
    struct Fail : ::xx::Object {
        // 失败原因( 用户名重复? )
        std::string reason;
        XX_CODEGEN_CLASS_HEADER(Fail, ::xx::Object)
    };
}
namespace ChatServer_To_Client::Enter {
    // 进入房间 成功
    struct Success : ::xx::Object {
        // 房间( 完整同步 )
        std::shared_ptr<PKG::Chat::Room> room;
        XX_CODEGEN_CLASS_HEADER(Success, ::xx::Object)
    };
    // 进入房间 失败
    struct Fail : ::xx::Object {
        // 失败原因( 错误的房间名? )
        std::string reason;
        XX_CODEGEN_CLASS_HEADER(Fail, ::xx::Object)
    };
}
namespace ChatServer_To_Client::Send {
    // 发信息 失败
    struct Fail : ::xx::Object {
        // 失败原因( 未进入房间? 超长? )
        std::string reason;
        XX_CODEGEN_CLASS_HEADER(Fail, ::xx::Object)
    };
    // 发信息 成功
    struct Success : ::xx::Object {
        // 服务器收到时间
        int64_t createTime = 0;
        XX_CODEGEN_CLASS_HEADER(Success, ::xx::Object)
    };
}
namespace ChatServer_To_Client::Broadcast {
    // 消息内容
    struct Message : ::xx::Object {
        std::shared_ptr<PKG::Chat::Message> message;
        XX_CODEGEN_CLASS_HEADER(Message, ::xx::Object)
    };
}
}
namespace xx {
    template<> struct TypeId<PKG::Chat::Room> { static const uint16_t value = 1; };
    template<> struct TypeId<PKG::Chat::User> { static const uint16_t value = 2; };
    template<> struct TypeId<PKG::Chat::Message> { static const uint16_t value = 13; };
    template<> struct TypeId<PKG::Chat::MessageEx> { static const uint16_t value = 14; };
    template<> struct TypeId<PKG::Client_To_ChatServer::Login> { static const uint16_t value = 3; };
    template<> struct TypeId<PKG::Client_To_ChatServer::Enter> { static const uint16_t value = 4; };
    template<> struct TypeId<PKG::Client_To_ChatServer::Send> { static const uint16_t value = 5; };
    template<> struct TypeId<PKG::ChatServer_To_Client::Login::Success> { static const uint16_t value = 6; };
    template<> struct TypeId<PKG::ChatServer_To_Client::Login::Fail> { static const uint16_t value = 7; };
    template<> struct TypeId<PKG::ChatServer_To_Client::Enter::Success> { static const uint16_t value = 8; };
    template<> struct TypeId<PKG::ChatServer_To_Client::Enter::Fail> { static const uint16_t value = 9; };
    template<> struct TypeId<PKG::ChatServer_To_Client::Send::Fail> { static const uint16_t value = 10; };
    template<> struct TypeId<PKG::ChatServer_To_Client::Send::Success> { static const uint16_t value = 11; };
    template<> struct TypeId<PKG::ChatServer_To_Client::Broadcast::Message> { static const uint16_t value = 12; };
}
#include "PKG_class_end.inc"