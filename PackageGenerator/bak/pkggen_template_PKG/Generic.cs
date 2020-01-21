#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Generic
{
    [Desc("通用返回")]
    class Success
    {
    }

    [Desc("通用返回失败")]
    class Fail
    {
        long number;
        string message;
    }

    [Desc("通用错误返回")]
    class Error
    {
        long number;
        string message;
    }

    [Desc("心跳保持兼延迟测试 -- 请求")]
    class Ping
    {
        long ticks;
    }

    [Desc("心跳保持兼延迟测试 -- 回应")]
    class Pong
    {
        long ticks;
    }

    [Desc("玩家一次游戏操作引起的金币变化")]
    class OnceChange
    {
        [Desc("账号id")]
        int accountId;

        [Desc("一次游戏操作的金币变化")]
        double onceChange;
    }
    [Desc("游戏中续命包(续命60秒)")]
    class KeepHeartbeat
    {
        [Desc("大厅与游戏协商的token")]
        [Limit(64)]
        string token;
    }

    [Desc("放分")]
    class UpdateGiftMoney
    {
        [Desc("账号ID")]
        int accountId;
        [Desc("实时钱")]
        double giftMoney;
        [Desc("鱼死亡比例")]
        int fish_dead_percent;
        [Desc("是否记录总压总得(1=记录；0=不记录[默认不记录])")]
        int is_cost_get;
    }

    [Desc("杀分")]
    class ChangeKillPercent
    {
        [Desc("账号Id")]
        int accountid;
        [Desc("杀分比例")]
        int kill_percent;
        [Desc("是否记录总压总得(1=记录；0=不记录[默认不记录])")]
        int is_cost_get;
    }

    [Desc("停止杀放分状态")]
    class StopWinLoseControl
    {
        [Desc("账号ID")]
        int accountId;
    }

    [Desc("slots同步当前玩家的放分值到大厅")]
    class NowWinControlMoney
    {
        [Desc("账号ID")]
        int accountId;
        [Desc("实时钱")]
        double giftMoney;
    }

    [Desc("slots跑马灯")]
    class SlotsMarquee
    {
        [Desc("玩家昵称")]
        string nickName;
        [Desc("游戏类型")]
        int gameId;
        [Desc("中奖金额")]
        double winMoney;
        [Desc("累计充值金额(计算vip等级)")]
        double totalRecharge;
    }

}
