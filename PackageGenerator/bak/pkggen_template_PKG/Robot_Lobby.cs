#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Robot_Lobby
{

    class RegisterRobotService
    {
        [Desc("机器人管理服务ID")]
        int serviceId;

        [Desc("等级Id")]
        int levelId;
    }

    [Desc("拉在线玩家列表")]
    class GetOnlines
    {

    }

    [Desc("更新游戏相关配置")]
    class UpdateGameConfig
    {

    }


    [Desc("机器人管理")]
    class UpdateRobot
    {

    }

    [Desc("获取机器人账号表")]
    class GetAccount
    {
        int level_id;
    }
    [Desc("生成机器人")]
    class MakeAccount
    {
        [Desc("总人数")]
        int numbers;
        int level_id;
    }
    [Desc("取游戏所有配置信息, 含游戏列表, 以及游戏下面所有子表配置")]
    class GetLobby
    {

    }

    [Desc("获取机器人需求量")]
    class RobotManager
    {

    }

    [Desc("改变机器人的money")]
    class ChangeMoney
    {
        int id;
        double money;
    }

}
