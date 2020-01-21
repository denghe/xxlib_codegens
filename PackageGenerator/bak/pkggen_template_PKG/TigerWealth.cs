
#pragma warning disable 0169, 0414
using TemplateLibrary;


//金虎聚财213 
namespace TigerWealth
{
    enum TigerWealthSymbol
    {
        SymbolWild = 1,    //替代 
        SymbolTiger = 2,   //老虎 
        SymbolCaibao = 3,  //财宝 这个来出免费
        SymbolYellow = 4,  //黄宝石
        SymbolRed = 5,     //红宝石
        SymbolGreen = 6,   //绿宝石
        SymbolBule = 7,    //蓝宝石
        SymbolA = 8,       //A
        SymbolK = 9,       //K
        SymbolQ = 10,      //Q
        SymbolJ = 11,      //J
        SymbolTen = 12,    //10
        SymbolNine = 13,   //9
    }
    class map
    {
        int index;
        int num;
    }
    [Desc("生成图形数组和相关结果的基类")]
    class TigerWealthBase
    {
        [Desc("(一次旋转押注 免费游戏为0)")]
        double bet;

        [Desc("(一次旋转赢钱)")]
        double winCoin;
        [Desc("百搭图形的倍率")]
        int wildBet;

        [Desc("生成的15个图形集合")]
        List<Cell> grids;

        [Desc("哪些列要变凤凰 (3,5)")]
        List<int> changeWilds;
        [Desc("哪些列要变背景")]
        List<int> changeBacks;
        [Desc("背景图片信息（01234567）")]
        List<map> backinfos;
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        TigerWealthSymbol icon;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        TigerWealthSymbol icon;

        [Desc("一条线上的中奖集合")]
        List<Cell> lineCells;
    }
   
}
