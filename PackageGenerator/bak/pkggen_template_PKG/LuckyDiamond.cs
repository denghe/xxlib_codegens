#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace LuckyDiamond
{
    enum LuckyDiamondSymbol
    {
        LuckyDiamondWild = 1,    //替代
        LuckyDiamondCherry = 2,    //樱桃
        LuckyDiamondOrange = 3,    //橘子
        LuckyDiamondGrape = 4,    //葡萄
        LuckyDiamondWatermelon = 5,    //西瓜
        LuckyDiamondBell = 6,    //铃铛
        LuckyDiamondBar1 = 7,    //Bar1
        LuckyDiamondBar2 = 8,    //Bar2
        LuckyDiamondBar3 = 9,    //Bar3
        LuckyDiamond7 = 10,   //7
        LuckyDiamondStar = 11,   //星星(触发免费游戏)
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;
        [Desc("点对应图形Id")]
        LuckyDiamond.LuckyDiamondSymbol icon;
    }

    [Desc("生成图形数组和相关结果的基类")]
    class LuckyDiamondBase
    {
        [Desc("总押注(一次旋转 免费游戏为0)")]
        double bet;

        [Desc("总赢钱(一次旋转)")]
        double winCoin;

        [Desc("中奖转盘剩余次数")]
        int validSpinCount;

        [Desc("中奖转盘总次数")]
        int totalSpinCount;

        [Desc("生成的15个图形集合")]
        List<Cell> grids;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("中奖的图标")]
        LuckyDiamond.LuckyDiamondSymbol icon;

        [Desc("生成的15个图形集合")]
        List<Cell> lineCells;
    }
}
