#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace LeprechaunStackGold
{
    enum LeprechaunStackGoldSymbol
    {
        StackGoldWild = 1,          //替代
        StackGoldBaoZang = 2,       //宝藏
        StackGoldJingLing = 3,      //精灵
        StackGoldShuiJing = 4,      //水井
        StackGoldTiQing = 5,        //提琴
        StackGoldPiJiu = 6,         //啤酒
        StackGoldYanDou = 7,        //烟斗
        StackGoldA = 8,             //A
        StackGoldK = 9,             //K
        StackGoldQ = 10,            //Q
        StackGoldJ = 11,            //J
        StackGoldTen = 12,          //10
        StackGoldNine = 13,	        //9
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;
        [Desc("点对应图形Id")]
        LeprechaunStackGoldSymbol icon;
    }
    class Freeinfo
    {
        int index;
        int time;
    }
    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        LeprechaunStackGoldSymbol icon;

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

        [Desc("进入免费游戏的次数")]
        int freeTime;
        List<Freeinfo> freelist;
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

        [Desc("进入免费游戏的次数")]
        int addFreeTime;
        List<Freeinfo> freelist;

    }
    class FreeType
    {
        int count;
    }


}