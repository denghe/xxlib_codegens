#pragma warning disable 0169, 0414
using TemplateLibrary;
namespace SlotsBase
{
    class BetRatio
    {
       [Desc("押注金额")]
       double betMoney;

        [Desc("押注数")]
        int betLine;
    }

    class EntryConditions
    {
        [Desc("算法级别ID")]
        int id;

        [Desc("押注区间:最小值")]
        double spinMinMoney;

        [Desc("押注区间:最大值")]
        double spinMaxMoney;

        [Desc("进入该算法区间的最小值")]
        double enterMinMoney;

        [Desc("描述名称")]
        string desc;
    };
}
