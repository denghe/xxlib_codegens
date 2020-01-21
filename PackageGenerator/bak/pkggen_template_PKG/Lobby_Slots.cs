#pragma warning disable 0169, 0414
using TemplateLibrary;
namespace Lobby_Slots
{


    [Desc("玩家申请进入游戏")]
    class Enter
    {
        [Desc("网关")]
        uint gateWayId;

        [Desc("客服端")]
        uint clientId;

        [Desc("玩家id")]
        int accountId;

        [Desc("玩家所在到子库KEY")]
        string cb_key;

        [Desc("原始用户名 唯一( GUID )")]
        string userName;

        [Desc("昵称 唯一( 默认用某种规则生成 )")]
        string nickName;

        [Desc("累计充值金额(计算vip等级)")]
        double totalRecharge;

        [Desc("头像")]
        int avatarId;

        [Desc("进入时间( 所有进出时间均以大厅生成的为准 )")]
        long enterTime;

        [Desc("杀分比")]
        int kill_percent;

        [Desc("放的钱")]
        double gift_money;

        [Desc("是否带入总压总得")]
        int is_cost_get;

        [Desc("玩家钱")]
        double money;

        [Desc("保险箱")]
        double money_safe;

        [Desc("绑定的金币")]
        double money_gift;

        [Desc("绑定金币的保险箱")]
        double money_gift_safe;

        [Desc("玩家的洗码级别")]
        int wash_amount_lv;

        [Desc("玩家的洗码量")]
        double wash_amount_value;

        [Desc("玩家是否触发洗码派奖状态 0表示不触发 1表示进入派奖逻辑")]
        int wash_check;
    }

    [Desc("大厅通知游戏强制让玩家离开游戏")]
    class KillPlayer
    {
        [Desc("账号ID")]
        int accountId;
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
