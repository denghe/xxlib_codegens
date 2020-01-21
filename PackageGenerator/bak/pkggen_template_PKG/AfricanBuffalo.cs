
#pragma warning disable 0169, 0414
  using TemplateLibrary;

namespace  AfricanBuffalo
{
    enum AfricanBuffaloSymbol
    {
        ServerBuffaloWild       = 1,        //替代
        ServerBuffaloManNiu     = 2,        //蛮牛 
        ServerBuffaloCaibao     = 3,        //硬币
        ServerBuffaloShizi      = 4,        //狮子
        ServerBuffaloDaXiang    = 5,        //大象
        ServerBuffaloBanMa      = 6,        //斑马
        ServerBuffaloLu         = 7,        //鹿
        ServerBuffaloA          = 8,
        ServerBuffaloK          = 9,
        ServerBuffaloQ          = 10,
        ServerBuffaloJ          = 11,
        ServerBuffaloTen        = 12,
        ServerBuffaloNine       = 13,
    }
    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        AfricanBuffaloSymbol icon;
        [Desc("替代的倍率")]
        int wildbet;
    }
    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        AfricanBuffaloSymbol icon;

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
        [Desc("进入时免费次数")]
        int FreeTime;
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
        [Desc("再次中免费")]
        int newFreeTime;
        [Desc("中奖的线")]
        List<Line> lines;
    }

}
