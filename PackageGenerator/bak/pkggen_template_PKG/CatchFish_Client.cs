#pragma warning disable 0169, 0414
using TemplateLibrary;

// CatchFish Server 向 Client 发的包
namespace CatchFish_Client
{
    [Desc("退出游戏成功")]
    class Leave_Success
    {
        [Desc("大厅ip")]
        string lobbyIp;

        [Desc("大厅port")]
        int lobbyPort;

        [Desc("大厅token")]
        string lobbyToken;
    }

    [Desc("进入游戏成功")]
    class Enter_Success
    {
        [Desc("自己的座位索引")]
        int sitIndex;

        [Desc("游戏场景的完整数据")]
        CatchFish.Scene scene;
    }


    [Desc("玩家同步成功")]
    class Player_Syc
    {
        [Desc("plyaer的座位索引")]
        int sitIndex;

        [Desc("coin")]
        long coin;
    }


    [Desc("一帧内所有下发数据汇总( 推送 )")]
    class FramePackages
    {
        [Desc("下发时的帧编号")]
        int frameNumber;

        [Desc("鱼发生器")]
        List<CatchFish.Emitter> emitters;

        [Desc("所有事件包")]
        List<CatchFish.Events.EventBase> events;

    }

    [Desc("当前账号在其他设备上登录")]
    class OtherDeviceLogin
    {

    }

    [Desc("公告")]
    class Notice
    {
        [Desc("id")]
        int id;
        [Desc("标题")]
        string title;
        [Desc("内容")]
        string content;
        [Desc("发件人")]
        string sender;
        [Desc("创建时间")]
        long create_time;

    }


    [Desc("公告列表")]
    class GetNotices
    {
        [Desc("公告")]
        List<Notice> Notices;

    }
 
}

