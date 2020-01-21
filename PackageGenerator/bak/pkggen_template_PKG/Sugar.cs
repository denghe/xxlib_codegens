#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Sugar
{
    enum SugarSymbol
    {
        SugarWild = 1,   //替代
        SugarTen = 2,    //10
        SugarJ = 3,    //J
        SugarQ = 4,    //Q
        SugarK = 5,    //K
        SugarA = 6,    //A
        SugarGreen = 7,    //绿色
        SugarBule = 8,    //蓝色
        SugarPink = 9,    //粉色
        SugarRed = 10,   //红色
        SugarRainbow = 11,   //彩虹       
        SugarCandy = 12,   //糖果
    }
    [Desc("生成图形数组和相关结果的基类")]
    class SugarBase
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
        SugarSymbol icon;

        [Desc("点对应图形Id的类型 0为常规图形 其他为有奖图形 1普通奖励 2小奖分值 3中奖分值 4大奖分值 5巨奖分值")]
        int sugarSymbolType;

        [Desc("图形的彩金值")]
        double sugarSymbolValue;

        [Desc("点对应遮挡图形Id")]
        SugarSymbol childIcon;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        SugarSymbol icon;

        [Desc("一条线上的中奖集合")]
        List<Cell> lineCells;
    }
}


