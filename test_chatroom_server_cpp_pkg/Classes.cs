#pragma warning disable 0169, 0414
using TemplateLibrary;

// 公共 & 服务器 数据结构
namespace Chat {
    [Desc("一个 房间. 可以被直接同步到 客户端"), Include]
    class Room {
        [Desc("房间名( key )")]
        string name;
        [Desc("用户列表")]
        List<Shared<User>> users;
        [Desc("消息历史记录")]
        List<Shared<Message>> messages;
    }
    [Desc("用户"), Include]
    class User {
        [Desc("用户名( key )")]
        string name;
        [Desc("当前所在 房间")]
        Weak<Room> room;
    }
    [Desc("一条基础聊天信息")]
    class Message {
        [Desc("服务器收到时间( 排序依据 )")]
        long createTime;
        [Desc("谁发的")]
        Weak<User> sender;
        [Desc("内容")]
        string content;
    }
    [Desc("一条豪华聊天信息( 体现下继承关系 )")]
    class MessageEx : Message {
        List<string> contents;
    }
}
// 客户端向服务器发送的包结构
namespace Client_To_ChatServer {
    [Desc("用户登录")]
    class Login {
        [Desc("用户名"), Limit(64)]
        string name;
    }
    [Desc("进入房间")]
    class Enter {
        [Desc("房间名"), Limit(64)]
        string roomName;
    }
    [Desc("发信息")]
    class Send {
        [Desc("内容"), Limit(1024)]
        string content;
        [Desc("豪华聊天内容( 可空 )"), Limit(16), Limit(1024)]
        Nullable<List<string>> contents;
    }
}
// 服务器向客户端发送的包结构
namespace ChatServer_To_Client {
    namespace Login {
        [Desc("用户登录 成功")]
        class Success {
            [Desc("房间名列表")]
            List<string> roomNames;
        }
        [Desc("用户登录 失败")]
        class Fail {
            [Desc("失败原因( 用户名重复? )")]
            string reason;
        }
    }
    namespace Enter {
        [Desc("进入房间 成功")]
        class Success {
            [Desc("房间( 完整同步 )")]
            Shared<Chat.Room> room;
        }
        [Desc("进入房间 失败")]
        class Fail {
            [Desc("失败原因( 错误的房间名? )")]
            string reason;
        }
    }
    namespace Send {
        [Desc("发信息 失败")]
        class Fail {
            [Desc("失败原因( 未进入房间? 超长? )")]
            string reason;
        }
        [Desc("发信息 成功")]
        class Success {
            [Desc("服务器收到时间")]
            long createTime;
        }
    }
    namespace Broadcast {
        [Desc("消息内容")]
        class Message {
            Shared<Chat.Message> message;
        }
    }
}
