#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace FortuneCat
{
    enum FortuneCatSymbol
    {
        FortuneCatWild =1,
        FortuneCatCaibao = 2,
        FortuneCatBaGua = 3,
        FortuneCatJinYu = 4,
        FortuneCatZhuZi = 5,
        FortuneCatPiPa = 6,
        FortuneCatZhiSan = 7,
        FortuneCatA=8,
        FortuneCatK=9,
        FortuneCatQ=10,
        FortuneCatJ=11,
        FortuneCat10=12,
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        FortuneCatSymbol icon;

        [Desc("点对应图形Id的类型 0为常规图形 其他为有奖图形 1普通奖励 2小奖分值 3中奖分值 4大奖分值 5巨奖分值")]
        int SymbolType;

        [Desc("图形的彩金值")]
        double SymbolValue;
        [Desc("是否遮挡")]
        int ishide;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        FortuneCatSymbol icon;

        [Desc("一条线上的中奖集合")]
        List<Cell> lineCells;
    }

    class NormalSpin
    {
        [Desc("(一次旋转押注 )")]
        double bet;
        [Desc("(一次旋转赢钱)")]
        double winCoin;
        [Desc("生成的20个图形集合")]
        List<Cell> grids;

        [Desc("是否进入免费游戏")]
        int intoFree;
        [Desc("是否进入特别游戏")]
        int intoSpecial;

        [Desc("(符合连线规则)")]
        List<Line> lines;
    }

    class FreeSpin
    {
        [Desc("(一次旋转押注 )")]
        double bet;
        [Desc("(一次旋转赢钱)")]
        double winCoin;
        [Desc("生成的20个图形集合")]
        List<Cell> grids;

        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int totalCount;
        [Desc("中奖的线")]
        List<Line> lines;
        [Desc("免费游戏增加免费次数")]
        int addTime;
    }
}

