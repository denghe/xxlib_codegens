#pragma warning disable 0169, 0414
using TemplateLibrary;

// Client 向 CatchFish Server 发的包
namespace Client_CatchFish
{
    [Desc("请求进入游戏")]
    class Enter
    {
        [Desc("大厅与游戏协商的token")]
        [Limit(64)]
        string token;

        [Limit(64), Desc("MD5")]
        string pkgGenMd5;

    }


    [Desc("请求离开游戏( 玩家将立即切换到 存盘 模式, 向 db 发起存盘, 再向 lobby 发起退出, 在得到 lobby 返回的信息后, 发回给 客户端 )")]
    class Leave
    {

    }

    [Desc("请求同步")]
    class Sync
    {

    }

    [Desc("战斗指令之基类( 推送 )")]
    class BattleCmd
    {
        int frameNumber;

    }


    [Desc("子弹倍率改变")]
    class BulletPower : BattleCmd
    {
        [Desc("倍率值")]
        long power;
    }


    [Desc("玩家开火锁定")]
    class FireLock : BattleCmd
    {
    }

    [Desc("摇杆按钮判断")]
    class Rocker : BattleCmd
    {
        bool rocker;
    }

    [Desc("音乐按钮判断")]
    class BackgroundMusic : BattleCmd
    {
        bool backgroundMusic;
    }

    [Desc("音效按钮判断")]
    class SoundEffect : BattleCmd
    {
        bool soundEffect;
    }

    [Desc("玩家锁定后瞄准某鱼")]
    class FireAim : BattleCmd
    {
        [Desc("鱼流水号")]
        int fishSerialNumber;
    }


    [Desc("玩家开火解除锁定")]
    class FireUnlock : BattleCmd
    {
    }
    [Desc("玩家开启自动开火")]
    class FireAutomating : BattleCmd
    {
    }
    [Desc("玩家关闭自动开火")]
    class FireUnAutomating : BattleCmd
    {
    }

    [Desc("玩家开启闪电武器")]
    class FireLightning : BattleCmd
    {
    }
    [Desc("玩家解除闪电武器")]
    class FireUnLightning : BattleCmd
    {

    }

    [Desc("开火( 单次, 或频繁单次模拟伪连发 )")]
    class Fire : BattleCmd
    {
        [Desc("子弹类型")]
        CatchFish.BulletTypes bulletType;

        [Desc("子弹流水号")]
        int bulletSerialNumber;

        [Desc("子弹的初始位置")]
        xx.Pos pos;

        [Desc("金币价值( 也可理解为倍率 )")]
        long coin;

        [Desc("步进")]
        xx.Pos moveInc;

    }

    [Desc("炮台改变")]
    class CannonSwitch : BattleCmd
    {
        [Desc("改变的炮台类型")]
        CatchFish.CannonTypes cannonType;

        [Desc("玩家索引")]
        int sitPlayerIndex;

        [Desc("是否为解决客户端未找到鱼而从烈焰风暴转换回之前的炮台")]
        bool forError;
    }

    [Desc("当前玩家自己的子弹打中鱼")]
    class Hit : BattleCmd
    {
        [Desc("子弹流水号")]
        int bulletSerialNumber;

        [Desc("鱼流水号")]
        int fishSerialNumber;

        int cfgId;
    }
    [Desc("向服务器请求发射钻头子弹")]
    class SendDrillBullet : BattleCmd
    {
        [Desc("步进")]
        xx.Pos moveInc;
        [Desc("出生位置")]
        xx.Pos bornPos;
        [Desc("子弹编号")]
        int bulletSerialNumber;
    }

    [Desc("当前玩家Ping信息")]
    class PlayPing : BattleCmd
    {
        [Desc("发送时间")]
        long recvtime;
    }

    [Desc("更改公告阅读状态")]
    class ChangeNoticeReadState
    {
        [Desc("id")]
        int id;

    }

}

