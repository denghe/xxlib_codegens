#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Extral
{
    [Desc("请求所有彩金信息")]
    class ReqAllLottery
    {

    }

    [Desc("请求自身彩金信息")]
    class ReqOneGameLottery
    {
        [Desc("游戏id")]
        int game_id;
    }

    [Desc("玩家押注时会去影响彩金值")]
    class ChangeGameLottery
    {
        int coin;
        int game_id;
    }

    [Desc("请求按线算分处理,会出现押注彩金,玩家押注时会去影响彩金值")]
    class ReqSlotCounter
    {
        [Desc("玩家ID")]
        int guid;

        [Desc("游戏级别")]
        int levelId;

        [Desc("游戏服ID")]
        int gameId;

        [Desc("押注金额")]
        double inMoney;

        [Desc("期望获得的金额")]
        double hopeGet;

        [Desc("隐藏金币 用于参与计算 不计入总得")]
        double hiddenMoney;

        [Desc("0代表没有押注彩金")]
        int lotteryID;

        [Desc("本次是否免费 免费则不增加总押")]
        bool isFree;

        [Desc("是否直接给得分")]
        bool isPresent;

        [Desc("是否影响该游戏服关联的彩金")]
        bool needChangeLottery;
    }

    [Desc("一组算分请求")]
    class ReqSlotCounters
    {
        List<ReqSlotCounter> counters;
    }

    [Desc("返回所有彩金信息")]
    class AllLotteryRet
    {
        List<LotteryInfo> lotterys;
    }

    [Desc("单个彩金信息")]
    class LotteryInfo
    {
        [Desc("彩金ID")]
        int lotteryID;

        [Desc("大厅展示 0不展示  1,是基本展示,101-福树 102-冒火 103-闪电")]
        int hallShow;

        [Desc("彩金类型 0为常规 其他为有奖 1普通奖励 2小奖分值 3中奖分值 4大奖分值 5巨奖分值")]
        int lType;

        [Desc("彩金的属性:0倍数还是 1钱")]
        int betOrMoney;

        [Desc("当前的彩金值/彩金倍数")]
        double lReal;

        [Desc("彩金属性为倍数时,彩金最小倍数")]
        long lMinBet;

        [Desc("彩金属性为倍数时,彩金最大倍数")]
        long lMaxBet;

        [Desc("该彩金最小的押注,超过该值才能影响彩金的变动")]
        double coinMin;

        [Desc("该彩金的增量 ,用于客服端模拟自增")]
        double setp;

        [Desc("该彩金关联的游戏")]
        List<int> gameIDs;
    }

    [Desc("返回某个游戏的彩金信息")]
    class OneGameLotteryRet
    {
        List<LotteryInfo> infos;
    }

    [Desc("算分结果返回")]
    class SlotCountRet
    {
        [Desc("玩家ID")]
        int guid;
        [Desc("是否押中")]
        bool isWin;
        [Desc("押中金额")]
        double winMoney;
    }

    [Desc("一组算分结果返回")]
    class SlotCountRets
    {
        List<SlotCountRet> rets;
    }

}