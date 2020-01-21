#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace CatchFish_Lobby
{

    class RegisterCatchFishService
    {
        [Desc("游戏服务ID")]
        int serviceId;

        [Desc("等级Id")]
        int levelId;
    }

    [Desc("算力注册, 申请监听端口")]
    class Register
    {
        [Desc("level_Id")]
        int level_id;

        [Desc("能带多少间房的游戏实例")]
        int numRooms;

    }


    [Desc("玩家申请进入反馈")]
    class EnterRet
    {
        [Desc("0表示成功 1表示玩家的钱小于最低进入条件 2表示游戏内存在玩家信息")]
        int code;
    }


    enum RestoreStates
    {
        Success,
        Busy,
        NotFound
    }

    [Desc("玩家恢复连接结果")]
    class Restore_Result
    {
        [Desc("返回结果状态(只有success会携带token)")]
        RestoreStates state; 

        string ip;
        int port;
        string token;
    } 

    [Desc("玩家下线成功")]
    class KillPlayerSuccess
    {
        [Desc("更新后的token")]
        string token;

        [Desc("当前状态(0=等待连接中;1=游戏中;2=退出中( 准备向 db 发起 Leave 请求 );3=退出中( 正在等 db 的操作结果 );4=退出中( 正要向 lobby 发起请求 );5=退出中( 正在等 lobby 的操作结果 );6=退出中( 正要 Cleanup player ))")]
        int state;

    }


    [Desc("更改公告阅读状态")]
    class ChangeNoticeReadState
    {
        [Desc("id")]
        int id;

        int accountId;

    }

    [Desc("玩家金币记录")]
    class PlayerMoneyRecord
    {
        [Desc("日记Id")]
        long log_id;
        [Desc("账号ID")]
        int accountid;
        [Desc("上一次金币")]
        double last_money;
        [Desc("当前金币")]
        double cur_money;
        [Desc("当前绑定金币")]
        double cur_money_gift;
        [Desc("0,非游戏中,1 进入房间，2 游戏中 ,3离开房间")]
        int state;
    }

}
