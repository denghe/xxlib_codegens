
#pragma warning disable 0169, 0414
using TemplateLibrary;
//彩凤 215
namespace LotteryPhoenix
{
    enum LotteryPhoenixSymbol
    {
        SymbolWild      = 1,    //替代 
        SymbolCaibao    = 2,    //财宝 这个来出免费
        SymbolLong      = 3,    //龙
        SymbolWa        = 4,    //蛙
        SymbolYu        = 5,    //鱼
        SymbolGui       = 6,    //龟
        SymbolFudai     = 7,    //福袋
        SymbolA         = 8,    //A
        SymbolK         = 9,    //K
        SymbolQ         = 10,   //Q
        SymbolJ         = 11,   //J
        SymbolTen       = 12,   //10
        SymbolNine      = 13,   //9
    }
    class map
    {
        int index;
        int num;
    }
        [Desc("生成图形数组和相关结果的基类")]
    class LotteryPhoenixBase
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
        LotteryPhoenixSymbol icon;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        LotteryPhoenixSymbol icon;

        [Desc("一条线上的中奖集合")]
        List<Cell> lineCells;
    }
    class Lottery
    {
        [Desc("序号 约定 1 小2 中 3大 4巨 5 五福")]
        int Index;

        [Desc("倍率or值 倍率是0 值是 1")]
        int BetorMoney;

        [Desc("对应的奖金")]
        int Value;

    }
}
