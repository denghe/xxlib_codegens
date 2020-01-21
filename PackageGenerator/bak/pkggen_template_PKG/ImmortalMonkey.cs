
#pragma warning disable 0169, 0414
  using TemplateLibrary;

namespace ImmortalMonkey
{
    enum ImmortalMonkeySymbol
    {
        ServerMonkeyWild        = 1,    //替代 4个以上出免费 
        ServerMonkeyNine        = 2,    //       
        ServerMonkeyTen         = 3,    //    
        ServerMonkeyJ           = 4,    //   
        ServerMonkeyQ           = 5,       
        ServerMonkeyK           = 6,       
        ServerMonkeyA           = 7,       
        ServerMonkeyGeBuLin     = 8,    //哥布林
        ServerMonkeyHongHaiEr   = 9,    //红孩儿
        ServerMonkeyTieShan     = 10,   //铁扇
        ServerMonkeyNiuMoWang   = 11,   //牛魔王
        ServerMonkeyWuKong      = 12,   //悟空
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        ImmortalMonkeySymbol icon;
    }
    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        ImmortalMonkeySymbol icon;

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
       
        [Desc("(符合连线规则)")]
        List<Line> lines;
    }

    class FreeSpin
    {
        //次数无上限  结束标识 悟空打死牛魔王 或者悟空掉下去了
        [Desc("(一次旋转押注 )")]
        double bet;
        [Desc("(一次旋转赢钱)")]
        double winCoin;
        [Desc("生成的15个图形集合")]
        List<Cell> grids;
        [Desc("结束时 的赢钱")]
        double bonusWinCoin;
        [Desc("当前次数")]
        int totalCount;
        [Desc("中奖的线")]
        List<Line> lines;
        //开始前 小怪兽的数量 4个数组 分别标识 
        //哥布林 红孩儿 铁扇 牛魔王 的个数
        [Desc("小怪兽的数量")]
        List<int> aryMonster;
        [Desc("结束标识")]
        int isEnd;
        [Desc("打妖怪标识")]
        int haveWukong;
    }
}
