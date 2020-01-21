#pragma warning disable 0169, 0414
using TemplateLibrary;
namespace Client_Slots
{
    [Desc("请求进入游戏")]
    class Enter
    {

    }

    [Desc("离开结算")]
    class Leave
    {

    }

    [Desc("玩家锁住状态离开")]
    class LockLeave
    {

    }

    [Desc("请求同步")]
    class Sync
    {

    }

    [Desc("Sugar请求获取生成图形")]
    class SugarNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("Sugar请求特殊旋转图形")]
    class SugarRainbowFreeSpin
    {

    }

    [Desc("Sugar请求特殊旋转图形")]
    class SugarResipnSpin
    {

    }

    [Desc("Festival请求获取生成图形")]
    class FestivalNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("Festival请求特殊旋转图形")]
    class FestivalLionFreeSpin
    {

    }

    [Desc("Festival请求特殊旋转图形")]
    class FestivalResipnSpin
    {

    }

    [Desc("财神道 请求获取生成图形")]
    class WealthGoldNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("财神道 请求免费类型")]
    class WealthGoldFreeType
    {
        [Desc("押注类型")]
        int type;
    }

    [Desc("财神道 免费游戏请求获取生成图形")]
    class WealthGoldFreeSpin
    {

    }

    [Desc("LuckyDiamond请求获取生成图形")]
    class LuckyDiamondNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("LuckyDiamond请求特殊旋转图形")]
    class LuckyDiamondFreeSpin
    {

    }


    [Desc("彩凤 215请求获取生成图形")]
    class LotteryPhoenixNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("彩凤 215 请求免费类型")]
    class LotteryPhoenixFreeType
    {
        [Desc("押注类型")]
        int type;
    }

    [Desc("彩凤 215 免费游戏请求获取生成图形")]
    class LotteryPhoenixFreeSpin
    {

    }
    [Desc("彩凤 215 获取彩金的值")]
    class LotteryPhoenixLottery
    {

    }
    [Desc("熊猫宝藏 请求获取生成图形")]
    class PandaTreasuresNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }
    [Desc("熊猫宝藏 请求免费类型")]
    class PandaTreasuresFreeType
    {
    }

    [Desc("熊猫宝藏 免费游戏请求获取生成图形")]
    class PandaTreasuresFreeSpin
    {

    }
    [Desc("熊猫宝藏 获取彩金的值")]
    class PandaTreasuresLottery
    {

    }

    [Desc("巨龙争霸214  请求获取生成图形")]
    class GrandDragonsNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("巨龙争霸214  请求免费类型")]
    class GrandDragonsFreeType
    {
        [Desc("押注类型")]
        int type;
    }

    [Desc("巨龙争霸214  免费游戏请求获取生成图形")]
    class GrandDragonsFreeSpin
    {

    }
    [Desc("金虎聚财213   请求获取生成图形")]
    class TigerWealthNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("金虎聚财213   请求免费类型")]
    class TigerWealthFreeType
    {
        [Desc("押注类型")]
        int type;
    }

    [Desc("金虎聚财213   免费游戏请求获取生成图形")]
    class TigerWealthFreeSpin
    {

    }

    [Desc("幸运美金222 请求获取生成图形")]
    class LuckyDollarNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("幸运美金222 请求特殊旋转图形")]
    class LuckyDollarSpecialSpin
    {

    }
   
    [Desc("所有游戏  测试专用包")]
    class TestNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
        [Desc("游戏元素")]
        List<int> cells;
    }
    [Desc("所有游戏  用于发送状态")]
    class pushType
    {
        int id;
        int a;
        int b;
    }

    [Desc("金币点击信息")]
    class CionInfo
    {
        List<int> cells;
    }

    [Desc("皇朝88 请求普通旋转图形")]
    class Empire88NormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }
    [Desc("皇朝88 请求免费类型")]
    class Empire88FreeType
    {
        [Desc("押注类型")]
        int type;
    }
    [Desc("皇朝88 请求免费旋转图形")]
    class Empire88FreeSpin
    { }
    [Desc("皇朝88 请求特殊旋转图形")]
    class Empire88SpecialSpin
    { }
    
    [Desc("狂野蛮牛 请求普通旋转图形")]
    class AfricanBuffaloNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }
   
    [Desc("狂野蛮牛 请求免费旋转图形")]
    class AfricanBuffaloFreeSpin
    { }

    [Desc("齐天大圣 请求普通旋转图形")]
    class ImmortalMonkeyNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("齐天大圣 请求免费旋转图形")]
    class ImmortalMonkeyFreeSpin
    { }


    [Desc("龙海公主 请求普通旋转图形")]
    class OceanPrincessNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("龙海公主 请求免费旋转图形")]
    class OceanPrincessFreeSpin
    { }

   
    [Desc("精灵宝藏 请求普通旋转图形")]
    class LeprechaunStackGoldNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("精灵宝藏   请求免费类型")]
    class LeprechaunStackGoldFreeType
    {
        [Desc("押注类型")]
        int type;
    }
    [Desc("精灵宝藏 请求免费旋转图形")]
    class LeprechaunStackGoldFreeSpin
    { }

    [Desc("维加斯之夜请求获取生成图形")]
    class VegasNightNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("维加斯之夜 请求免费旋转图形")]
    class VegasNightFreeSpin
    {

    }

    [Desc("维加斯之夜请求特殊旋转图形")]
    class VegasNightSpecialSpin
    {

    }
    //竖屏游戏 
    [Desc("法老宝藏 请求普通旋转图形")]
    class EgyptianFantasyNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }

    [Desc("法老宝藏 请求免费旋转图形")]
    class EgyptianFantasyFreeSpin
    { }

    [Desc("发财树 请求普通旋转图形")]
    class FortuneTreeNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }
    [Desc("发财树 请求免费类型")]
    class FortuneTreeFreeType
    {
        [Desc("押注类型")]
        int type;
    }
    [Desc("发财树 请求免费旋转图形")]
    class FortuneTreeFreeSpin
    { }

    [Desc("黄金88 请求普通旋转图形")]
    class Golden88NormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }
    [Desc("黄金88 请求免费旋转图形")]
    class Golden88FreeType
    {
        [Desc("押注类型")]
        int type;
    }

    [Desc("黄金88 请求免费旋转图形")]
    class Golden88FreeSpin
    { }


    [Desc("招财猫 请求获取生成图形")]
    class FortuneCatNormalSpin
    {
        [Desc("押注金额")]
        double betMoney;
    }
    [Desc("招财猫 请求免费旋转图形")]
    class FortuneCatFreeSpin
    {}
    [Desc("招财猫 请求特殊旋转图形")]
    class FortuneCatSpecialSpin
    {}


}
