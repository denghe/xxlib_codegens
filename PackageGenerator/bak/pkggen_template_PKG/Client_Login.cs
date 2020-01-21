#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Client_Login
{

    [Desc("客户端包类型验证")]
    class ClientPackagetType
    {
        [Limit(50), Desc("facebook账号/id")]
        string facebook;

        [Limit(64), Desc("用户名")]
        string username;

        [Limit(100), Desc("设备号(手机唯一编号),注册时写入(不修改)")]
        string device_id;

        [Desc("包类型(0=正常官网包;1=特殊推荐包)")]
        int packageType;
    }

    [Desc("客户端推广码验证")]
    class ClientPromotionCode
    {
        [Limit(50), Desc("facebook账号/id")]
        string facebook;

        [Limit(64), Desc("用户名")]
        string username;

        [Limit(100), Desc("设备号(手机唯一编号),注册时写入(不修改)")]
        string device_id;

        [Limit(300), Desc("推广码")]
        string promotion_code;
    }

    [Desc("客户端登陆类型")]
    class ClientType
    {
        [Limit(64), Desc("客户端登陆类型[unknown,windows,iphone10,xiaomi...]")]
        string clientType;

        [Desc("最后一次登录的手机系统(0 robot,1 andriod,2 ios,3 windows)")]
        int phoneType;

        [Limit(500), Desc("注册IP")]
        string createIp;

        [Limit(50), Desc("版本号")]
        string version;

        [Limit(300), Desc("包名")]
        string packageName;

        [Limit(300), Desc("设备号(手机唯一编号),注册时写入(不修改)")]
        string device_id;

        [Limit(50), Desc("内存信息")]
        string ram;

        [Limit(300), Desc("推广码")]
        string promotion_code;

        [Limit(50), Desc("facebook账号/id")]
        string facebook;
    }


    [Desc("验证包基类")]
    class Auth : ClientType
    {
        [Limit(64), Desc("包生成时的协议号")]
        string pkgGenMd5;


    }

    [Desc("手机登陆( 已过时，会直接用这个手机号来建 acc, 密码为空 )")]
    class AuthByPhone : Auth
    {
        [Limit(50), Desc("手机号")]
        string phone;
    }


    [Desc("游客 登录( 通过传空用户名来实现新用户注册 + 登录, 成功后记录到本地 )")]
    class AuthByUsername : Auth
    {
        [Limit(64), Desc("用户名")]
        string username;
    }

    [Desc("手机 + 密码 登陆")]
    class AuthByPhonePassword : Auth
    {
        [Limit(50), Desc("手机号")]
        string phone;

        [Limit(16), Desc("密码")]
        string password;
    }


    [Desc("请求下发短信")]
    class RequireVerifyCode
    {
        [Limit(50), Desc("用户填写的电话号码")]
        string phone;

        [Limit(50), Desc("区号")]
        string AreaCode;
    }


    [Desc("重置密码")]
    class ResetPassword
    {
        [Limit(50), Desc("用户填写的电话号码")]
        string phone;

        [Limit(16), Desc("短信校验码")]
        string verifyCode;

        [Limit(16), Desc("新密码")]
        string newPassword;

        [Limit(50), Desc("设备号(手机唯一编号)")]
        string device_id;
    }


    [Desc("获取热更新路径")]
    class GetUpdatePath
    {

    }

    [Desc("验证facebook")]
    class ClientVerificationFacebook
    {
        [Limit(300), Desc("token")]
        string token;
    }

}
