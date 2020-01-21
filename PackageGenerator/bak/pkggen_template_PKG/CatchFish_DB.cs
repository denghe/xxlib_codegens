#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace CatchFish_DB
{


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


    [Desc("更新总压总得数据")]
    class update_get_cost
    {
        [Desc("id(主键自增ID)")]
        int id;
        [Desc("总压")]
        double total_cost;
        [Desc("总得")]
        double total_get;
        [Desc("更新时间")]
        long updated_time;

    }

    [Desc("定时获取更新")]
    class get_dead_param_get_cost
    {
        [Desc("param_id")]
        int param_id;

        [Desc("总压")]
        double total_cost;

        [Desc("总得")]
        double total_get;
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

    [Desc("记录在不同游戏级别中各种类鱼的击杀次数和消耗情况")]
    class log_game_catchfish_fish_statistic
    {
        [Desc("游戏id")]
        int game_id;
        [Desc("游戏级别id")]
        int level_id;
        [Desc("鱼的种类id")]
        int fish_id;
        [Desc("倍数")]
        int bet;
        [Desc("赢分")]
        double win_score;
        [Desc("输分")]
        double fail_score;
        [Desc("尽分")]
        double all_score;
        [Desc("死亡次数")]
        int death;
        [Desc("打击次数")]
        int attack;
        [Desc("添加日期")]
        long addtime;
    }


    [Desc("记录在不同游戏级别中各种类鱼的击杀次数和消耗情况")]
    class catchfish_statistics
    {
        long push_time;

        List<log_game_catchfish_fish_statistic> fish_statistics;
    }

}

