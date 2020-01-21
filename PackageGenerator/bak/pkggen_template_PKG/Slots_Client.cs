#pragma warning disable 0169, 0414
using TemplateLibrary;
namespace Slots_Client
{
    [Desc("玩家长时间未操作离线倒计时")]
    class OfflineCheck
    {

    }

    [Desc("退出游戏成功")]
    class Leave_Success
    {
    }

    [Desc("玩家锁住状态离开成功")]
    class LockLeaveSuccess
    {

    }

    [Desc("进入游戏成功")]
    class Enter_Success
    {
        [Desc("用户Id ( 随机 8 位整数 )")]
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

        [Desc("押注金额和押注数的对应关系")]
        List<SlotsBase.BetRatio> betRatios;

        [Desc("各个级别的进入条件")]
        List<SlotsBase.EntryConditions> entryConditions;
    }

    [Desc("玩家收集进度")]
    class CollectValue
    {
        int nowValue;
    }

    [Desc("Sugar-普通结果")]
    class SugarSpinNormalRet : Sugar.SugarBase
    {
        [Desc("彩虹中奖线(3个以上)")]
        Sugar.Line rainbowFreeLine;
        [Desc("糖果中奖线(6个以上)")]
        Sugar.Line resipnLine;
        [Desc("普通的中奖线组(符合连线规则)")]
        List<Sugar.Line> lines;
    }

    [Desc("Sugar-彩虹触发的免费游戏结果")]
    class SugarRainbowFreeRet : Sugar.SugarBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("中奖的线")]
        List<Sugar.Line> lines;
    }

    [Desc("Sugar-触发的免费游戏结果")]
    class SugarResipnRet : Sugar.SugarBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
    }

    [Desc("Sugar-断线重连数据恢复")]
    class SugarEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        [Desc("恢复普通结果")]
        SugarSpinNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        SugarRainbowFreeRet resumedFree;
        [Desc("恢复糖果旋转结果")]
        SugarResipnRet resumedResipn;
    }

    [Desc("Festival-普通结果")]
    class FestivalSpinNormalRet : Festival.FestivalBase
    {
        [Desc("免费游戏")]
        Festival.Line LionFreeLine;
        [Desc("lightning resipn")]
        Festival.Line resipnLine;
        [Desc("普通的中奖线组(符合连线规则)")]
        List<Festival.Line> lines;
    }

    [Desc("Festival-免费次数游戏")]
    class FestivalLionFreeRet : Festival.FestivalBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("中奖的线")]
        List<Festival.Line> lines;
    }

    [Desc("Festival-触发的灯笼游戏")]
    class FestivalResipnRet : Festival.FestivalBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
    }

    [Desc("Festival-断线重连数据恢复")]
    class FestivalEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        [Desc("恢复普通结果")]
        FestivalSpinNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        FestivalLionFreeRet resumedFree;
        [Desc("恢复灯笼旋转结果")]
        FestivalResipnRet resumedResipn;
    }

    [Desc("财神道 普通结果")]
    class WealthGoldNormalRet : WealthGold.WealthGoldBase
    {
        [Desc("是否进入免费游戏")]
        int avatarId;

        [Desc("普通的中奖线组(符合连线规则)")]
        List<WealthGold.Line> lines;
    }

    [Desc("财神道 免费结果")]
    class WealthGoldFreeRet : WealthGold.WealthGoldBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int totalCount;
        [Desc("再次中免费")]
        int newFreeTime;
        [Desc("替代的倍率")]
        int wildBet;
        [Desc("增加的轮次")]
        int addTime;
        [Desc("中奖的线")]
        List<WealthGold.Line> lines;
    }

    [Desc("财神道 设置免费类型返回结果")]
    class WealthGoldFreeRetType
    {
        [Desc("是否成功")]
        int avatarId;
    }

    [Desc("财神道 游戏断线重连数据恢复")]
    class WealthGoldEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;
        int freetime;
        [Desc("恢复普通结果")]
        WealthGoldNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        WealthGoldFreeRetType resumedFreeType;
        [Desc("恢复免费游戏结果")]
        WealthGoldFreeRet resumedFreeRet;
    }

    [Desc("LuckyDiamond-普通结果")]
    class LuckyDiamondSpinNormalRet : LuckyDiamond.LuckyDiamondBase
    {
        [Desc("免费游戏")]
        LuckyDiamond.Line FreeLine;
        [Desc("lightning resipn")]
        List<LuckyDiamond.Line> lines;
    }

    [Desc("LuckyDiamond-免费次数游戏")]
    class LuckyDiamondFreeRet : LuckyDiamond.LuckyDiamondBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("中奖的线")]
        List<LuckyDiamond.Line> lines;
    }

    [Desc("LuckyDiamond-断线重连数据恢复")]
    class LuckyDiamondEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        [Desc("恢复普通结果")]
        LuckyDiamondSpinNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        LuckyDiamondFreeRet resumedFree;
    }

    [Desc("彩凤 普通结果")]
    class LotteryPhoenixNormalRet : LotteryPhoenix.LotteryPhoenixBase
    {
        [Desc("是否进入免费游戏")]
        int avatarId;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
        [Desc("普通的中奖线组(符合连线规则)")]
        List<LotteryPhoenix.Line> lines;
    }
    [Desc("彩凤 免费结果")]
    class LotteryPhoenixFreeRet : LotteryPhoenix.LotteryPhoenixBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int nowCount;
        [Desc("再次中免费")]
        int newFreeTime;
        [Desc("中奖的线")]
        List<LotteryPhoenix.Line> lines;
    }
    [Desc("彩凤 设置免费类型返回结果")]
    class LotteryPhoenixFreeRetType
    {
        [Desc("返回免费的次数")]
        int freeTime;
    }
    [Desc("彩凤 游戏断线重连数据恢复")]
    class LotteryPhoenixEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;
        int freetime;
        [Desc("游戏彩金已开的数组")]
        List<int> cells;
        [Desc("恢复普通结果")]
        LotteryPhoenixNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        LotteryPhoenixFreeRetType resumedFreeType;
        [Desc("恢复免费游戏结果")]
        LotteryPhoenixFreeRet resumedFreeRet;
    }
    class LotteryPhoenixLotteryRet
    {
        [Desc("彩金的列表")]
        List<LotteryPhoenix.Lottery> lines;
    }

    [Desc("熊猫宝藏 普通结果")]
    class PandaTreasuresNormalRet : PandaTreasures.PandaTreasuresBase
    {
        [Desc("是否进入免费游戏")]
        int avatarId;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
        [Desc("普通的中奖线组(符合连线规则)")]
        List<PandaTreasures.Line> lines;
    }
    [Desc("熊猫宝藏  免费结果")]
    class PandaTreasuresFreeRet : PandaTreasures.PandaTreasuresBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int nowCount;
        [Desc("增加的轮次")]
        int addTime;
        [Desc("免费的倍率（2/3）")]
        int wildBet;
        [Desc("中奖的线")]
        List<PandaTreasures.Line> lines;
    }
    [Desc("熊猫宝藏 设置免费类型返回结果")]
    class PandaTreasuresFreeRetType
    {
        [Desc("返回免费的次数")]
        int freeTime;
        [Desc("开格子依次开的东西")]
        List<int> openlist;
        [Desc("顺序 剩下没有开的东西")]
        List<int> ohterlist;
    }

    [Desc("熊猫宝藏  游戏断线重连数据恢复")]
    class PandaTreasuresEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;
        // int freetime;
        [Desc("恢复普通结果")]
        PandaTreasuresNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        PandaTreasuresFreeRetType resumedFreeType;
        [Desc("恢复免费游戏结果")]
        PandaTreasuresFreeRet resumedFreeRet;
    }

    class PandaTreasuresLotteryRet
    {
        [Desc("彩金的列表")]
        List<PandaTreasures.Lottery> lines;
    }

    [Desc("巨龙争霸214 普通结果")]
    class GrandDragonsNormalRet : GrandDragons.GrandDragonsBase
    {
        [Desc("是否进入免费游戏")]
        int avatarId;
        [Desc("普通的中奖线组(符合连线规则)")]
        List<GrandDragons.Line> lines;
    }

    [Desc("巨龙争霸214 免费结果")]
    class GrandDragonsFreeRet : GrandDragons.GrandDragonsBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int totalCount;
        [Desc("再次中免费")]
        int newFreeTime;
        [Desc("替代的倍率")]
        int wildBet;
        [Desc("中奖的线")]
        List<GrandDragons.Line> lines;
    }
    [Desc("巨龙争霸214 设置免费类型返回结果")]
    class GrandDragonsFreeRetType
    {
        [Desc("返回免费的次数")]
        int freeTime;
    }


    [Desc("巨龙争霸214 游戏断线重连数据恢复")]
    class GrandDragonsEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;
        int freetime;
        [Desc("恢复普通结果")]
        GrandDragonsNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        GrandDragonsFreeRetType resumedFreeType;
        [Desc("恢复免费游戏结果")]
        GrandDragonsFreeRet resumedFreeRet;
    }
    [Desc("金虎聚财213  普通结果")]
    class TigerWealthNormalRet : TigerWealth.TigerWealthBase
    {
        [Desc("是否进入免费游戏")]
        int intoFree;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
        [Desc("普通的中奖线组(符合连线规则)")]
        List<TigerWealth.Line> lines;
    }

    [Desc("金虎聚财213  免费结果")]
    class TigerWealthFreeRet : TigerWealth.TigerWealthBase
    {
        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int totalCount;
        [Desc("增加的轮次")]
        int addTime;
        [Desc("中奖的线")]
        List<TigerWealth.Line> lines;
    }

    [Desc("金虎聚财213 设置免费类型返回结果")]
    class TigerWealthFreeRetType
    {
        [Desc("返回免费的次数")]
        int freeTime;
    }

    [Desc("金虎聚财213游戏断线重连数据恢复")]
    class TigerWealthEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;
        int freetime;
        [Desc("游戏彩金已开的数组")]
        List<int> cells;
        [Desc("恢复普通结果")]
        TigerWealthNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        TigerWealthFreeRetType resumedFreeType;
        [Desc("恢复免费游戏结果")]
        TigerWealthFreeRet resumedFreeRet;
    }
    [Desc("幸运美金222 普通结果")]
    class LuckyDollarNormalRet : LuckyDollar.LuckyDollarBase
    {
        [Desc("是否进入特殊游戏")]
        int intoSpecial;

        [Desc("普通的中奖线组(符合连线规则)")]
        List<LuckyDollar.Line> lines;
    }
    [Desc("幸运美金222 特殊结果")]
    class LuckyDollarSpecialRet : LuckyDollar.LuckyDollarSpecial
    {
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
    }
    [Desc("幸运美金222 游戏断线重连数据恢复")]
    class LuckyDollarEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        [Desc("转盘是否点击")]
        int click;
        [Desc("美金点击次数")]
        int dollarNum;
        [Desc("游戏类型")]
        int type;
        [Desc("恢复普通结果")]
        LuckyDollarNormalRet resumedNormal;
        [Desc("恢复特殊结果")]
        LuckyDollarSpecialRet resumedSpecial;
    }
    [Desc("皇朝88 请求普通旋转图形结果")]
    class Empire88NormalRet : Empire88.NormalSpin
    { }
    [Desc("皇朝88 设置免费类型返回结果")]
    class Empire88FreeRetType : Empire88.FreeType
    { }
    [Desc("皇朝88 免费结果")]
    class Empire88FreeRet : Empire88.FreeSpin
    { }
    [Desc("皇朝88 特殊结果")]
    class Empire88SpecialRet : Empire88.SpecialSpin
    { }
    [Desc("皇朝88 游戏断线重连数据恢复")]
    class Empire88EnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        [Desc("游戏类型")]
        int type;
        [Desc("剩余免费轮数")]
        int freetime;
        [Desc("恢复普通结果")]
        Empire88NormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        Empire88FreeRetType resumedFreeType;
        [Desc("恢复免费结果")]
        Empire88FreeRet resumedFreeRet;
        [Desc("恢复特殊结果")]
        Empire88SpecialRet resumedSpecial;
    }

    [Desc("狂野蛮牛 请求普通旋转图形结果")]
    class AfricanBuffaloNormalRet : AfricanBuffalo.NormalSpin
    { }

    [Desc("狂野蛮牛 免费结果")]
    class AfricanBuffaloFreeRet : AfricanBuffalo.FreeSpin
    { }
    [Desc("狂野蛮牛 游戏断线重连数据恢复")]
    class AfricanBuffaloEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;

        [Desc("恢复普通结果")]
        AfricanBuffaloNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        AfricanBuffaloFreeRet resumedFreeRet;
    }

    [Desc("齐天大圣 请求普通旋转图形结果")]
    class ImmortalMonkeyNormalRet : ImmortalMonkey.NormalSpin
    { }

    [Desc("齐天大圣 免费结果")]
    class ImmortalMonkeyFreeRet : ImmortalMonkey.FreeSpin
    { }
    [Desc("齐天大圣 游戏断线重连数据恢复")]
    class ImmortalMonkeyEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        int type;
        [Desc("恢复普通结果")]
        ImmortalMonkeyNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        ImmortalMonkeyFreeRet resumedFreeRet;
    }
    [Desc("龙海公主 请求普通旋转图形结果")]
    class OceanPrincessNormalRet : OceanPrincess.NormalSpin
    { }

    [Desc("龙海公主 免费结果")]
    class OceanPrincessFreeRet : OceanPrincess.FreeSpin
    { }
    [Desc("龙海公主 游戏断线重连数据恢复")]
    class OceanPrincessEnterResumed
    {

        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;

        [Desc("游戏彩金已开的数组")]
        List<int> cells;

        [Desc("恢复普通结果")]
        OceanPrincessNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        OceanPrincessFreeRet resumedFreeRet;
    }
    [Desc("精灵宝藏 免费结果")]
    class LeprechaunStackGoldNormalRet : LeprechaunStackGold.NormalSpin
    {

    }
    [Desc("精灵宝藏 免费结果")]
    class LeprechaunStackGoldFreeRet : LeprechaunStackGold.FreeSpin
    {

    }
    [Desc("精灵宝藏 免费结果")]
    class LeprechaunStackGoldFreeTypeRet : LeprechaunStackGold.FreeType
    {

    }
    class LeprechaunStackGoldEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        int type;
        [Desc("恢复普通结果")]
        LeprechaunStackGoldNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        LeprechaunStackGoldFreeTypeRet resumedFreeTypeRet;
        [Desc("恢复免费游戏结果")]
        LeprechaunStackGoldFreeRet resumedFreeRet;
    }
    class VegasNightNormalRet : VegasNight.NormalSpin
    {

    }
    class VegasNightFreeRet : VegasNight.FreeSpin
    {

    }
    class VegasNightSpecialRet : VegasNight.FreeSpin
    {
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
    }
    class VegasNightEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        int type;

        [Desc("恢复普通结果")]
        VegasNightNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        VegasNightFreeRet resumedFreeRet;
        [Desc("恢复免费游戏结果")]
        VegasNightSpecialRet resumedSpecialRet;
    }
    //竖屏游戏
    [Desc("法老宝藏 请求普通旋转图形结果")]
    class EgyptianFantasyNormalRet : EgyptianFantasy.NormalSpin
    { }

    [Desc("法老宝藏 免费结果")]
    class EgyptianFantasyFreeRet : EgyptianFantasy.FreeSpin
    { }
    [Desc("法老宝藏 游戏断线重连数据恢复")]
    class EgyptianFantasyEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;

        int type;

        [Desc("游戏彩金已开的数组")]
        List<int> cells;

        [Desc("恢复普通结果")]
        EgyptianFantasyNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        EgyptianFantasyFreeRet resumedFreeRet;
    }


    //--------
    [Desc("发财树 请求普通旋转图形结果")]
    class FortuneTreeNormalRet : FortuneTree.NormalSpin
    { }
    [Desc("发财树 请求普通旋转图形结果")]
    class FortuneTreeFreeRetType : FortuneTree.FreeType
    { }
    [Desc("发财树 免费结果")]
    class FortuneTreeFreeRet : FortuneTree.FreeSpin
    { }
    [Desc("发财树 游戏断线重连数据恢复")]
    class FortuneTreeEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        int type;


        [Desc("游戏彩金已开的数组")]
        List<int> cells;

        [Desc("恢复普通结果")]
        FortuneTreeNormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        FortuneTreeFreeRetType resumedFreeTypeRet;
        [Desc("恢复免费游戏结果")]
        FortuneTreeFreeRet resumedFreeRet;
    }
    //--------
    [Desc("黄金88 请求普通旋转图形结果")]
    class Golden88NormalRet : Golden88.NormalSpin
    { }

    [Desc("黄金88 请求普通旋转图形结果")]
    class Golden88FreeRetType : Golden88.FreeType
    { }
    [Desc("黄金88 免费结果")]
    class Golden88FreeRet : Golden88.FreeSpin
    { }
    [Desc("黄金88 游戏断线重连数据恢复")]
    class Golden88EnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        int type;


        [Desc("游戏彩金已开的数组")]
        List<int> cells;

        [Desc("恢复普通结果")]
        Golden88NormalRet resumedNormal;
        [Desc("恢复免费游戏类型")]
        Golden88FreeRetType resumedFreeTypeRet;
        [Desc("恢复免费游戏结果")]
        Golden88FreeRet resumedFreeRet;
    }
    //--------
    [Desc("招财猫 普通结果")]
    class FortuneCatNormalRet : FortuneCat.NormalSpin
    {

    }
    [Desc("招财猫 免费结果")]
    class FortuneCatFreeRet : FortuneCat.FreeSpin
    {

    }
    [Desc("招财猫 灯笼结果")]
    class FortuneCatSpecialRet : FortuneCat.FreeSpin
    {
        [Desc("	正常状态 0,大奖锁住 1,巨奖锁住 2")]
        int lockStates;
    }
    [Desc("招财猫 游戏断线重连数据恢复")]
    class FortuneCatEnterResumed
    {
        [Desc("进入游戏的常规数据恢复")]
        Enter_Success enterBase;
        [Desc("当前赢钱面板上显示的金额")]
        double currentWinCoin;
        [Desc("当前押注金额")]
        double currentBetMoney;
        int type;

        [Desc("恢复普通结果")]
        FortuneCatNormalRet resumedNormal;
        [Desc("恢复免费游戏结果")]
        FortuneCatFreeRet resumedFreeRet;
        [Desc("恢复特殊灯笼结果")]
        FortuneCatSpecialRet resumedSpecialRet;
    }
}
