#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace DB_CatchFish
{
 
    [Desc("获取捕鱼初始化参数")]
    class push_fish_dead_init_param
    {
        [Desc("总压总得")]
        game_catchfish_getcost get_cost;
        [Desc("算法参数")]
        game_catchfish_dead_param dead_param;

    }

    [Desc("等级对应炮台区间值")]
    class push_game_level_cannon
    {
        [Desc("等级对应炮台区间值集合")]
        List<CatchFish.game_level_cannon> level_cannons;

        [Desc("跑马灯配置")]
        List<marqueeconfig> marqueeconfigs;
    }

    [Desc("跑马灯配置")]
    class marqueeconfig
    {
        [Desc("游戏id")]
        int gameId;
        [Desc("触发跑马灯的最低中奖倍率")]
        int marquee_bet_limit;
        [Desc("触发跑马灯的最低中奖金额")]
        double marquee_money_limit;
    }

    [Desc("推送捕鱼参数变动")]
    class push_fish_dead_change_param
    {
        [Desc("算法参数")]
        game_catchfish_dead_param dead_param;

    }

    [Desc(@"算法参数")]
    class game_catchfish_dead_param
    {
        [Desc(@"自增id")]
        [Column(true)] int id;
        [Desc(@"配置id")]
        [Column] int param_id;
        [Desc(@"玩家胜率")]
        [Column] double radio;
        [Desc(@"最大波幅")]
        [Column] double max_ratio;
        [Desc(@"最小波幅")]
        [Column] double min_ratio;
        [Desc(@"起始时间")]
        [Column] long start_time;
        [Desc(@"结束时间")]
        [Column] long end_time;
    }


    [Desc(@"总压总得")]
    class game_catchfish_getcost
    {
        [Desc(@"自增ID")]
        [Column(true)] int id;
        [Desc(@"捕鱼游戏的Level_id;老虎游戏的Game_id")]
        [Column] int level_id;
        [Desc(@"1=捕鱼游戏配置；2=老虎机游戏配置")]
        [Column] int type;
        [Desc(@"上次游戏总压分")]
        [Column] double last_total_cost;
        [Desc(@"当前游戏压分")]
        [Column] double total_cost;
        [Desc(@"上次游戏总得分")]
        [Column] double last_total_get;
        [Desc(@"当前游戏得分")]
        [Column] double total_get;
        [Desc(@"创建时间")]
        [Column] long create_time;
        [Desc(@"最后更新时间")]
        [Column] long update_time;
        [Desc(@"胜率id")]
        [Column] int param_id;
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

    [Desc("更新总压总得成功")]
    class update_getcost_success
    {
        [Desc("最新的自增ID")]
        int id;
    }

}
