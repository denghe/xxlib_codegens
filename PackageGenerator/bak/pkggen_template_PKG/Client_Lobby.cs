#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Client_Lobby
{
    [Desc("在大厅中移动的所有指令的基类")]
    class Move
    {

    }


    [Desc("请求进入大厅( 下发游戏列表 )")]
    class Enter : Move
    {
        [Limit(64), Desc("大厅生成的 token")]
        string token;
    }

    [Desc("请求进入游戏( 下发具体游戏的下一级的数据. 捕鱼下发 levels )")]
    class EnterGame : Move
    {
        [Desc("游戏id")]
        int gameId;
    }

    [Desc("请求进入捕鱼游戏的某个级别( 将下发该级别下所有房间列表, 以及里面的玩家分布 )")]
    class EnterGameCatchFishLevel : Move
    {
        [Desc("捕鱼游戏级别id")]
        int levelId;
    }

    [Desc("请求进捕鱼房并坐下( 此动作可能触发动态开房 ). 一般性失败错误码为 0, 即因为时间差, 当前位置上已经有人了.")]
    class EnterGameCatchFishLevelRoomSit : Move
    {
        [Desc("捕鱼游戏房间id")]
        int roomId;

        [Desc("目标座位号( 0 ~ 3 ). 如果为 -1 表示自动选择")]
        int sitIndex;
    }

    [Desc("返回上一级菜单")]
    class ReturnUp : Move { }


    [Desc("断线重连状态恢复请求")]
    class Restore : Enter
    {
        [Desc("除开 Enter 的上面层层进入的包的打包体( 逻辑进入顺序需要正确 )")]
        List<Move> moves;
    }


    [Desc("获取自己的信息")]
    class OwnInfo
    {

    }





    [Desc("修改昵称")]
    class ChangeNickname
    {
        [Limit(50), Desc("昵称")]
        string nickname;

    }


    [Desc("修改头像")]
    class ChangeAvatar
    {
        [Desc("头像")]
        int avatar_id;

    }


    [Desc("为绑定电话 请求下发短信( 结构和用法同登录 请求下发短信 )")]
    class RequireVerifyCodeForBindPhone : Client_Login.RequireVerifyCode
    {
    }

    [Desc("为重置密码 请求下发短信( 结构和用法同登录 请求下发短信 )")]
    class RequireVerifyCodeForResetPassword : Client_Login.RequireVerifyCode
    {
    }

    [Desc("绑定电话, 成功返回 Success( 结构和用法同登录 重置密码 )")]
    class BindPhone : Client_Login.ResetPassword
    {
    }

    [Desc("重置密码, 成功返回 Success( 结构和用法同登录 重置密码 )")]
    class ResetPassword : Client_Login.ResetPassword
    {
    }

    // 听说当前没这个需求, 先放这
    [Desc("修改密码, 成功返回 Success")]
    class ChangePassword
    {
        [Limit(16), Desc("原先的密码")]
        string oldPassword;

        [Limit(16), Desc("新的密码")]
        string newPassword;

        [Limit(50), Desc("短信验证码内容")]
        string content;

    }


    [Desc("玩家申请提现")]
    class Withdrawals
    {
        [Desc("银行卡Id")]
        int id;
        [Desc("提现金额")]
        double money;
        [Desc("提现请求( 渠道, 账号等 )")]
        string requirement;
        [Desc("提现渠道(1=支付宝,2=银行卡)")]
        int pay_channel_id;

    }


    [Desc("获取玩家提现集合")]
    class GetWithdrawalsList
    {

    }


    [Desc("排行榜.")]
    class GetTodayEarnMoney
    {

    }


    [Desc("保险箱充值/取出.")]
    class ChangeMoneySafe
    {
        [Desc("账号id")]
        int accountId;

        [Desc("金额(>0充值，<0取出)")]
        double money;
    }


    [Desc("获取公告信息")]
    class GetNotice
    {

    }


    [Desc("在线聊天")]
    class Chat
    {
        [Desc("内容")]
        string content;
        [Desc("图片url(多个图片用逗号分隔)")]
        string urls;

    }


    [Desc("更改公告/邮件阅读状态")]
    class ChangeReadState
    {
        [Desc("id")]
        int id;

        [Desc("1公告，2邮件")]
        int type;

    }


    [Desc("获取跑马灯信息")]
    class GetMarquee
    {

    }


    [Desc("支付宝渠道信息")]
    class ChangePayChannelAlipay
    {
        [Desc("支付宝实名制姓名")]
        string name;
        [Desc("支付宝账号")]
        string account;
        [Desc("是否需要加上判断额0=不加1=加")]
        int is_accuracy;
    }


    [Desc("银行卡渠道信息")]
    class ChangePayChannelBank
    {
        [Desc("id")]
        int id;
        [Desc("银行卡开户人姓名")]
        string name;
        [Desc("银行卡卡号")]
        string card_number;
        [Desc("银行卡开户行")]
        string bank_name;
        [Desc("渠道Id")]
        int pay_channel_id;
    }


    [Desc("获取充值方式信息")]
    class GetRecharge
    {

    }


    [Desc("获取兑换方式信息")]
    class GetWithdrawals
    {

    }


    [Desc("获取正式注册赠送金额")]
    class GetFormalRegistMoney
    {

    }


    [Desc("客户端发起充值请求")]
    class ClientRechargeRequest
    {
        [Desc("充值的金额")]
        double money;
        [Desc("充值的方式,1 支付宝，2 银行卡, 3 微信，10 谷歌支付")]
        int pay_type;
        [Desc("ip信息")]
        string ip_info;
        [Desc("是否生成订单(1=生成)")]
        int is_create_order;
        [Desc("付款人姓名")]
        string pay_name;
        [Desc("付款人卡号")]
        string pay_card_number;
    }


    [Desc("玩家正常登出.")]
    class Login_out
    {

    }


    [Desc("获取公司网站")]
    class GetWebsite
    {

    }


    [Desc("获取客户端发起充值后的结果信息")]
    class GetClientRechargeResult
    {
        [Desc("packageName")]
        string packageName;

        [Desc("productId")]
        string productId;

        [Desc("token")]
        string token;

        [Desc("订单号")]
        string order_num;

    }

    [Desc("设置赠送密码")]
    class ClientGiftPassword
    {
        [Limit(50), Desc("密码")]
        string content;
    }

    [Desc("赠送金币")]
    class ClientGiftMoney
    {
        [Desc("获赠玩家id")]
        int account_id;
        [Desc("赠送金额")]
        double money;
        [Limit(50), Desc("密码")]
        string content;
    }

    [Desc("申请代理")]
    class ClientApplyPromotion
    {
        [Desc("代理名称")]
        string promotion_name;
        [Desc("facebook")]
        string facebook;
        [Desc("手机号码")]
        string phone_number;
        [Desc("语言(1=中文;2=英文；3=缅文；4=马来文)")]
        int language;
    }

    [Desc("代理信息")]
    class ClientPromotion
    {
        [Desc("语言(1=中文;2=英文；3=缅文；4=马来文)")]
        int language;
    }

    [Desc("绑定推广码")]
    class ClientBindPromotionCode
    {
        string promotion_code;
    }

    [Desc("代理申请提现")]
    class ClientApplyPromotionWithdrawals
    {
        double money;
        string bank_card;
        string bank_name;
        string phone;
        string description;
    }

    [Desc("代理提现列表")]
    class ClientPromotionWithdrawalsList
    {

    }

    [Desc("获取代理提现周期限制剩余时间")]
    class GetPromotionWithdrawalsCycleLimitdSurplusTime
    {

    }


    [Desc("验证facebook")]
    class ClientVerificationFacebook
    {
        [Limit(300), Desc("token")]
        string token;
    }

    [Desc("获取每日签到信息")]
    class Get_SigninGaveInfo { }

    [Desc("签到")]
    class SigninToDay { }

    [Desc("玩家赠送记录")]
    class ClientGiftRecord
    {

    }

    [Desc("客户端充值生成订单后，提交付款人信息")]
    class ClientRechargeRequestPayerInfo
    {
        [Limit(50), Desc("订单号")]
        string order_num;
        [Limit(250), Desc("付款人姓名")]
        string payer_name;
        [Limit(250), Desc("付款方式")]
        string payer_type;
    }    

    [Desc("获取当前位置")]
    class GetPlace
    {

    }
}
