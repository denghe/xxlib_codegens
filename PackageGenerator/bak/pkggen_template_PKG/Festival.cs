#pragma warning disable 0169, 0414
using TemplateLibrary;
namespace Festival
{
    enum FestivalSymbol
    {
        //传给客服端的从 1开始 服务器逻辑还是从0开始的
        FestivalMammon = 1,  //财神(替代)
        Festival9 = 2,  //9
        Festival10 = 3,  //10
        FestivalJ = 4,  //J
        FestivalQ = 5,  //Q
        FestivalK = 6,  //K
        FestivalFirecrackers = 7,  //鞭炮
        FestivalRedEnvelope = 8,  //红包
        FestivalMoneyTree = 9,  //摇钱树
        FestivalDrum = 10, //鼓
        FestivalLion = 11, //狮子(分散图案)
        FestivalLantern = 12, //灯笼(不定分值)
    }

    class Cell
    {
        [Desc("点对应下标")]
        int index;

        [Desc("点对应图形Id")]
        Festival.FestivalSymbol icon;

        [Desc("点对应图形Id的类型 0为常规图形 其他为有奖图形 1普通奖励 2小奖分值 3中奖分值 4大奖分值 5巨奖分值")]
        int FestivalSymbolType;

        [Desc("图形的彩金值")]
        double FestivalSymbolValue;
    }

    class Line
    {
        [Desc("中奖线(eg:1号线)")]
        int lineIndex;

        [Desc("赢钱(当前线赢的金币)")]
        double winCoin;

        [Desc("中奖的图标")]
        Festival.FestivalSymbol icon;

        [Desc("一条线上的中奖集合")]
        List<Cell> lineCells;
    }

    [Desc("生成图形数组和相关结果的基类")]
    class FestivalBase
    {
        [Desc("总押注(一次旋转 免费游戏为0)")]
        double bet;

        [Desc("总赢钱(一次旋转)")]
        double winCoin;

        [Desc("中奖转盘剩余次数")]
        int validSpinCount;

        [Desc("中奖转盘总次数")]
        int totalSpinCount;

        [Desc("免费游戏增加的替代图形数量")]
        int freeWildCount;

        [Desc("生成的15个图形集合")]
        List<Cell> grids;
    }
}
