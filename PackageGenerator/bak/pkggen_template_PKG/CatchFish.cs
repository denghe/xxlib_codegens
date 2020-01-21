#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace CatchFish
{
    enum PlayerStates
    {
        [Desc("等待连接中( 大厅通知游戏有玩家要进来, 此时创建的玩家就是这个状态. 或是玩家掉线, 也是这个状态. )")]
        Wait,
        [Desc("游戏中")]
        Play,
        [Desc("退出中( 准备向 db 发起 Leave 请求 )")]
        Leave0,
        [Desc("退出中( 正在等 db 的操作结果 )")]
        Leave1,
        [Desc("退出中( 正要向 lobby 发起请求 )")]
        Leave2,
        [Desc("退出中( 正在等 lobby 的操作结果 )")]
        Leave3,
        [Desc("退出中( 正要 Cleanup player )")]
        Leave4,
    }

    enum CannonTypes//炮台类型
    {
        Ordiary_Cannon = 0,//普通炮台
        Brass_Lighting,//黄铜闪电
        The_Snow_OF_Venus,//金星踏雪
        Fire_Flame_Cromn,//烈火焰冠
        Blue_Blood_Rosefinch,//碧血朱雀
        Silver_Cloud_Dragon,//银云青龙
        Phantom_Basal_Tic,//幻影玄武
        Firestorm,//烈焰风暴
        Electro_Drill//电钻

    };
    enum BulletTypes//子弹类型
    {
        ORDIARY_BULLET = 0,//普通子弹
        DOUBLE_BULLET,//双弹
        FirestormBullet//烈焰风暴
    };
    [Desc("玩家"), AttachInclude]
    class Player : PlayerBase
    {

        [Desc("位于容器时的下标")]
        int indexAtContainer;

        [Desc("座位索引, 同时也用于标识玩家")]
        int sitIndex;

        [Desc("玩家大炮类型")]
        CannonTypes cannonType;

        [Desc("玩家上个大炮类型")]
        CannonTypes lastCannonType;

        [Desc("当前子弹倍率")]
        long bulletPower;

        [Desc("玩家最近一次碰撞的子弹方向")]
        xx.Pos bulletMoveInc;

        [Desc("开火锁定状态")]
        bool fireLocking;

        [Desc("自动开火状态")]
        bool automating;

        [Desc("雷电武器开关")]
        bool fire_lightning;  

        [Desc("开火锁定状态期间被点选瞄准的鱼的编号( 用于还原 aimFish )")]
        int aimFishSerialNumber;

        [Desc("开火锁定状态期间被点选瞄准的鱼( 弱引用, 同步后需要手工还原 )")]
        Weak<Fish> aimFish;

        [Desc("破产标识")]
        bool isBankRuptcy;

        [Desc("子弹的自增流水号")]
        int bulletSerialNumber;

        [Desc("所有子弹( 当前不下发 )")]
        List<Bullet> bullets;

        [Desc("收包队列, 方便控制指令执行的生命周期")]
        [NotSerialize]
        List<Client_CatchFish.BattleCmd> recvs;

        [Desc("收到的最后一个开火包的帧编号")]
        [NotSerialize]
        int lastRecvFireFrameNumber;

        [Desc("上次请求时间")]
        long lastRequestTime = 0;

        [Desc("当玩家断线时, 令该值等于 frameNumber + 配置的断线等待时长. 当 !peer 与 fishScene->frameNumber >= waitFrameNumber 条件达成时, 将玩家状态转为 Leave, 开始存盘通知退出")]
        [NotSerialize]
        long waitFrameNumber = 0;

        [Desc("当前状态")]
        PlayerStates state;

        [Desc("所在场景")]
        Weak<Scene> scene;

        [Desc("当 Client 通过 Lobby 服务到 Game 发 Enter 时, Game 需要生成一个 token 以便 Client Enter 时传入以校验身份")]
        string token;

        [Desc("游戏币数. 当玩家进入到游戏时, 该值填充 money * exchangeCoinRatio. 玩家退出时, 做除法还原为 money.")]
        long coin;

        [Desc("开炮等行为花掉的金币数汇总")]
        long consume_coin;

        [Desc("打死钻头蟹时的子弹倍率")]
        long killDrillBulletPower;

        [Desc("钻头飞往炮台所需的延迟时间")]
        int drillCannonTime = -1;

        [Desc("钻头发射倒计时时间")]
        int drillDownTime = -1;

        [Desc("钻头弹发射标识符")]
        bool fireDrillBullet;

        [Desc("连环炸弹蟹爆炸标识符")]
        bool fireContinuityBombCrab;

        [Desc("烈焰风暴飞往炮台所需的延迟时间")]
        int fireCanoonTime = -1;


        [Desc("打死烈焰风暴时的子弹倍率")]
        long killFireStromBulletPower;

        [Desc("剩余烈焰风暴子弹数")]
        int fireStromBulletNumber = -1;

        [Desc("当前烈焰风暴子收益")]
        long curFireStromCoin;

        [Desc("剩余烈焰风暴游戏时间")]
        int fireStromTime = -1;

        [Desc("烈焰风暴游戏状态(0 为未开始，1 为正常， 2 为结束)")]
        int fireStromGameState;

        [Desc("玩家名下有未爆炸的炸弹蟹存在数")]
        int bombCrabNumber;

        [Desc("炮台旋转状态")]
        bool isRotate = true;
        [Desc("money 自动兑换成 coin 要 乘除 的系数")]
        int exchangeCoinRatio;

        [Desc("玩家打死鱼的倍率大于该值时，在跑马灯中显示")]
        int fish_ratio;

        [Desc("log 日志的自增ID")]
        int logAutoIncrementId;

        [Desc("上次记录的玩家的money")]
        double oldMoney;

        [Desc("杀分百分比（0~100）如果这个值>0 开始按比例丢弃子弹，并且放分不生效")]
        int kill_percent;

        [Desc("放钱的值")]
        double gift_money;

        [Desc("游戏id")]
        int gameId;

        [Desc("级别id")]
        int levelId;

        [Desc("房间id")]
        int roomId;

    }


    [Desc("Player类基类(account)")]
    class PlayerBase
    {
        [Desc("用户Id ( 随机 8 位整数 )")]
        int id;
        [Desc("原始用户名 唯一( GUID )")]
        string username;
        [Desc("昵称 唯一( 默认用某种规则生成 )")]
        string nickname;
        [Desc("头像")]
        int avatar_id;
        [Desc("网关")]
        uint gateWayId;
        [Desc("累计消费金额( 比如在鱼里一共打了多少钱的炮 )")]
        uint clientId;
        [Desc("电话号码 唯一( 默认填充 username GUID )")]
        string phone;
        [Desc("账户余额( 保留4位小数位, 进部分游戏时会被清0, 结束时会兑换返还 )")]
        double money;
        [Desc("累计充值金额")]
        double total_recharge;     
        [Desc("玩家进游戏身上的金币")]
        double enterMoney = 0;
        [Desc("玩家进游戏保险箱的金币")]
        double enterMoneySalfe = 0;
        [Desc("玩家进游戏身上的赠送金币")]
        double enterMoneyGift = 0;
        [Desc("玩家进游戏保险箱中的赠送金币")]
        double enterMoneyGiftSafe = 0;
        [Desc("正在游戏中的玩家日志id(为空 则离开游戏)")]
        long log_game_id;
        [Desc("是否机器人(1机器人)")]
        int is_robot;
        [Desc("玩家操作频率(1秒内只能发包一次，防止恶意发包)")]
        long currTicks = 0;
    }

    [Desc("鱼和子弹的基类"), AttachInclude]
    class MoveObject
    {
        [Desc("位于容器时的下标")]
        int indexAtContainer;

        [Desc("序列号( 当发生碰撞时用于标识 )")]
        int serialNumber;

        [Desc("创建时的帧编号( 用于推算鱼的已前进距离, 进而可查鱼线得到所有 )")]
        int bornFrameNumber;

        [Desc("当前位置( 用于鱼时, 不下发, 客户端要自己算 )")]
        xx.Pos pos;

    }

    [Desc("子弹"), AttachInclude]
    class Bullet : MoveObject
    {
        [Desc("每帧的直线移动坐标增量( 60fps )")]
        xx.Pos moveInc;

        [Desc("金币价值( 也可理解为倍率 )")]
        long coin;

        [Desc("子弹类型")]
        BulletTypes type;

        [Desc("渔网类型")]
        int fishNetType;
    }

    [Desc("钻头弹状态")]
    enum DrillBulletState
    {
        [Desc("移动产生碰撞")]
        Move,
        [Desc("即将爆炸")]
        Fire,
        [Desc("死亡爆炸")]
        End
    }
    [Desc("钻头弹"), AttachInclude]
    class DrillBullet : Bullet
    {
        [Desc("当前状态")]
        DrillBulletState state;

        [Desc("当前剩余倍率")]
        long curCoinNumber;

        [Desc("当前阶段已经进行帧数")]
        int curFrameNumber;

        [Desc("当前已经获得的金币数")]
        long curCoin;

        [Desc("Move阶段帧数")]
        int moveFrameNumber = 900;

        [Desc("Fire阶段帧数")]
        int fireFrameNumber = 300;

        [Desc("爆炸范围")]
        int boom_Radius = 500;

    }

    [Desc("鱼"), AttachInclude]
    class Fish : MoveObject
    {
        [Desc("配置id( 用来还原配置 )")]
        int cfgId;

        [Desc("鱼的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        FishConfig cfg;


        [Desc("鱼线配置编号. 超出鱼线长则杀鱼( 用于网络同步 )")]
        int fishLineCfgId;

        [Desc("鱼线( 不参与网络传输, 需要根据 fishLineCfgId 去 cfg.allFishLines 定位手工还原 )")]
        [NotSerialize]
        FishLine fishLine;


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

    }
    [Desc("背景"), AttachInclude]
    class Background_Config
    {
        [Desc("编号")]
        int cfgId;



        [Desc("渐隐透明度减量")]
        float opacityInc;

        [Desc("需要重复拼接的次数")]
        int repeatSplicingCount;

        [Desc("背景图")]
        string fileName;

    }

    class Background_KingCrab_Common_Config : Background_Config
    {

    }
    class Background_KingCrab_Boss_Config : Background_Config
    {

        [Desc("缩放数据(根据当前帧数为下标做对应缩放)")]
        List<float> scaleNumbers;

        [Desc("做动画部分图")]
        string stoneFileName;
    }
    class Background_Crocodile_Common_Config : Background_Config
    {
        [Desc("拼接用")]
        string fileName_Left;
        string fileName_Right;
    }
    //鳄鱼场景配置
    class Background_Crocodile_Boss_Config : Background_Config
    {

        [Desc("最大移动量")]
        float maxMoveNumber;

        [Desc("每帧移动量")]
        float frameMoveNumber;

        [Desc("动画图(做动画的部分)")]
        string fileName_Left;
        string fileName_Right;
    }

    class Background_NightTorches_Common_Config : Background_Config
    {

    }
    //暗夜炬兽场景配置
    class Background_NightTorches_Boss_Config : Background_Config
    {
        [Desc("黑幕特效文件名")]
        string effertFileName;

        [Desc("每帧移动量")]
        float FrameMoveNumber;


    }
    class Background_Octopus_Common_Config : Background_Config
    {

    }
    //八爪章鱼场景配置
    class Background_Octopus_Boss_Config : Background_Config
    {
        [Desc("背景图2")]
        string fileName2;

        [Desc("背景图3")]
        string fileName3;

        [Desc("中间的章鱼头特效2")]
        string effertFileName;
        [Desc("中间的章鱼头黑幕特效2")]
        string effertFileName2;
        [Desc("每帧的旋转度数")]
        float frameRotate;
        [Desc("旋转cd时间")]
        float rotateTime;
        [Desc("出生点")]
        List<BornPoint> bornPoints;

    }
    [Desc("八爪章鱼的出生点（洞）"), AttachInclude] 
    class BornPoint
    {
        xx.Pos pos;

        float rotation;

        float scale;

        int zOlder;

        [Desc("位于容器时的下标")]
        int indexAtContainer;
    }
    [Desc("背景"), AttachInclude]
    class Background
    {
        [Desc("编号")]
        int cfgId;

        [Desc("渐隐透明度(默认值1)")]
        float opacity;

    }


    class Background_KingCrab_Common : Background
    {
        [Desc("普通场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_KingCrab_Common_Config cfg;
    }
    [Desc("霸王蟹场景"), AttachInclude]
    class Background_KingCrab_Boss : Background
    {
        [Desc("boss场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_KingCrab_Boss_Config cfg;

        [Desc("缩放动画帧数(根据当前帧数为下标做对应缩放)")]
        int spriteIndex;
    }
    [Desc("鳄鱼普通场景"), AttachInclude]
    class Background_Crocodile_Common : Background
    {
        [Desc("普通场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_Crocodile_Common_Config cfg;
    }
    [Desc("鳄鱼boss场景"), AttachInclude]
    class Background_Crocodile_Boss : Background
    {
        [Desc("boss场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_Crocodile_Boss_Config cfg;

        [Desc("当前移动量")]
        float spritemove;

        [Desc("石块摇动方向标识(false为上true为下)")]
        bool move_Forward;

    }

    class Background_NightTorches_Common : Background
    {
        [Desc("普通场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_NightTorches_Common_Config cfg;
    }
    [Desc("暗夜巨兽boss场景"), AttachInclude]
    class Background_NightTorches_Boss : Background
    {
        [Desc("boss场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_NightTorches_Boss_Config cfg;

        [Desc("当前坐标")]
        xx.Pos spritePos;

        [Desc("当前坐标")]
        xx.Pos spritePos2;

    }
    class Background_Octopus_Common : Background
    {
        [Desc("普通场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_Octopus_Common_Config cfg;
    }
    [Desc("八爪章鱼boss场景"), AttachInclude]
    class Background_Octopus_Boss : Background
    {
        [Desc("boss场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_Octopus_Boss_Config cfg;

        [Desc("当前背景旋转度数")]
        float curRotation;

        [Desc("当次旋转已旋转度数")]
        float curTimeRotation;

        [Desc("当前距离下次旋转时间")]
        float curRotateTime;

    }

    [Desc("其他场景"), AttachInclude]
    class Background_Common : Background
    {
        [Desc("普通场景的配置信息( 不参与网络传输, 需要根据 cfgId 去 cfgs 定位手工还原 )")]
        [NotSerialize]
        Background_Config cfg;
    }

    [Desc("场景"), AttachInclude]
    class Scene
    {
        [Desc("帧编号, 每帧 + 1")]
        int frameNumber;

        [Desc("本地鱼生成专用随机数发生器")]
        xx.Random rnd;

        [Desc("服务器补鱼 或 客户端显示 用随机数发生器, 自行创建, 不做网络同步")]
        [NotSerialize]
        xx.Random rnd2;

        [Desc("所有玩家( 子弹在玩家下面 )")]
        List<Player> players;

        [Desc("鱼的自增流水号")]
        int fishSerialNumber;

        [Desc("鱼的自减流水号( 用于服务器下发鱼, 从 -1 开始 )")]
        int fishSerialNumber2;

        [Desc("所有鱼( 1级下标为 priority 优先级 )")]
        List<List<Fish>> fishss;

        [Desc("预约生成鱼数组")]
        List<Events.MakeFish> makeFishs;

        [Desc("被电死的鱼")]
        List<Fish> shockFishs;

        [Desc("游戏阶段( 含各种 emitters )")]
        Stage stage;

        [Desc("当前游戏阶段下标")]
        int stageIndex;

        [Desc("游戏背景")]
        List<Background> backgrounds;

        [Desc("存储暗夜炬兽")]
        [NotSerialize]
        Weak<Fish> nightTorches;

        [Desc("游戏id")]
        int gameId;

        [Desc("级别id")]
        int levelId;

        [Desc("房间id")]
        int roomId;

        [Desc("准入金")]
        double minMoney;

        [Desc("最低炮注")]
        long minBet;

        [Desc("最高炮注")]
        long maxBet;

        [Desc("进出游戏时 money 自动兑换成 coin 要 乘除 的系数")]
        int exchangeCoinRatio;

        [Desc("玩家打死鱼的倍率大于该值时，在跑马灯中显示")]
        int fish_ratio;

        [Desc("空闲座位下标( 初始时填入 0,1,2,3 )")]
        List<int> freeSitIndexs;

        [Desc("鱼种类(game_catchfish_fish)")]
        List<FishType> fishsTypes;

        [Desc("等级对应炮台区间值集合")]
        List<game_level_cannon> level_cannons;
    }

    [Desc(@"等级对应炮台区间值")]
    class game_level_cannon
    {
        [Desc(@"等级Id")]
        [Column] int level_id;
        [Desc(@"炮值")]
        [Column] int coin;
    }

    [Desc("鱼种类表"), AttachInclude]
    class FishType
    {
        [Desc("鱼配置id（cfgId）")]
        int id;
        [Desc("名称")]
        string name;
        [Desc("打死鱼的金币所得最小基数( 也可理解成倍率 )")]
        int minCoin;
        [Desc("打死鱼的金币所得最大基数( 也可理解成倍率 )")]
        int maxCoin;
    }

    [Desc("霸王蟹的几种状态")]
    enum KingCrabStates
    {
        [Desc("出生( 由小变大 )")]
        Born,
        [Desc("移动")]
        Move,
        [Desc("逃离( 从当前坐标开始向屏幕外移 )")]
        Escape
    }

    [Desc("霸王蟹")]
    class Fish_KingCrab : Fish
    {
        [Desc("当前阶段")]
        KingCrabStates state;

        [Desc("逃离阶段: 运行时的坐标增量")]
        xx.Pos escapePosInc;

        [Desc("用于记录当次移动策略要持续到多少 frameNumber")]
        int interval;

        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;

        // 初生阶段: 运行时的体积缩放, 基于 cfg 的 scale 再 * 一次. 初生时为 0, 渐变为 1. 每帧 + FishConfig_KingCrab.bornSizeScaleIncrease
        // 移动分 4 种情况: 顺/逆 * 正常/加速. 移动一个阶段之后, 再次随机这两个参数.
    }





    [Desc("鳄鱼的几种状态")]
    enum CrocodileStates
    {
        [Desc("移动( 接近炮台时会 跳起, 落下后会再次变为 移动 状态, 鱼线走完后会重新随机一根继续 移动2 )")]
        Move,
        [Desc("跳起( 嘴巴与炮台重叠时将临时改变 ZOrder 显示层级 )")]
        Jump,
        [Desc("移动( 接近炮台时会 跳起, 落下后的 移动 状态 )")]
        Move2,
        [Desc("逃离( 不再跳起且移速快 10 倍, 移到鱼线尽头即自杀 )")]
        Escape
    }

    [Desc("鳄鱼")]
    class Fish_Crocodile : Fish
    {
        [Desc("当前阶段")]
        CrocodileStates state;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
    }
    [Desc("暗夜炬兽的几种状态")]
    enum NightTorchesStates
    {
        [Desc("移动( 接近炮台时会 跳起, 落下后会再次变为 移动 状态, 鱼线走完后会重新随机一根继续 移动2 )")]
        Move,
        [Desc("逃离( 不再跳起且移速快 10 倍, 移到鱼线尽头即自杀 )")]
        Escape
    }
    [Desc("暗夜炬兽")]
    class Fish_NightTorches : Fish
    {
        NightTorchesStates state;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
    }
    [Desc("霸王鲸的几种状态")]
    enum TyrannosaurusStates
    {
        [Desc("移动")]
        Move,
        [Desc("逃离")]
        Escape
    }
    [Desc("霸王鲸")]
    class Fish_Tyrannosaurus : Fish
    {
        TyrannosaurusStates state;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
    }
    [Desc("大王乌贼的几种状态")]
    enum SepiaStates
    {
        [Desc("移动")]
        Move,
        [Desc("逃离")]
        Escape
    }
    [Desc("大王乌贼")]
    class Fish_Sepia : Fish
    {
        SepiaStates state;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
    }
    [Desc("赤焰龙龟的几种状态")]
    enum DragonTurtleStates
    {
        [Desc("翻滚")]
        Roll,
        [Desc("变身")]
        Change,
        [Desc("移动")]
        Move,

    }

    [Desc("赤焰龙龟")]
    class Fish_DragonTurtle : Fish
    {
        [Desc("当前阶段")]
        DragonTurtleStates state;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
        [Desc("每帧的直线移动坐标增量( 60fps )")]
        xx.Pos moveInc;
        [Desc("出生点")]
        xx.Pos bornPoint;
        [Desc("当前翻滚次数")]
        int curRollCounts;
        [Desc("此鱼的生成器结束时间")]
        int endFrameNumber;
    }
    [Desc("炸弹蟹的几种状态")]
    enum BombCrabStates
    {
        [Desc("移动()")]
        Move,
        [Desc("引爆( 被子弹打死后 )")]
        Fire,
        [Desc("死亡( 做完爆炸动画后 )")]
        Die
    }


    [Desc("炸弹蟹"), AttachInclude]
    class Fish_BombCrab : Fish
    {
        [Desc("当前阶段")]
        BombCrabStates state;
        [Desc("当前爆炸已过帧数")]
        int boomFrameNumber;
        [Desc("打死炸弹蟹的玩家的位置")]
        int sitIndex;
        [Desc("打死炸弹蟹的子弹的倍数")]
        long coin;
      
        [Desc("炸弹蟹红光缩放数组的下标")]
        int scaleIdnex;
        [Desc("打死炸弹蟹的子弹编号")]
        int bulletSeriNumber;
    }

    [Desc("连环炸弹蟹的几种状态")]
    enum ContinuityBombCrabStates
    {
        [Desc("移动()")]
        Move,
        [Desc("第一次被打死在原地等待时间到爆炸变成boom状态")]
        Fire,
        [Desc("发爆炸包,根据次数的多少马上改变到movefire或者die的状态")]
        Boom,
        [Desc("爆炸之后移动状态")]
        FireMove,
        [Desc("死亡( 做完爆炸动画后 )")]
        Die
    }


    [Desc("连环炸弹蟹"), AttachInclude]
    class Fish_ContinuityBombCrab : Fish
    {
        [Desc("当前阶段")]
        ContinuityBombCrabStates state;
        [Desc("当前爆炸已过帧数")]
        int boomFrameNumber;
        [Desc("打死炸弹蟹的玩家的位置")]
        int sitIndex;
        [Desc("打死炸弹蟹的子弹的倍数")]
        long coin;
        [Desc("炸弹蟹红光缩放数组的下标")]
        int scaleIdnex;
        [Desc("打死炸弹蟹的子弹编号")]
        int bulletSeriNumber;
        [Desc("轰炸次数")]
        int bombCount;
        [Desc("已轰炸次数")]
        int nowBombCount;
        [Desc("每次爆炸后移动的点")]
        List<xx.Pos> moveEndPos;
        [Desc("移动方向")]
        xx.Pos moveInc;
        [Desc("移动终点")]
        xx.Pos endMove;
        [Desc("累计炸死的钱")]
        long totalCoin;
        [Desc("每次爆炸给的子弹")]
        long bulletNumber;

        [Desc("剩余的子弹")]
        long remainingBulletNumber;

    }

    [Desc("八爪章鱼的状态")]
    enum OctopusStates
    {
        [Desc("出生()")]
        Born,
        [Desc("摆动(正常状态)")]
        Swing,
        [Desc("消失()")]
        Disappear,
    }
    [Desc("八爪章鱼"), AttachInclude]
    class Fish_Octopus : Fish
    {
        [Desc("当前阶段")]
        OctopusStates state;
        [Desc("当前摇摆时间")]
        int curTime;
        [Desc("当前出生冷却时间")]
        int curBronCDTime;
        [Desc("是否不在出现")]
        bool isEscape;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
        [Desc("当前出生点")]
        BornPoint curPoint;
    }
    [Desc("龙虾将军的几种状态")]
    enum GeneralLobsterStates
    {
        [Desc("移动")]
        Move,
        [Desc("逃离")]
        Escape
    }
    [Desc("龙虾将军"), AttachInclude]
    class Fish_GeneralLobster : Fish
    {
        GeneralLobsterStates state;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
    }
    [Desc("电光水母的几种状态")]
    enum ElectroJellyStates
    {
        [Desc("移动")]
        Move,
        [Desc("逃离")]
        Escape
    }
    [Desc("电光水母")]
    class Fish_ElectroJelly : Fish
    {
        ElectroJellyStates state;
        [Desc("自用随机数发生器( 解决因服务器下发接收延迟而导致的鱼生成用随机数发生器值错乱问题 )")]
        xx.Random rnd;
    }
    [Desc("大盘鱼")]
    class Fish_Grail : Fish
    {
        [Desc("副鱼队列")]
        List<Fish> mateFishs;
        [Desc("偏移坐标List")]
        List<xx.Pos> offsetPosList;
        [Desc("鱼倍率")]
        int fishBet = 0;
        [Desc("鱼倍率")]
        int grailFishId = 0;
    }

    [Desc("虎头鲨")]
    class Fish_TigerHeadShark : Fish
    {
        [Desc("虎头鲨起始坐标")]
        xx.Pos startPos;

        [Desc("虎头鲨移动方向")]
        xx.Pos moveInc;

        [Desc("虎头鲨移动计数器")]
        int moveCount;

    }
}
