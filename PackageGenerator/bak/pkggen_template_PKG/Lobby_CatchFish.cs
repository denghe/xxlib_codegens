#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Lobby_CatchFish
{
    [Desc("算力注册成功")]
    class Register_Success
    {
        [Desc("外部监听端口")]
        int port;
    }

    [Desc("玩家进入捕鱼游戏")]
    class Enter
    {





        [Desc("游戏id")]
        int gameId;

        [Desc("游戏名")]
        string gamename;

        [Desc("房间级别id")]
        int levelId;

        [Desc("房间id")]
        int roomId;

        [Desc("座位号")]
        int sitIndex;


        [Desc("网关")]
        uint gateWayId;

        [Desc("客服端")]
        uint clientId;






        [Desc("玩家id")]
        int accountId;

        [Desc("原始用户名 唯一( GUID )")]
        string userName;

        [Desc("昵称 唯一( 默认用某种规则生成 )")]
        string nickName;

        [Desc("头像")]
        int avatarId;

        [Desc("玩家进游戏身上的金币")]
        double enterMoney = 0;

        [Desc("玩家进游戏保险箱的金币")]
        double enterMoneySalfe = 0;

        [Desc("玩家进游戏身上的赠送金币")]
        double enterMoneyGift = 0;

        [Desc("玩家进游戏保险箱中的赠送金币")]
        double enterMoneyGiftSafe = 0;

        [Desc("进入时间( 所有进出时间均以大厅生成的为准 )")]
        long enterTime;

        [Desc("累计充值")]
        double total_recharge;

        [Desc(@"是否机器人(1机器人)")]
        int is_robot;


        [Desc("日志编号( 用于记录游戏过程中的玩家事件日志 )")]
        long logId;

        [Desc("玩家上次游戏退出前用的子弹的倍率( 游戏内金币整数值 )")]
        long lastBet;

        [Desc("玩家上次游戏退出前用的炮台id")]
        int lastCannonId;



        [Desc("最低倍率")]
        long minBet;

        [Desc("最高倍率")]
        long maxBet;

        [Desc("准入金额")]
        double minMoney;

        [Desc("进出游戏时 money 自动兑换成 coin 要 乘除 的系数")]
        int exchangeCoinRatio;

        [Desc("玩家打死鱼的倍率大于该值时，在跑马灯中显示")]
        int fish_ratio;



        [Desc("是否带入总压总得")]
        int is_cost_get;

        [Desc("杀分比")]
        int kill_percent;

        [Desc("鱼死亡比")]
        int fish_dead_proportion;

        [Desc("放的钱")]
        double gift_money;


        [Desc("捕鱼游戏鱼种类表")]
        List<CatchFish.FishType> fishs;

    }

    [Desc("从lobby Retore到游戏")]
    class Restore
    {
        [Desc("账号ID")]
        int accountId;
    }

    [Desc("让玩家下线")]
    class KillPlayer
    {
        [Desc("账号ID")]
        int accountId;

        [Desc("是否修改玩家游戏状态(true让玩家下线并开始结算；false 玩家下线，暂不结算[用于：断线重连、同一账号互挤])")]
        bool UpdatePlayerState;

    }

    [Desc("修改玩家杀分比例")]
    class ChangeKillPercent
    {
        int accountId;

        [Desc("杀分比例 0~100")]
        int kill_percent;

        [Desc("是否记录总压总得(1=记录；0=不记录[默认不记录])")]
        int is_cost_get;
    }

    [Desc("更新放分的实时数据")]
    class UpdateGiftMoney
    {
        [Desc("账号ID")]
        int accountId;
        [Desc("实时钱")]
        double giftMoney;
        [Desc("鱼死比例")]
        int fish_dead_proportion;
        [Desc("是否记录总压总得(1=记录；0=不记录[默认不记录])")]
        int is_cost_get;
    }

    class Leave
    {
        [Desc("账号ID")]
        int accountId;

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

    [Desc("游戏中续命包(续命60秒)")]
    class KeepHeartbeat
    {
        [Desc("账号ID")]
        int accountId;

    }

    [Desc("修改炮倍")]
    class UpdateLevelCannon
    {
        [Desc("等级对应炮台区间值集合")]
        List<CatchFish.game_level_cannon> level_cannons;
    }

    [Desc("玩家重连")]
    class ReEnter
    {
        [Desc("账号ID")]
        int accountId;

        [Desc("客户端连接Id")]
        uint clientId;

        [Desc("网关Id")]
        uint gateWayId;
    }

}
