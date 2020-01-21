#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Lobby_Client
{
    [Desc("请求移动指令的处理结果的基类")]
    class Move_Result { }

    [Desc("请求进入大厅成功")]
    class Enter_Success : Move_Result
    {
        [Desc("游戏id列表")]
        List<int> gameIds;

        [Desc("当前玩家自己的信息")]
        selfAccount self;

        [Desc("vip")]
        List<Vip> vips;

        [Desc("官网")]
        string website;

        [Desc("老虎机游戏level集合")]
        List<GameEntryConditions> gameEntryConditionsList;

        [Desc("是否开启代理服务 1=开启 0=关闭")]
        int is_open_promotion;

        [Desc("金币和真钱比例 1000 表示 1真钱=1000游戏币")]
        int money_exchange_coin;

        [Desc("聊天服URL")]
        string contact_service;

        [Desc("图片上传URL")]
        string upload_image_path;
    }


    [Desc("玩家信息")]
    class UserInfo
    {
        [Desc("当前玩家自己的信息")]
        selfAccount self;

        [Desc("vip")]
        List<Vip> vips;
    }


    [Desc("VIP信息")]
    class Vip
    {
        int id;
        double total_recharge = 0;
    }

    [Desc("当前玩家自己的信息")]
    class selfAccount
    {
        [Desc("用户Id ( 随机 8 位整数 )")]
        int id;
        [Desc("原始用户名 唯一( GUID )")]
        string username;
        [Desc("昵称 唯一( 默认用某种规则生成 )")]
        string nickname;
        [Desc("头像")]
        int avatar_id;
        [Desc("电话号码 唯一( 默认填充 username GUID )")]
        string phone;
        [Desc("账户余额( 保留4位小数位, 进部分游戏时会被清0, 结束时会兑换返还 )")]
        double money;
        [Desc("锁定金币")]
        double money_gift;
        [Desc("保险箱( 玩家可在账户余额间搬运数据 )")]
        double money_safe;
        [Desc("锁定金币保险箱")]
        double money_gift_safe;
        [Desc("累计充值金额")]
        double total_recharge;     
        [Desc("累计提现金额")]
        double total_withdraw;
        [Desc("支付渠道基表id")]
        int? pay_channel_id;
        [Desc("是否有兑换权限(1有兑换权限)")]
        int has_withdrawals_purview;
        [Desc("玩家是否已设置赠送金币密码(1已设置)")]
        int is_gift_money_password;
        [Desc("玩家是否已获赠金币(1已赠送)")]
        int is_system_gift_money;
        [Desc("玩家是否有推广权限(1有)")]
        int is_popularize;
        [Desc("玩家是否已是代理(1=是)")]
        int is_promotion;
        [Desc("玩家是否有赠送金币权限(1有)")]
        int has_gift_money_purview;
        [Desc("这次登录是否SHOW每日签到(1=是)")]
        int is_signingave;
        [Desc("是否显示签到按钮(1=显示)")]
        int is_shows_signingave_button;
        [Desc("兑换最小金额")]
        double withdrawals_min_money;
        [Desc("兑换保底金额")]
        double withdrawals_min_remain_money;
        [Desc("兑换金额增幅")]
        double withdrawals_margin;
        [Desc("中大奖锁 1=锁")]
        int is_bigwin_lock;
        [Desc("可配置多少张银行卡")]
        int bankcardcount;
        [Desc("玩家绑定的支付信息")]
        List<PayChannelAccount> pay_channel_accounts;
    }

    [Desc("玩家绑定的支付信息")]
    class PayChannelAccount
    {
        [Desc("id")]
        int id;
        [Desc("pay_channel_id")]
        int pay_channel_id;
        [Desc("持卡人姓名/实名人姓名")]
        string name;
        [Desc("银行卡卡号/支付宝账号")]
        string card_number;
        [Desc("银行名称")]
        string bank_name;
    }


    [Desc("请求进入大厅成功(机器人管理者特殊)")]
    class Enter_Success_For_RobotManager
    {
        [Desc("初级场人数")]
        int primaryLevelNumber;

        [Desc("中级场人数")]
        int centerLevelNumber;

        [Desc("高级场人数")]
        int topLevelNumber;

    }

    [Desc("捕鱼游戏级别明细")]
    class GameCatchFish_LevelInfo : Move_Result
    {
        int levelId;

        [Desc("炮值(从)(游戏内金币)")]
        long minBet;

        [Desc("炮值(到)(游戏内金币)")]
        long maxBet;

        [Desc("最低准入金额")]
        double minMoney;

        [Desc("money 自动兑换成 coin 要 乘除 的系数")]
        int exchangeCoinRatio;
    }


    [Desc("请求进游戏 的执行结果( 捕鱼特化版 )")]
    class EnterGameCatchFish_Success : Move_Result
    {
        List<GameCatchFish_LevelInfo> levels;
    }

    [Desc("Huca游戏级别明细")]
    class GameHuca_LevelInfo : Move_Result
    {
        int levelId;

        [Desc("押注(从)(金额)")]
        double minBet;

        [Desc("押注(到)(金额)")]
        double maxBet;

        [Desc("最低准入金额")]
        double minMoney;

        [Desc("money 自动兑换成 coin 要 乘除 的系数")]
        int exchangeCoinRatio;
    }

    [Desc("请求进游戏 的执行结果")]
    class EnterGameHuca_Success : Move_Result
    {
        List<GameHuca_LevelInfo> levels;
    }

    [Desc("房间玩家信息")]
    class GameCatchFish_RoomInfo : Move_Result
    {
        [Desc("房间id")]
        int roomId;

        [Desc("所有座位玩家列表(定长数组)")]
        List<RoomPlayers> players;
    }

    [Desc("所有座位玩家列表")]
    class RoomPlayers
    {
        [Desc("用户Id ( 随机 8 位整数 )")]
        int id;
        [Desc("昵称 唯一( 默认用某种规则生成 )")]
        string nickname;
        [Desc("头像")]
        int avatar_id;
        [Desc("账户余额( 保留4位小数位, 进部分游戏时会被清0, 结束时会兑换返还 )")]
        double money;
        [Desc("是否机器人(1机器人)")]
        int is_robot;
    }


    [Desc("请求进入捕鱼游戏的某个级别 的执行结果")]
    class EnterGameCatchFishLevel_Success : Move_Result
    {
        List<GameCatchFish_RoomInfo> rooms;
    }


    [Desc("请求进捕鱼房并坐下 成功")]
    class EnterGameCatchFishLevelRoomSit_Success : Move_Result
    {
        [Desc("游戏服Id")]
        int GameId;

        [Desc("服务Id")]
        int serviceId;
    }

    [Desc("请求进入老虎机游戏 成功")]
    class EnterGameSlots_Success : Move_Result
    {
        [Desc("游戏服Id")]
        int GameId;

        [Desc("服务Id")]
        int serviceId;
    }



    [Desc("断线重连状态恢复请求 成功")]
    class Restore_Success : Move_Result
    {
        [Desc("每一个 moves 的成员的返回结果的打包")]
        List<Move_Result> moveResults;
    }




    // 事件推送集
    namespace Events
    {
        [Desc("有某玩家坐到了某座位上")]
        class PlayerSitdown
        {
            [Desc("目标玩家")]
            RoomPlayers player;

            [Desc("房间id")]
            int roomId;

            [Desc("座位下标")]
            int sitIndex;
        }

        [Desc("有某玩家离开了某座位")]
        class PlayerStandup
        {
            [Desc("房间id")]
            int roomId;

            [Desc("座位下标")]
            int sitIndex;
        }

      
    }




    [Desc("修改昵称 成功")]
    class ChangeNickname_Success
    {
        [Desc("修改后的昵称")]
        string nickname;
    }

    [Desc("修改头像 成功")]
    class ChangeAvatar_Success
    {
        [Desc("修改后的头像")]
        int avatar_id;
    }

    [Desc("请求下发短信 成功")]
    class RequireVerifyCode_Success : Login_Client.RequireVerifyCode_Success
    {
    }


    [Desc(@"申请提现表")]
    class withdrawals
    {
        [Desc(@"提现订单号( 时间戳 )")]
        [Column] long withdrawals_id;
        [Desc(@"帐号id")]
        [Column] int account_id;
        [Desc(@"提现金额")]
        [Column] double money;
        [Desc(@"手续费")]
        [Column] double server_fee_money;
        [Desc(@"请求提现的实名")]
        [Column] string cash_name;
        [Desc(@"提现请求的实名账号")]
        [Column] string cash_account;
        [Desc(@"如果是银行提现，则会显示银行名称")]
        [Column] string cash_bank_name;
        [Desc(@"0 打款成功或者退币中，1 退币成功")]
        [Column] int return_pay_status;
        [Desc(@"操作状态(0=apply; 1=success; 2=failed;)")]
        [Column] int state;
        [Desc(@"操作描述")]
        [Column] string description;
        [Desc(@"提现申请时间")]
        [Column] long create_time;
        [Desc(@"最后处理时间")]
        [Column] long updated_time;
        [Desc(@"支付渠道基表id，1 支付宝，2 银行卡")]
        [Column] int pay_channel_id;
        [Desc(@"提现前玩家的账户金额")]
        [Column] double before_money;
        [Desc(@"提现前玩家的保险箱金额")]
        [Column] double before_money_safe;
        [Desc(@"提现后玩家金币")]
        [Column] double after_money;
        [Desc(@"提现后玩家保险箱金币")]
        [Column] double after_money_safe;
        [Desc(@"是否为异常订单,0不是，1是")]
        [Column] int is_execption;
        [Desc(@"")]
        [Column] string ip;
        [Desc(@"")]
        [Column] string ip_area;
    }

    [Desc("获取玩家提现集合")]
    class GetWithdrawalsList
    {
        int accountId;

        List<withdrawals> withdrawals_List;
    }


    [Desc("保险箱充值/取出")]
    class ChangeMoneySafe_Success
    {
        [Desc("操作后的总金额")]
        double money;

        [Desc("操作后的保险箱金额")]
        double money_safe;

        [Desc("操作后的总绑定金额")]
        double money_gift;

        [Desc("操作后的保险箱绑定金额")]
        double money_gift_safe;
    }

    [Desc("...")]
    class TodayEarnMoney
    {
        [Column, Desc("名次")]
        int ranking;
        [Column, Desc("玩家id")]
        int account_id;
        [Column, Desc("玩家username")]
        string username;
        [Column, Desc("玩家nickname")]
        string nickname;
        [Column, Desc("玩家头像")]
        int avatar_id;
        [Column, Desc("玩家赢金币总量")]
        double money;
        [Desc("玩家Vip等级")]
        int vip;
    }

    [Desc("排行榜")]
    class GetTodayEarnMoneys : TodayEarnMoney
    {
        [Desc("排行榜")]
        List<TodayEarnMoney> todayEarnMoneys;
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
        [Desc("图片url(多个图片用逗号分隔)")]
        string urls;
        [Desc("发件人")]
        string sender;
        [Desc("创建时间")]
        long create_time;
        [Desc("是否已读(0未读, 1已读)")]
        int is_read;
        [Desc("1公告，2邮件)")]
        int type;
        [Desc("1全服公告，2大厅公告)")]
        int notice_type;
        [Desc("是否弹出显示(1弹出显示)")]
        int is_show;
        [Desc("对应模板key")]
        string template_key;     
    }

    [Desc("公告列表")]
    class GetNotices
    {
        [Desc("公告")]
        List<Notice> Notices;

        [Desc("需要玩家下载的APP的URL")]
        string appurl;

        [Desc("是否显示whats")]
        int app_switch;
    }

    [Desc("获取充值成功后的money")]
    class GetRechargeMoney
    {
        double money;

        double money_safe;

        double total_recharge;
    }

    [Desc("支付方式信息")]
    class PayChannel
    {
        [Column, Desc("支付方式id")]
        int id;
        [Column, Desc("支付方式名称")]
        string name;
        [Desc("是否添加识别金额")]
        int is_accuracy;
    }

    [Desc(@"VIP方式")]
    class vip_channel
    {
        [Desc(@"")]
        [Column(true)] int id;
        [Desc(@"代理昵称")]
        [Column] string nickname;
        [Desc(@"银行名称")]
        [Column] string bank_name;
        [Desc(@"卡号")]
        [Column] string account;
        [Desc(@"聊天方式 1：line2：Messenger3：twitter4：viber5：WhatsApp6：zalo")]
        [Column] int type;
        [Desc(@"是否显示中(1显示中)")]
        [Column] int enabled;
        [Desc(@"描述")]
        [Column] string description;
        [Desc(@"上线时间")]
        [Column] long create_time;
        [Desc(@"支付渠道Id")]
        [Column] int pay_channel_id;
    }

    [Desc(@"配置支付渠道金额")]
    class config_channel_money
    {
        [Desc(@"支付渠道Id")]
        [Column] int pay_channel_id;
        [Desc(@"金额")]
        [Column] int money;
    }


    [Desc("获取充值方式信息")]
    class GetRecharges
    {
        List<PayChannel> payChannels;
        List<vip_channel> vipChannels;
        List<config_channel_money> channelMoneys;
        [Desc("是否显示whats")]
        int app_switch;
        [Desc("最大充值金币上限")]
        double recharge_max_money;
        [Desc("识别额度")]
        double accuracy;
        [Desc("最小支付金额")]
        double recharge_min_money;
    }


    [Desc("获取兑换方式信息")]
    class GetWithdrawals
    {
        List<PayChannel> PayChannels;

        [Desc("金币和真钱比例")]
        int money_exchange_coin;
    }

    namespace Marquee
    {
        [Desc("获取跑马灯信息(系统跑马灯信息)")]
        class Marquee
        {
            [Desc("内容")]
            string content;

            [Desc("喊话频率(多长时间循环执行一次，单位 秒)")]
            int frequency;
        }

        [Desc("获取跑马灯信息(系统跑马灯信息)")]
        class GetGMMarquees
        {
            [Desc("跑马灯集合")]
            List<Marquee> marquees;
        }

        [Desc("获取跑马灯信息(游戏跑马灯信息)")]
        class GetMarquees
        {
            [Desc("跑马灯集合")]
            List<object> marquees;
        }

        [Desc("获取游戏服跑马灯信息(捕鱼游戏)")]
        class CatchFishMarquees
        {
            [Desc("游戏ID")]
            int game_id;

            [Desc("游戏名称")]
            string gameName;

            [Desc("玩家昵称")]
            string nickName;

            [Desc("累计充值金额(计算vip等级)")]
            double total_recharge;

            [Desc("赚的金币")]
            double coin;

            [Desc("子弹的倍率")]
            double bet;

            [Desc("打死的鱼id")]
            int fish_id;

            [Desc("打死的鱼名称")]
            string fish_name;

            [Desc("打死鱼的倍率")]
            int fish_ratio;
        }

        [Desc("获取捕鱼游戏跑马灯信息")]
        class GetCatchFishMarquees : GetMarquees
        {
        }

        [Desc("获取游戏服跑马灯信息(Huca游戏)")]
        class HucaMarquees
        {
            [Desc("游戏ID")]
            int game_id;

            [Desc("游戏名称")]
            string gameName;

            [Desc("玩家昵称")]
            string nickName;

            [Desc("累计充值金额(计算vip等级)")]
            double total_recharge;

            [Desc("赚的金币")]
            long coin;

            [Desc("押注金币")]
            long bet;

            [Desc("获取的倍数")]
            int marquee_bet;
        }

        [Desc("获取Huca游戏跑马灯信息")]
        class GetHucaMarquees : GetMarquees
        {
        }
    }



    [Desc("绑定手机成功")]
    class BindPhone_Success
    {
        [Desc("账户余额")]
        double money;
        [Desc("是否已赠送金币(1已赠送)")]
        int is_system_gift_money;
    }

    [Desc("金额变化通知")]
    class MoneyChanged
    {     
        [Desc("账户余额")]
        double money;

        [Desc("账户保险箱余额")]
        double money_safe;

        [Desc("绑定金币")]
        double money_gift;

        [Desc("绑定金币保险箱")]
        double money_gift_safe;

        [Desc("累计提现金额")]
        double total_withdraw;

        [Desc("累计充值金额")]
        double total_recharge;
    }

    [Desc("兑换结果金额变化通知")]
    class WithdrawalsResultMoneyChanged : MoneyChanged
    {
        [Desc("兑换金额")]
        double withdrawalsMoney;
    }

    [Desc("正式注册赠送金额")]
    class FormalRegistMoney
    {
        [Desc("赠送金额")]
        double money;
    }

    [Desc("发起充值成功返回数据给客户端")]
    class ClientRechargeRequestSuccess
    {
        string pay_url;
        string order_num;
    }

    [Desc("公司网站")]
    class Website
    {
        [Desc("官网地址")]
        string website;
        [Desc("官网二维码地址")]
        string website_qrcode;
    }

    [Desc("发起充值后的结果信息")]
    class ClientRechargeResult
    {
        [Desc("状态(0=success;1=fail)")]
        int state;

        [Desc("充值金额")]
        double money;

        [Desc("充值后玩家当前总金额")]
        double curr_money;

        double total_recharge;
    }

    [Desc("当前账号在其他设备上登录")]
    class OtherDeviceLogin
    {

    }

    [Desc("赠送金币的结果信息(赠送者)")]
    class GiftMoneyResult
    {
        [Desc("账户余额")]
        double money;

        [Desc("账户保险箱余额")]
        double money_safe;

    }

    [Desc("赠送金币的结果信息(获赠者)")]
    class ReceivedMoneyResult
    {
        [Desc("账户余额")]
        double money;

        [Desc("账户保险箱余额")]
        double money_safe;

        [Desc("累计充值金额")]
        double total_recharge;

        [Desc("赠送者id")]
        int gift_account_id;

        [Desc("赠送金额")]
        double gift_money;
    }

    [Desc("申请代理结果")]
    class ApplyPromotionResult
    {
        [Desc("状态(-1=未申请；0=申请中；1=成功；2=失败;)")]
        int state;

        [Desc("我的推广")]
        PromotionDetail promotion_detail;

        [Desc("我的利润")]
        PromotionProfit promotion_profit;

        [Desc("我的用户")]
        List<PromotionUser> promotion_user;

    }

    [Desc("代理信息")]
    class ReceivedPromotion
    {

        [Desc("是否是代理(1是代理 0是申请中 2是失败)")]
        int is_promoter;

        [Desc("是否已绑定上级代理(1已绑定)")]
        int is_superior;

        [Desc("银行账户")]
        string bank_card;

        [Desc("开户银行")]
        string bank_name;

        [Desc("手机号或者Viber账号")]
        string phone;

        [Desc("我的推广")]
        PromotionDetail promotion_detail;

        [Desc("我的利润")]
        PromotionProfit promotion_profit;

        [Desc("我的用户")]
        List<PromotionUser> promotion_user;


        [Desc("代理(推广员)兑换最小金额")]
        double promotion_withdrawals_min_money;

        [Desc("代理(推广员)兑换金额增幅")]
        double promotion_withdrawals_margin;
    }

    [Desc("我的推广")]
    class PromotionDetail
    {
        [Desc("推广链接")]
        string link_url;
        [Desc("推广二维码")]
        string qrcode_url;
        [Desc("推广码")]
        string promotion_code;
        [Desc("上级ID")]
        int superior_id;
        [Desc("ID")]
        int id;
        [Desc("团队人数")]
        int team_number;
        [Desc("直属代理")]
        int directly_promotion;
        [Desc("今日增加人数")]
        int today_add_number;
        [Desc("今日活跃人数")]
        int today_active_number;
    }

    [Desc("我的利润")]
    class PromotionProfit
    {
        [Desc("分成比例")]
        double profit_ratio;
        [Desc("利润")]
        double profit_money;
    }

    [Desc("我的用户")]
    class PromotionUser
    {
        [Desc("ID")]
        int id;
        [Desc("用户名")]
        string username;
        [Desc("昵称")]
        string nickname;
        [Desc("是否游客(1正式玩家)")]
        int is_tourist;
        [Desc("金币")]
        double money;
        [Desc("银行")]
        double money_safe;
        [Desc("总充值")]
        double total_recharge;
        [Desc("总提现")]
        double total_withdraw;
        [Desc("总输赢")]
        double total_win_lose;
        [Desc("时间")]
        long create_time;
    }

    [Desc("代理申请提现成功")]
    class PromotionWithdrawalsSuccess
    {
        [Desc("提现前利润")]
        double before_profit_money;

        [Desc("提现金额")]
        double withdrawals_money;

        [Desc("提现后利润")]
        double after_profit_money;
    }

    [Desc("代理提现列表")]
    class PromotionWithdrawalsList
    {
        List<PromotionWithdrawals> list;
    }

    [Desc("代理提现列表")]
    class PromotionWithdrawals
    {
        [Desc("订单号")]
        long withdrawals_id;
        [Desc("提现金额")]
        double money;
        [Desc("实际提现金额")]
        double actual_money;
        [Desc("银行卡卡号")]
        string bank_card;
        [Desc("开户行姓名")]
        string name;
        [Desc("银行名称")]
        string bank_name;
        [Desc("申请时间")]
        long create_time;
        [Desc("最早到账时间")]
        long arrival_time;
        [Desc("到账时间")]
        long update_time;
        [Desc("状态(0=apply; 1=success; 2=failed;)")]
        int state;
        [Desc("描述/备注")]
        string description;
    }

    [Desc("代理提现周期限制剩余时间(秒)")]
    class PromotionWithdrawalsCycleLimitdSurplusTime
    {
        [Desc("剩余多少时间才能提现(秒)")]
        long surplus_time;
    }

    [Desc("验证facebook")]
    class VerificationFacebook_Success
    {
        [Desc("账户余额")]
        double money;
        [Desc("是否已赠送金币(1已赠送)")]
        int is_system_gift_money;
        [Desc("facebook id")]
        string facebook;
    }


    [Desc(@"每日签到定义")]
    class everyday_signin_monery_config
    {
        [Desc(@"天")]
        [Column] int day;
        [Desc(@"送的金币")]
        [Column] double money;
    }

    [Desc("每日签到信息")]
    class EverydaySigninInfo
    {
        [Desc("当前能领取的格子Id(1-7),小于此值的表示已领取，大于此值表示未领取，但是不能领取")]
        int index;

        [Desc("每个格子送的钱，共7个")]
        List<everyday_signin_monery_config> moneylist;

        [Desc("今天是否领取")]
        int IsTodayComplete;
    }


    [Desc("签到反馈")]
    class SigninToDayResult
    {
        [Desc("获取了多少钱")]
        double give_money;

        [Desc("当前用户一共有多少钱")]
        double curr_money;

    }

    [Desc(@"玩家赠送金币记录表")]
    class log_gift_money_to_account
    {
        [Desc(@"")]
        [Column(true)] int id;
        [Desc(@"赠送金币玩家")]
        [Column] int give_account_id;
        [Desc(@"获赠金币玩家")]
        [Column] int received_account_id;
        [Desc(@"赠送金额")]
        [Column] double money;
        [Desc(@"赠送时间")]
        [Column] long create_time;
    }

    [Desc("玩家赠送记录")]
    class ReceivedGiftRecord
    {
        List<log_gift_money_to_account> gift_record;
    }

    [Desc("返回大厅")]
    class ReturnLobby
    {

    }

    [Desc("返回当前位置")]
    class GetPlaceRet
    {
        [Desc("0表示在大厅，大于0 等于游戏Id")]
        int Place;
    }

    [Desc("老虎机游戏的Level配置")]
    class GameEntryConditions
    {
        [Desc("游戏Id")]
        int gameid;

        [Desc("当前老虎机游戏的level")]
        List<EntryConditions> Entrys; 
    }

    [Desc("老虎机倍率区间")]
    class EntryConditions
    {
        [Desc("算法级别ID")]
        int id;

        [Desc("押注区间:最小值")]
        double spinMinMoney;

        [Desc("押注区间:最大值")]
        double spinMaxMoney;

        [Desc("进入该算法区间的最小值")]
        double enterMinMoney;

        [Desc("描述名称")]
        string desc;
    };

    [Desc("网络重新导向")]
    class ReRouteNetWork
    {
        [Desc("域名")]
        string host;

        [Desc("端口")]
        int port;
    }

    [Desc("银行卡渠道信息")]
    class ChangePayChannelBank_Success
    {
        List<PayChannelAccount> pay_channel_accounts;
    }

    [Desc("解锁大奖")]
    class UnLockAccount
    {

    }
}

