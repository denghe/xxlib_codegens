#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Lobby_Robot
{
    class RobotAccount
    {
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
    }
    [Desc("获取机器人账号信息")]
    class GetRobotAccount
    {
        [Desc("机器人账号")]
        List<RobotAccount> robots;

    }
    class UpdateRobot
    {
    }
    class RobotInfo
    {
        int gameId;

        int levelId;

        int roomId;

        int sitIndex;

        long enterTime = 0;
    }
    class PushRobot
    {
        List<RobotInfo> robots;
    }
    class PopRobot
    {
        List<RobotInfo> robots;
    }
    [Desc("获取机器人账号信息")]
    class PushRobotAccount
    {
        [Desc("机器人账号")]
        List<RobotAccount> robots;

    }


    [Desc("生成机器人账号信息")]
    class MakeRobotAccount
    {
        [Desc("机器人账号")]
        List<RobotAccount> robots;

    }

    [Desc(@"机器人管理")]
    class robot_manager
    {
        [Desc(@"游戏id")]
        [Column] int game_id;
        [Desc(@"游戏级别id. 0 1 2....: 新手厅  进阶厅  高手厅 ....")]
        [Column] int level_id;
        [Desc(@"机器人数量")]
        [Column] int number;
    }

    [Desc("获取机器人管理信息")]
    class GetRobotManager
    {
        [Desc("机器人管理")]
        List<robot_manager> robots;

    }
    [Desc("更改金币")]
    class ChangeMoney
    {

    }
}
