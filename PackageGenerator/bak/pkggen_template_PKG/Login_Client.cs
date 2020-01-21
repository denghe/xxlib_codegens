#pragma warning disable 0169, 0414
using Lobby_Client;
using TemplateLibrary;

namespace Login_Client
{
    [Desc("校验成功, 下一步去连大厅")]
    class Auth_Success_Lobby
    {    

        [Desc("大厅token")]
        string lobbyToken;

        [Desc("用户名( 如果是新用户以用户名方式注册, 就夹带生成的 GUID 码返回以便客户端记录下来 )")]
        string username;

        [Desc("用户Id")]
        int accountId;

        [Desc("电话")]
        string phone;

        [Desc("facebook账号/id")]
        string facebook;    

        [Desc("游戏Id")]
        int gameId;

        [Desc("玩家信息")]
        selfAccount self;

        [Desc("vip信息")]
        List<Vip> vips;

        [Desc("ServerId")]
        int serviceId;

        [Desc("金币和真钱比例 1000 表示 1真钱=1000游戏币")]
        int money_exchange_coin;
    }

    [Desc("校验成功, 下一步去连游戏")]
    class Auth_Success_Game
    {
        [Desc("游戏类型编号")]
        int gameId;

        [Desc("游戏ip")]
        string gameIp;

        [Desc("游戏port")]
        short gamePort;

        [Desc("游戏token")]
        string gameToken;

        string username;
        int accountId;
        string phone;

        [Desc("facebook账号/id")]
        string facebook;
    }

    [Desc("请求下发短信 成功")]
    class RequireVerifyCode_Success
    {
        [Desc("剩余有效时长( 秒 )")]
        int leftElapsedSeconds;
    }

    [Desc("获取热更新路径")]
    class GetUpdatePath_Success
    {
        [Desc("大厅热更新路径")]
        string lobbyUpdatePath;

        [Desc("游戏热更新路径")]
        List<GameUpdatePath> gameUpdatePaths;
    }

    class GameUpdatePath
    {
        [Desc("游戏id")]
        int gameId;

        [Desc("游戏热更新路径")]
        string gameUpdatePath;
    }

    class GameEnforceUpdatePath
    {
        [Desc("版本号")]
        string version;

        [Desc("更新路径")]
        string updatePath;

    }

    [Desc("客户端包类型验证")]
    class ReceivedPackagetType
    {
        [Desc("验证状态(0=失败；1成功)")]
        int state;

        string message;
    }

    [Desc("客户端推广码验证")]
    class ReceivedPromotionCode
    {
        [Desc("验证状态(0=失败；1成功)")]
        int state;

        string message;
    }

    [Desc("验证facebook")]
    class ReceivedVerificationFacebook
    {
        [Desc("验证状态(0=失败；1成功)")]
        int state;
        [Desc("Facebook_id")]
        string facebook_id;
        [Desc("Facebook_name")]
        string facebook_name;
    }
}
