#pragma warning disable 0169, 0414
using Generic;
using TemplateLibrary;
namespace CatchFish2_Lobby
{   
    class RegisterCatchFishService
    {
        [Desc("游戏服务ID")]
        int serviceId;
    }

    [Desc("玩家申请进入反馈")]
    class EnterRet
    {
        [Desc("0表示成功 1表示玩家的钱小于最低进入条件 2表示游戏内存在玩家信息")]
        int code;
    }

    [Desc("玩家离场, 退还游戏内金币")]
    class Leave
    {
        [Desc("日志编号( 用于记录游戏过程中的玩家事件日志 )")]
        long logId;

        [Desc("账号")]
        int accountId;

        [Desc("离开时游戏内剩余金币换算成的金额")]
        double leaveMoney;

        [Desc("离开时游戏内剩余的绑定金币转换算成金额")]
        double leaveMoneyGift;

        [Desc("以前到金币")]
        double oldMoney;

        [Desc("离开时游戏内打炮等消费行为所用金币换算成的金额")]
        double consume;

        [Desc("玩家上次游戏退出前用的子弹的倍率( 游戏内金币整数值 )")]
        long lastBet;

        [Desc("玩家上次游戏退出前用的炮台id")]
        CatchFish.CannonTypes lastCannonId;

        [Desc("是否机器人(1机器人)")]
        int is_robot;

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

    [Desc("更改公告阅读状态")]
    class ChangeNoticeReadState
    {
        [Desc("id")]
        int id;
        int accountId;
    }


    [Desc("更新放分的实时数据")]
    class UpdateGiftMoney
    {
        [Desc("账号ID")]
        int accountId;
        [Desc("实时钱")]
        double giftMoney;

    }


    [Desc("获取全服公告信息")]
    class GetAllServerNotice
    {
        [Desc("账号id")]
        int accountId;

    }
}

