#pragma warning disable 0169, 0414
  using TemplateLibrary;

namespace OceanPrincess
{
    enum OceanPrincessSymbol
    {
        ServerPrincessWild      = 1,        //替代
        ServerPrincessCaibao    = 2,        //财宝 这个来出免费 
        ServerPrincessLong      = 3,        //龙
        ServerPrincessGui       = 4,        //龟
        ServerPrincessHuaPing   = 5,        //花瓶
        ServerPrincessFengChai  = 6,        //凤钗
        ServerPrincessA         = 7,
        ServerPrincessK         = 8,
        ServerPrincessQ         = 9,
        ServerPrincessJ         = 10,
        ServerPrincessTen       = 11,
        ServerPrincessNine      = 12,
    }
    class map
    {
        int index;
        int num;
    }
    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        OceanPrincessSymbol icon;
    }
    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        OceanPrincessSymbol icon;

        [Desc("一条线上的中奖集合")]
        List<Cell> lineCells;
    }
    class NormalSpin
    {
        [Desc("(一次旋转押注 )")]
        double bet;
        [Desc("(一次旋转赢钱)")]
        double winCoin;
        [Desc("生成的15个图形集合")]
        List<Cell> grids;

        [Desc("是否进入免费游戏")]
        int intoFree;
        [Desc("进入时免费次数")]
        int FreeTime;
        [Desc("(符合连线规则)")]
        List<Line> lines;

        [Desc("哪些列要变凤凰 (3,5)")]
        List<int> changeWilds;
        [Desc("哪些列要变背景")]
        List<int> changeBacks;
        [Desc("背景图片信息（01234567）")]
        List<map> backinfos;

        [Desc("大奖锁定")]
        int lockStates;
    }


    class FreeSpin
    {
        [Desc("(一次旋转押注 )")]
        double bet;
        [Desc("(一次旋转赢钱)")]
        double winCoin;
        [Desc("生成的15个图形集合")]
        List<Cell> grids;

        [Desc("转盘次数为0时，返回的Bonus Win")]
        double bonusWinCoin;
        [Desc("总次数")]
        int allCount;
        [Desc("当前次数")]
        int totalCount;
        [Desc("再次中免费次数")]
        int addTime;
        [Desc("中奖的线")]
        List<Line> lines;

        [Desc("哪些列要变凤凰 (3,5)")]
        List<int> changeWilds;
        [Desc("哪些列要变背景")]
        List<int> changeBacks;
        [Desc("背景图片信息（01234567）")]
        List<map> backinfos;
        [Desc("大奖锁定")]
        int lockStates;
    }
}