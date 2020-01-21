#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace CatchFish
{
    [Desc("阶段( 游戏的不同进程 )")]
    class Stage
    {
        [Desc("阶段配置编号( 同时也为 cfg.allStages 数组下标 )")]
        int cfgId;

        [Desc("当前阶段结束时间点")]
        int endFrameNumber;

        [Desc("下一阶段编号")]
        int nextStageCfgId;

        [Desc("鱼发射器集合")]
        List<Emitter> emitters;


        [Desc("游戏阶段背景")]
        int  backgroundCfgId;

    }

    [Desc("鱼发射器( 只针对一种鱼, 多发射器并行运作, 兼容单只鱼生成 )")]
    class Emitter
    {
        [Desc("鱼发射器配置编号( 同时也为 cfg.allEmitters 数组下标 )")]
        int cfgId;

        [Desc("鱼配置编号( 同时也为 cfg.allFishs 数组下标 )")]
        int fishCfgId;

        [Desc("生效的帧编号( 有可能提前生成, 但不启用. 用 fishScene->frameNumber 与之相减即得到当前 frameNumber )")]
        int beginFrameNumber;

        [Desc("结束时的帧编号( 这个值如果超越 fishScene.frameNumber, 就杀掉这个 Emitter )")]
        int endFrameNumber;

        [Desc("单鱼或鱼群或鱼组生成 cd( 如果 >= fishScene.frameNumber 就表示时间到 )")]
        int interval;


        [Desc("boss以及特殊鱼的当前序列号")]
        int curFishSeriNumber;
    }


    [Desc("鱼发射器: 一小团鱼, 随机线团鱼线")]
    class Emitter_Group : Emitter
    {
        [Desc("小团中的个体总数( 不得超出线团size )")]
        int groupSize;

        [Desc("当前鱼群ID")]
        int fishGroupId;

        [Desc("小帧间隔: 一群中的个体的生成 cd( 如果 >= fishScene.frameNumber 就表示时间到 )")]
        int smallInterval;

    }

    [Desc("鱼发射器: 线形组队鱼, 随机大曲线鱼线")]
    class Emitter_LineArray : Emitter
    {
        [Desc("小帧间隔: 一队中的个体的生成 cd( 如果 >= fishScene.frameNumber 就表示时间到 )")]
        int smallInterval;

        [Desc("每一队的个体总数( 每生成一条就 -1, 到 0 时读配置填充, 并重置 大帧间隔. 小帧间隔不用重置 )")]
        int arraySize;

        [Desc("鱼的每帧移动距离")]
        float lineFrameDistance;

        [Desc("上一条鱼的鱼线ID")]
        int lastFishLineCfgId;

        [Desc("可用鱼线数组")]
        List<FishLine> fishLines;

       
    }
    [Desc("鱼发射器: 组队鱼,顺序分配鱼线")]
    class Emitter_Array : Emitter
    {
        [Desc("下一条鱼的鱼线ID")]
        int nextLineCfgId;

        [Desc("当前鱼的鱼线种类")]
        int fishLinesId;

    }

    [Desc("鱼发射器: 特殊线形组队鱼, 指定鱼线")]
    class Emitter_SpecialLineArray : Emitter
    {
        [Desc("指定鱼线配置编号")]
        int fishLineCfgId;

        [Desc("小帧间隔: 一队中的个体的生成 cd( 如果 >= fishScene.frameNumber 就表示时间到 )")]
        int smallInterval;

        [Desc("每一队的个体总数( 每生成一条就 -1, 到 0 时读配置填充, 并重置 大帧间隔. 小帧间隔不用重置 )")]
        int arraySize;

        [Desc("鱼线ID")]
        int fishLineId;

        [Desc("鱼的每帧移动距离")]
        float lineFrameDistance;

    }
    [Desc("鱼发射器: 监控场景内鱼 少就加入预生成容器")]
    class Emitter_FishMonitor : Emitter
    {
        [Desc("小鱼集合id")]
        List<int> smallFishId;
        [Desc("大鱼集合id")]
        List<int> largeFishId;
        [Desc("游戏内最少存活小鱼数是多少")]
        int smallFishNumber;
        [Desc("游戏内最少存活大鱼数是多少")]
        int largeFishNumber ;
    }

    [Desc("鱼发射器: 特殊鱼生成器")]
    class Emitter_SpecialFish : Emitter
    {
        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化特殊鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化特殊鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("特殊鱼id")]
        List<int> specialFishId;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }


    [Desc("鱼发射器: 霸王蟹")]
    class Emitter_KingCrab : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化霸王蟹的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化霸王蟹的 serialNumber")]
        int fishSerialNumber;
        
        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;


    }


    [Desc("鱼发射器: 鳄鱼")]
    class Emitter_Crocodile : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }


    [Desc("鱼发射器: 暗夜炬兽")]
    class Emitter_NightTorches : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }
    [Desc("鱼发射器:霸王鲸")]
    class Emitter_Tyrannosaurus : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }
    [Desc("鱼发射器:大王乌贼")]
    class Emitter_Sepia : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }
    [Desc("鱼发射器:赤焰龙龟")]
    class Emitter_DragonTurtle : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }
    [Desc("鱼发射器:八爪章鱼")]
    class Emitter_Octopus : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }
    [Desc("鱼发射器:龙虾将军")]
    class Emitter_GeneralLobster : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }
    [Desc("鱼发射器:电光水母")]
    class Emitter_ElectroJelly : Emitter
    {
        // 特殊：客户端只生成一次鱼, 到期时间等同于生效时间.
        // 服务器端逻辑则不同, 在生成一次鱼之后, 会续命并转为监视状态, 补鱼并下发

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 rnd seed")]
        int seed;

        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化鳄鱼的 serialNumber")]
        int fishSerialNumber;

        [Desc("服务器弱引用到鱼, 以监视该鱼的死活( 死了准备就补. 补生成会有一个延迟, 用 interval 来存 )")]
        [NotSerialize]
        Weak<Fish> fish;
    }
    [Desc("鱼发射器:旋风鱼")]
    class Emitter_CycloneFish : Emitter
    {
        [Desc("当用作服务器下发时, 服务器将该值传给客户端以初始化旋风鱼的 rnd seed")]
        int seed;

        [Desc("所有旋风鱼的id")]
        List<int> cycloneFishId;

        [Desc("所有旋风鱼的对应的普通鱼id")]
        List<int> fishId;

        [Desc("小团中的个体总数( 不得超出线团size )")]
        int groupSize;

        [Desc("小帧间隔: 一群中的个体的生成 cd( 如果 >= fishScene.frameNumber 就表示时间到 )")]
        int smallInterval;
    }
    [Desc("鱼发射器:旋风鱼同类鱼专用")]
    class Emitter_CycloneFish_Group : Emitter
    {
        [Desc("小团中的个体总数( 不得超出线团size )")]
        int groupSize;

        [Desc("当前鱼群可用鱼线")]
        List<int> fishLinesId;

        [Desc("当前鱼群ID")]
        int fishGroupId;

        [Desc("小帧间隔: 一群中的个体的生成 cd( 如果 >= fishScene.frameNumber 就表示时间到 )")]
        int smallInterval;
    }
    [Desc("鱼发射器:大盘鱼")]
    class Emitter_Fish_Grail : Emitter
    {
        [Desc("副鱼数量")]
        int fishNumber;
        [Desc("副鱼id")]
        int mateCfgId;
        [Desc("大盘鱼id")]
        int grailFishId;
        [Desc("偏移坐标List")]
        List<xx.Pos> offsetPosList;
        [Desc("鱼线")]
        List<FishLine> fishLines;
        [Desc("鱼倍率")]
        int fishBet;
    }
    [Desc("特效播放: 所有鱼逃离现场")]
    class Emitter_Effect_Escape : Emitter
    {
        // interval 用于表达加速倍数
        int frameRatio;

        float SpeedScale;
    }

    [Desc("特效播放: BOSS来袭的 警报声 + warning特效")]
    class Emitter_Effect_Warning : Emitter
    {
        int time;


    }

    [Desc("特效播放: 霸王蟹BOSS来袭的动画特效")]
    class Emitter_Effect_KingCrab_Coming : Emitter
    {
        // todo: warning 的资源内容
        // interval 用于表达持续时长, 当这个时间到达时, 清掉横幅, 关闭声音

    }
    [Desc("特效播放: 史前巨鳄BOSS来袭的动画特效")]
    class Emitter_Effect_Crocodile_Coming : Emitter
    {
        // todo: warning 的资源内容
        // interval 用于表达持续时长, 当这个时间到达时, 清掉横幅, 关闭声音

    }

    [Desc("特效播放: 暗夜炬兽BOSS来袭的动画特效")]
    class Emitter_Effect_NightTorches_Coming : Emitter
    {
        // todo: warning 的资源内容
        // interval 用于表达持续时长, 当这个时间到达时, 清掉横幅, 关闭声音

    }
    [Desc("特效播放: 八爪章鱼BOSS来袭的动画特效")]
    class Emitter_Effect_Octopus_Coming : Emitter
    {
        // todo: warning 的资源内容
        // interval 用于表达持续时长, 当这个时间到达时, 清掉横幅, 关闭声音

    }


    [Desc("特效播放: 全屏顺逆小角度来回旋转模拟地震")]
    class Emitter_Effect_Earthquake1 : Emitter
    {
        // 算法上需要能通过已经历的帧数来推算出当前角度
        // interval 用于表达持续时长, 当这个时间到达时, 恢复角度
        int  rotationIndex;

        List<float> rotations;


    }

    [Desc("特效播放: 覆盖全屏(含UI)的血光闪烁")]
    class Emitter_Effect_BloodFlash : Emitter
    {
        int time;
    }

    [Desc("特效播放: 覆盖全屏的气泡")]
    class Emitter_Effect_Bubbles : Emitter
    {
        // interval 在此处没啥用
    }
    [Desc("特效播放: 小BOSS来临")]
    class Emitter_Effect_CommonBoss : Emitter
    {
        int coin;
    }
    [Desc("特效播放: 粉红光效"), AttachInclude]
    class Emitter_Effect_PinkLight : Emitter
    {
        int opacityIndex;

        List<float> opacitys;
    }
    [Desc("特效播放: 渐变切换背景图为深渊( 中间部分浮动效果 )")]
    class Emitter_Effect_Background_Normal_To_Abyss : Emitter
    {
        // todo: 所有坐标, 进度, 透明度啥的全都在这建成员变量
    }

    [Desc("特效播放: 渐变切换深渊背景图为普通")]
    class Emitter_Effect_Background_Abyss_To_Normal : Emitter
    {
        // todo: 所有坐标, 进度, 透明度啥的全都在这建成员变量
    }


    // todo: 部分 interval 改成 范围, 以增加变数
}
