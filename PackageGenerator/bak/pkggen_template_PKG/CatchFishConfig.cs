#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace CatchFish
{
    [Desc("鱼帧动画基本信息( 做死亡动画够用了 )"), AttachInclude]
    class FishSpriteFrameBase
    {
        [Desc("打包前的散文件名( 帧定位名 )")]
        string fileName;

    }


    [Desc("鱼帧动画信息"), AttachInclude]
    class FishSpriteFrame : FishSpriteFrameBase
    {
        [Desc("基于当前帧图的多边形碰撞顶点包围区( 由多个凸多边形组合而成 )")]
        List<List<xx.Pos>> polygons;

        [Desc("首选锁定点( 如果该点还在屏幕上, 则 lock 准星一直在其上 )")]
        xx.Pos lockPoint;

        [Desc("锁定点集合( 串成一条线的锁定点. 当首选锁定点不在屏上时, 使用该线与所在屏的边线的交点作为锁定点 )")]
        List<xx.Pos> lockPoints;

        [Desc("这一帧切到下一帧后应该移动的距离( 受 Fish.speedScale 影响 )")]
        float distance;
    }


    [Desc("鱼线点")]
    struct FishLinePoint
    {
        [Desc("x坐标")]
        float x;

        [Desc("y坐标")]
        float y;

        [Desc("角度")]
        float angle;

        [Desc("当前点到下一个点的物理/逻辑距离( 下一个点可能是相同坐标, 停在原地转身的效果 )")]
        float distance;
    }

    [Desc("鱼线")]
    class FishLine
    {
        [Desc("鱼线配置id. 对应到 cfg.allFishLines 中的下标")]
        int cfgId;

        [Desc("鱼线上的所有点")]
        List<FishLinePoint> points;

        [Desc("鱼线的总距离长度")]
        float distance;

        [Desc("是否为循环线( 即移动到最后一个点之后又到第 1 个点, 永远走不完")]
        bool loop;

    }


    [Desc("鱼配置信息"), AttachInclude]
    class FishConfig
    {
        [Desc("鱼配置id. 对应到 cfg.allFishs 中的下标")]
        int cfgId;

        [Desc("鱼名")]
        string name;


        [Desc("打死鱼的金币所得最小基数( 也可理解成倍率 )")]
        long minCoin;

        [Desc("打死鱼的金币所得最大基数( 也可理解成倍率 )")]
        long maxCoin;

        [Desc("基于整个鱼的最大晃动范围的圆形检测半径( 粗判 )")]
        int radius;

        [Desc("与该鱼绑定的鱼线集合( 不含鱼阵的鱼线 )")]
        List<FishLine> fishLines;


        [Desc("鱼移动帧总数( 同 frames.dataLen, 方便获取 )")]
        int moveFrameCount;

        [Desc("鱼移动帧集合")]
        List<FishSpriteFrame> moveFrames;

        // 并不存在 moveAnim. 鱼移动帧动画由逻辑代码驱动


        [Desc("鱼死亡帧总数( 同 frames.dataLen, 方便获取 )")]
        int dieFrameCount;

        [Desc("鱼死帧集合")]
        List<FishSpriteFrameBase> dieFrames;


        [Desc("显示时的放大系数. 平时为 1. 创建精灵时先设起. 后面不用反复改")]
        float scale;

        [Desc("图层优先级 ZOrder( 碰撞 & 点选优先级按 cfgId 由大到小来的, 和这个无关 )")]
        int priority;

        [Desc("影子显示时的放大系数. 平时与 scale 相等. 霸王蟹影子比身体小.")]
        float shadowScale;

        [Desc("影子的偏移坐标")]
        xx.Pos shadowOffset;

        [Desc("是否显示连击")]
        bool isShowHit;

        [Desc("是否是spine鱼")]
        bool isSpine;
        [Desc("spine的skel文件名")]
        string spine_skel_name;
        [Desc("spine的atlas文件名")]
        string spine_atlas_name;
        [Desc("spine动画循环状态")]
        bool spineLoop;
        [Desc("spine动画需要修正的度数，有些鱼头朝下，朝后 需统一为头朝右")]
        float spineAngle;
    }


    [Desc("游戏配置信息( 配置信息并不会随着网络同步而下发, 反序列化后需要手工还原 )"), AttachInclude]
    class Config
    {
        [Desc("所有鱼的配置信息")]
        List<FishConfig> allFishs;

        [Desc("所有的鱼线集合( 包含鱼阵所用鱼线 )")]
        List<FishLine> allFishLines;

        [Desc("分类放置的鱼线, 以方便随机取用. 一级下标为: 直线, 大曲, 中曲, 小曲, 特殊")]
        List<List<FishLine>> typedLines;

        [Desc("鱼线团, 一小团鱼专用")]
        List<List<FishLine>> groupLines;

        [Desc("非线形组队鱼,非线形的 组队鱼专用")]
        List<List<FishLine>> arrayLines;

        [Desc("所有鱼发射器配置")]
        List<Emitter> allEmitters;

        [Desc("所有阶段配置")]
        List<Stage> allStages;

        [Desc("阶段顺序配置表")]
        List<int> allStagesNumber;
        [Desc("所有背景配置")]
        List<Background_Config> allBackgrounds;

        // 下面是一些杂项配置

        [Desc("子弹配置")]
        List<int> bulletRadius;

        [Limit(64), Desc("MD5")]
        string configGenMd5;

        [Desc("基于设计尺寸为 1280x720, 屏中心为 0,0 点的 4 个玩家炮台的坐标( 0: 左下  1: 右下    2: 右上  3: 左上 )")]
        List<xx.Pos> cannonPoss;

        [Desc("烈焰风暴游戏时间")]
        int fireStromGameTime;

        [Desc("当前cfg所需资源名字集合")]
        List<string> resList;

        [Desc("当前cfg所需鱼id集合")]
        List<int> fishNameList;

    }


    [Desc("霸王蟹相关配置")]
    class FishConfig_KingCrab : FishConfig
    {
        [Desc("初生阶段: 每帧放大增量( 初时为 0. 直到放大到 1. )")]
        float bornSizeScaleIncrease;

        [Desc("逃离阶段: 进入阶段时 pos / 它 以得到每帧的坐标增量( 直到跑出屏幕看不到为止 )")]
        float escapePosScaleIncrease;
    }



    [Desc("鳄鱼相关配置")]
    class FishConfig_Crocodile : FishConfig
    {
        [Desc("鱼跳咬帧总数( 同 frames.dataLen, 方便获取 )")]
        int biteFrameCount;

        [Desc("鱼跳咬帧集合")]
        List<FishSpriteFrame> biteFrames;


        // 针对每个炮台的鱼线, 都在基类 fishLines 里, 下标除以 4 即得到对应的炮台下标


        [Desc("鳄鱼图片中心点离目标炮台的距离达到一定时( 误差小于移动时的 1 帧距离 ) 切换到起跳状态( 开始播放 biteFrames 里的帧图, 放完后切回正常移动状态 )")]
        float biteJumpDist;


        [Desc("咬到炮台时的 ZOrder 值")]
        float biteZOrder;

        [Desc("咬到炮台时段的帧下标范围起始")]
        int biteFrameIndexFrom;

        [Desc("咬到炮台时段的帧下标范围终止")]
        int biteFrameIndexTo;


        [Desc("逃离时的速度系数")]
        float escapeSpeedScale;
    }
    [Desc("赤焰龙龟相关配置")]
    class FishConfig_DragonTurtle : FishConfig
    {
        [Desc("翻滚帧总数( 同 frames.dataLen, 方便获取 )")]
        int rollFrameCount;

        [Desc("翻滚帧集合")]
        List<FishSpriteFrame> rollFrames;

        [Desc("变身帧总数( 同 frames.dataLen, 方便获取 )")]
        int changeFrameCount;

        [Desc("翻滚帧集合")]
        List<FishSpriteFrame> changeFrames;

        [Desc("翻滚时的速度")]
        float speed;
        [Desc("翻滚次数")]
        int rollCounts;
        [Desc("进场次数")]
        int enterCounts;

        [Desc("出生点（屏幕四个角）")]
        List<xx.Pos> bornPoints;
        [Desc("目标点的随机半径")]
        float randomRadius;

        [Desc("提前离场时间")]
        int advanceEndMakeFish;

    }
    [Desc("暗夜炬兽相关配置")]
    class FishConfig_NightTorches : FishConfig
    {
        [Desc("每帧的亮光点")]
        List<xx.Pos> lightPoints;
        [Desc("逃离时的速度系数")]
        float escapeSpeedScale;
    }
    [Desc("炸弹蟹相关配置")]
    class FishConfig_BombCrab : FishConfig
    {
        [Desc("炸弹蟹爆炸波及面积半径")]
        float boom_Radius;
        [Desc("爆炸延迟时间")]
        int boomTime;
        [Desc("炸弹蟹红光缩放比")]
        List<float> scales;
    }
    [Desc("连环炸弹蟹相关配置")]
    class FishConfig_ContinuityBombCrab : FishConfig
    {
        [Desc("炸弹蟹爆炸波及面积半径")]
        float boom_Radius;
        [Desc("爆炸延迟时间")]
        int boomTime;
        [Desc("炸弹蟹红光缩放比")]
        List<float> scales;
        [Desc("爆炸次数")]
        int frequency;
        [Desc("炸弹蟹死亡时缩放比")]
        List<float> deathScales;
    }

    class FishConfig_BlazeGale : FishConfig
    {
        [Desc("烈焰风暴炮台倒计时时间")]
        float cannon_Time;

        [Desc("火焰总帧数")]
        int blazeFrameNum;

        [Desc("火焰所有帧数集合")]
        List<FishSpriteFrame> blazeFrames;
    }
    [Desc("八爪章鱼相关配置")]
    class FishConfig_Octopus : FishConfig
    {
        [Desc("摆动CD时间")]
        int swingTime;
        [Desc("出现CD时间")]
        int bronTime;
        [Desc("碰撞半径")]
        int collideRadius;

    }
    [Desc("龙虾将军相关配置")]
    class FishConfig_GeneralLobster : FishConfig
    {
        [Desc("逃离时的速度系数")]
        float escapeSpeedScale;
        [Desc("蓝光特效名称")]
        string effertName;
    }
    [Desc("电光水母相关配置")]
    class FishConfig_ElectroJelly : FishConfig
    {
        [Desc("逃离时的速度系数")]
        float escapeSpeedScale;
    }

    [Desc("旋风鱼相关配置")]
    class FishConfig_CycloneFish : FishConfig
    {
        [Desc("旋风鱼的同类鱼对应的鱼容器下标")]
        int samePriority;
    }

    [Desc("虎头鲨相关配置")]
    class FishConfig_TigerHeadShark : FishConfig
    {
        [Desc("虎头鲨移动方向集合")]
        List<xx.Pos> moveIncs;
        [Desc("虎头鲨起始方向集合")]
        List<xx.Pos> startPoss;
    }
}
