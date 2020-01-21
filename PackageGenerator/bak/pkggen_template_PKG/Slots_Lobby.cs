#pragma warning disable 0169, 0414
using Generic;
using TemplateLibrary;
namespace Slots_Lobby
{
    class RegisterSlotsService
    {
        [Desc("游戏服务ID")]
        int serviceId;
        [Desc("游戏Id")]
        int gameId;
        [Desc("支持多少玩家")]
        int playerMaxNum;
    }

    [Desc("玩家申请进入反馈")]
    class EnterRet
    {
        [Desc("0表示成功 1表示玩家的钱小于最低进入条件 2表示游戏内存在玩家信息")]
        int code;
    }

    [Desc("游戏中的玩家正常离开")]
    class Leave
    {
        [Desc("用户Id")]
        int accountId;
    }

    [Desc("玩家锁住状态离开")]
    class LockLeave
    {
        [Desc("用户Id")]
        int accountId;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
    }

    [Desc("玩家一段时间游戏操作引起的金币变化")]
    class PlayingGameMoneyChanges
    {
        List<OnceChange> changes;
    }

    [Desc("通知大厅玩家的日志id")]
    class UpdateLogId
    {
        [Desc("用户Id")]
        int accountId;
        [Desc("本次进入游戏的日志id")]
        long logId;
    }
}

