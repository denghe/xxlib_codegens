#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace VegasNight
{
    enum VegasNightSymbol
    {
        VegasNightWild = 1,        //替代
        VegasNightBaoXiang= 2,        //宝箱 
        VegasNightCaiJing = 3,        //彩金
        VegasNightZhuanPan = 4,        //转盘
        VegasNightMeiNv = 5,        //美女
        VegasNightHaoChe = 6,        //豪车
        VegasNightPuKe = 7,        //扑克
        VegasNightK = 8,
        VegasNightQ = 9,
        VegasNightJ = 10,
        VegasNightTen = 11,
        VegasNightNine = 12,
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        VegasNightSymbol icon;

        [Desc("点对应图形Id的类型 0为常规图形 其他为有奖图形 1普通奖励 2小奖分值 3中奖分值 4大奖分值 5巨奖分值")]
        int FestivalSymbolType;

        [Desc("图形的彩金值")]
        double FestivalSymbolValue;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        VegasNightSymbol icon;

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
