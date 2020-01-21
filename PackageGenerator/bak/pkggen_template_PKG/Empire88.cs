
#pragma warning disable 0169, 0414
  using TemplateLibrary;

namespace Empire88
{
    enum Empire88Symbol
    {
        ServerEmpire88Wild = 1,        //替代
        ServerEmpire88GongDian = 2,        //宫殿 
        ServerEmpire88YuXi = 3,        //玉玺
        ServerEmpire88ShiZi = 4,        //狮子
        ServerEmpire88DaDing = 5,        //大鼎
        ServerEmpire88YuanBao = 6,        //元宝
        ServerEmpire88JiuBei = 7,        //酒杯
        ServerEmpire88A = 8,        //A
        ServerEmpire88K = 9,        //K
        ServerEmpire88Q = 10,       //Q
        ServerEmpire88J = 11,       //J
        ServerEmpire88Ten = 12,       //10
        ServerEmpire88Nine = 13,	    //9
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        Empire88Symbol icon;
    }
    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        Empire88Symbol icon;

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

        [Desc("替代的倍率")]
        int wildBet;
        [Desc("是否进入免费游戏")]
        int intoFree;
        [Desc("(符合连线规则)")]
        List<Line> lines;
        [Desc("是否进入彩金")]
        int intoLottery;
    }
    class FreeType
    {
        [Desc("倍率类型")]
        int type;
        [Desc("免费次数")]
        int time;
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
        [Desc("替代的倍率")]
        int wildBet;
        [Desc("中奖的线")]
        List<Line> lines;
    }
    class OneLine
    {
        // 1 空白 2 背景8 3 up  4 中间空白 5中间再来一次
        List<int> lines;
        double add8;
        double end8;
    }
    class SpecialSpin
    {
        [Desc("(特殊游戏的钱)")]
        double winMony;
        int winCout;
        List<OneLine> allSpecial;

        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int totalCount;
    }
}
