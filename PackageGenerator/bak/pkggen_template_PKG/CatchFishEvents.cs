﻿#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace CatchFish
{
    // 事件相关, 主用于下发广播, 不含依赖类
    namespace Events
    {
        [Desc("事件基类")]
        class EventBase
        {
            [Desc("座位索引")]
            int sitIndex;
        }

        [Desc("玩家离开( 比进入的处理优先级高 )")]
        class PlayerLeave : EventBase
        {
        }

        [Desc("玩家进入")]
        class PlayerEnter : EventBase
        {
            [Desc("玩家Id")]
            int accountId;

            [Desc("进入的玩家拥有的金币数量")]
            long coin;

            [Desc("进入的玩家炮台ID")]
            CannonTypes cannonType;

            [Desc("进入的玩家上个炮台ID")]
            CannonTypes lastCannonType;

            [Desc("进入的玩家炮台倍率")]
            long bulletPower;

            [Desc("进入的玩家头像ID")]
            int avatar_id;

            [Desc("进入的玩家昵称")]
            string nickname;

            [Desc("进入的用户名")]
            string username;

            [Desc("累计充值金额")]
            double total_recharge;

            [Desc("是否是机器人")]
            int is_robot = 0;
            // todo: more info columns
        }


        [Desc("玩家开火锁定")]
        class FireLock : EventBase
        {
        }

        [Desc("玩家锁定后瞄准某鱼")]
        class FireAim : EventBase
        {
            [Desc("鱼流水号")]
            int fishSerialNumber;
        }

        [Desc("玩家开火解除锁定")]
        class FireUnlock : EventBase
        {
        }

        [Desc("玩家自动开火")]
        class FireAutomating : EventBase
        {
        }

        [Desc("玩家解除自动开火")]
        class FireUnAutomating : EventBase
        {
        }

        [Desc("玩家开启闪电武器")]
        class FireLightning : EventBase
        {
        }
        [Desc("玩家解除闪电武器")]
        class FireUnLightning : EventBase
        {

        }


        [Desc("玩家开火( 单次 )")]
        class Fire : EventBase
        {
            [Desc("起始帧编号")]
            int frameNumber;

            [Desc("子弹类型")]
            BulletTypes bulletType;

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
        class CannonSwitch : EventBase
        {
            [Desc("改变的炮台类型")]
            CannonTypes cannonType;

            [Desc("玩家索引")]
            int sitPlayerIndex;
        }

        [Desc("子弹命中( 与鱼死分离. 鱼死相关可能要等服务器跨线程回调送回结果才能下发 )")]
        class BulletHit : EventBase
        {
            [Desc("子弹流水号")]
            int bulletSerialNumber;
        }

        [Desc("子弹倍率改变")]
        class BulletPower : EventBase
        {
            [Desc("倍率值")]
            long power;
        }

        [Desc("鱼被打死")]
        class FishDead : EventBase
        {
            [Desc("鱼流水号")]
            int fishSerialNumber;

            [Desc("子弹流水号（非子弹发射客户端可以选择在鱼死时抹除相应的子弹）")]
            int bulletSerialNumber;

            [Desc("金币所得")]
            long coin;

            int cfgId;
            xx.Pos pos;
        }
        [Desc("特殊鱼被打死")]
        class SpecialFish_Dead : FishDead
        {
            long bulletPower;

            int fishCfgId;

            [Desc("炸死的鱼的包")]
            List<FishDead> fishDeads;
        }

        [Desc("改变鱼的状态(特殊状态)")]
        class ChangeState_Special: EventBase
        {
            int cfgId;

            int fishSerialNumber;

            int bulletSerialNumber;

            int state;

            int bombCount;

            List<xx.Pos> endMovePos;


        }

        [Desc("处理连环炸弹蟹爆炸")]
        class ContinuityBombCrab_Boom : FishDead
        {
            long bulletPower;

            int fishCfgId;

            int state;

            [Desc("已爆炸次数")]
            int fireCount;

            [Desc("炸死的鱼的包")]
            List<FishDead> fishDeads;

        }

        [Desc("玩家突然退出清除鱼包(特殊鱼)")]
        class Clean_Fish : EventBase
        {
            int fishSerialNumber;

        }
        [Desc("玩家突然退出清除特殊子弹")]
        class Clean_Bullet : EventBase
        {
            int bulletSerialNumber;

        }
        [Desc("玩家发射钻头弹")]
        class SendDrillBullet : EventBase
        {
            DrillBullet bullet;
        }

        [Desc("钻头弹炸死的鱼")]
        class DrillBoom : EventBase
        {
            int bulletSerialNumber;
            [Desc("爆炸总收益")]
            long coin;
            [Desc("钻头弹总收益")]
            long allCoin;
            [Desc("炸死的鱼的包")]
            List<FishDead> fishDeads;
        }
        [Desc("烈焰风暴游戏结算包")]
        class FireStormClear : EventBase
        {
            [Desc("总收益")]
            long allCoin;
            [Desc("剩余子弹收益")]
            long returnCoin;
          
        }

        [Desc("服务器根据某种事件 & 条件生成鱼下发")]
        class FishBorn
        {
            [Desc("鱼流水号( 用负数, 以便与自动生成的区分开来 )")]
            int fishSerialNumber;

            [Desc("鱼配置编号")]
            int cfgId;

            [Desc("鱼线配置编号")]
            int fishLineCfgId;

            [Desc("出生帧编号( 可能会比当前帧大, 相当于是批量预约下发出鱼 )")]
            int frameNumber;
        }
        [Desc("鱼")]
        class FishEvent : EventBase
        {
            [Desc("配置id( 用来还原配置 )")]
            int cfgId;


            [Desc("鱼线配置编号. 超出鱼线长则杀鱼( 用于网络同步 )")]
            int fishLineCfgId;


            [Desc("当前鱼线点下标")]
            int fishLinePointIndex;

            [Desc("当前鱼线点上的已前进距离")]
            float fishLinePointDistance;

            [Desc("帧图下标( 每帧累加这个以替代每次取余的慢速计算 )")]
            int spriteFrameIndex;


            [Desc("帧比值, 平时为 1, 如果为 0 则表示鱼不动( 比如实现冰冻效果 ), 帧图也不更新. 如果大于 1, 则需要在 1 帧内多次驱动该鱼( 比如实现快速离场的效果 )")]
            int frameRatio;

            [Desc("是否为在鱼线上倒着移动( 默认否 )")]
            bool reverse;

            [Desc("运行时缩放比例( 通常为 1 )")]
            float scale;

            [Desc("加速度")]
            float speedScale;

            [Desc("默认与 cfg 的 lockPoint 相同, 但如果该点不在屏内, 则取 cfg 之 lockPoints 形成的线与屏边的交点来填充该值. 通常只需要算一次( 龙除外 )")]
            xx.Pos lockPoint;

            [Desc("鱼的每帧移动距离")]
            float lineFrameDistance;

            [Desc("当前音效Id")]
            int effectId;

            [Desc("位于容器时的下标")]
            int indexAtContainer;

            [Desc("序列号( 当发生碰撞时用于标识 )")]
            int serialNumber;

            [Desc("创建时的帧编号( 用于推算鱼的已前进距离, 进而可查鱼线得到所有 )")]
            int bornFrameNumber;

            [Desc("当前位置( 用于鱼时, 不下发, 客户端要自己算 )")]
            xx.Pos pos;

        }
        [Desc("预约生成鱼类")]
        class MakeFish: EventBase
        {
            FishEvent fish;
            [Desc("鱼定时器")]
            int timer;
        }
        [Desc("玩家进入破产状态")]
        class BankRuptcy : EventBase
        {
        }

        [Desc("玩家充值")]
        class PlayerCharge : EventBase
        {
            [Desc("新增币值")]
            long coin;
        }

    }
}
