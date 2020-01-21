#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace LuckyDollar
{
    enum LuckyDollarSymbol
    {
        ServerBoon = 1,    //替代
        Server1 = 2,   //1
        Server5 = 3,   //5
        Server10 = 4,   //10
        Server20 = 5,    //20
        Server50 = 6,    //50
        Server100 = 7,    //100
        Server200 = 8,   //200
        Server500 = 9,  //500
    }
    [Desc("生成图形数组和相关结果的基类")]
    class LuckyDollarBase
    {
        [Desc("(一次旋转押注 免费游戏为0)")]
        double bet;

        [Desc("(一次旋转赢钱)")]
        double winCoin;

        [Desc("中奖转盘剩余次数")]
        int validSpinCount;

        [Desc("中奖转盘总次数")]
        int totalSpinCount;

        [Desc("生成的15个图形集合")]
        List<Cell> grids;
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        LuckyDollarSymbol icon;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        LuckyDollarSymbol icon;

        [Desc("一条线上的中奖集合")]
        List<Cell> lineCells;
    }
    class RangeA
    {
        int index;
        int id;
        double value;
    }
    class RangeB
    {
        int index;
        double value;
    }
    class LuckyDollarSpecial
    {
        List<RangeA> ListA;
        int selectA;
        List<RangeB> ListB;
        int selectB;
        List<RangeB> ListC;
        double allWin;
        int intoSpecial;
    }
}


