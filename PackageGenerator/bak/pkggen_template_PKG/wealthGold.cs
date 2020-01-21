#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace WealthGold
{
    enum wealthGoldSymbol
    {
        SymbolCaibao    = 1,   //财宝 这个来出免费 
        SymbolWild      = 2,      //替代
        SymbolJingui    = 3,   //金龟
        SymbolJinyu     = 4,   //金鱼
        SymbolYuqi      = 5,   //玉器
        SymbolZhuzi     = 6,    //竹子
        SymbolFudai     = 7,    //福袋
        SymbolA         = 8,    //A
        SymbolK         = 9,    //K
        SymbolQ         = 10,    //Q
        SymbolJ         = 11,    //J
        SymbolTen       = 12,    //10
        SymbolNine      = 13,    //9
    }

    [Desc("生成图形数组和相关结果的基类")]
    class WealthGoldBase
    {
        [Desc("(一次旋转押注 免费游戏为0)")]
        double bet;
        [Desc("(一次旋转赢钱)")]
        double winCoin;
        [Desc("生成的15个图形集合")]
        List<Cell> grids;
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        wealthGoldSymbol icon;

    }

    class Line
    {
        [Desc("中奖线(eg:1号线) ")]
        int lineIndex;
        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;
        [Desc("一组中奖图标")]
        wealthGoldSymbol icon;
        List<Cell> lineCells;
    }
}
