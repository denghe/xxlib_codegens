
#pragma warning disable 0169, 0414
using TemplateLibrary;
//彩凤 215
namespace PandaTreasures
{
    enum PandaTreasuresSymbol
    {
        SymbolWild = 1,    //替代 
        SymbolTurtle = 2,   //龟
        SymbolFish = 3,    //鱼
        SymbolBarA = 4,    //
        SymbolBarB = 5,    //
        SymbolSevenA = 6,    //
        SymbolSevenB = 7,    //
        SymbolSevenC = 8,    //
        SymbolFire = 9,    //火焰 多次出现彩金
        SymbolPanda =10,    //熊猫  出现免费游戏
    }
   
    [Desc("生成图形数组和相关结果的基类")]
    class PandaTreasuresBase
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
        PandaTreasuresSymbol icon;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标类型")]
        int type;

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
