﻿#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace EgyptianFantasy
{
    enum EgyptianFantasySymbol
    {
        EgyptianFantasyWild =1,
        EgyptianFantasyJinZiTa =2,
        EgyptianFantasyFaLao =3,
        EgyptianFantasyMao =4,
        EgyptianFantasyNiu =5,
        EgyptianFantasyJi =6,
        EgyptianFantasyTuTen =7,
        EgyptianFantasyA =8,
        EgyptianFantasyK =9,
        EgyptianFantasyQ =10,
        EgyptianFantasyJ =11,
        EgyptianFantasy10 =12,
        EgyptianFantasy9 =13,
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        EgyptianFantasySymbol icon;
    }
    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("一组中奖图标")]
        EgyptianFantasySymbol icon;

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

        [Desc("是否进入收集游戏")]
        int intoCollect;
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
        [Desc("再次中免费")]
        int newFreeTime;
        [Desc("中奖的线")]
        List<Line> lines;
    }
}
