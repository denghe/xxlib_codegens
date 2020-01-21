#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Extral_Ex
{

    [Desc("打击明细")]
    struct BulletHit
    {
       
        [Desc("鱼id 鱼主键1/1")]
        int fishId;

        [Desc("鱼币值")]
        long fishCoin;

        [Desc("玩家id 子弹主键1/3")]
        int playerId;

        [Desc("炮台id 子弹主键2/3")]
        int cannonId;

        [Desc("子弹id 子弹主键3/3")]
        int bulletId;

        [Desc("子弹类型")]
        int bulletType;

        [Desc("子弹数量( 炸弹, 强火力数量会超过 1 )")]
        int bulletCount;

        [Desc("子弹币值")]
        long bulletCoin;
    }
    struct RoomBulletHit
    {
        [Desc("房间Id")]
        int roomId;

        [Desc("游戏Id")]
        int gameId;

        [Desc("级别Id")]
        int levelId;

        List<BulletHit> hits;
    }
    struct RoomBulleHitRet
    {
        [Desc("房间Id")]
        int roomId;

        [Desc("游戏Id")]
        int gameId;

        [Desc("级别Id")]
        int levelId;

        List<FishDie> fd;

        List<BulletHitNull> bhn;
    }
    [Desc("鱼被打死的明细")]
    struct FishDie
    {
        int fishId;
        long fishCoin;
        int playerId;
        int cannonId;
        int bulletId;
        int bulletType;
        long bulletCoin;
    }

    [Desc("子弹打空明细")]
    struct BulletHitNull
    {
        int playerId;
        int cannonId;
        int bulletId;
        int bulletType;
        int bulletCount;
        long bulletCoin;
    }

    [Desc("直接向总输入追加数据( 应对点杀之类需求 )")]
    class Push
    {
        long value;
    }

    [Desc("直接向总输出追加数据( 应对点送之类需求 )")]
    class Pop
    {
        long value;
    }
    [Desc("鱼死计算")]
    class ReqHitCheck
    {
        int serviceId;
        List<RoomBulletHit> roomBulletHits;
    }

    [Desc("鱼死计算结果")]
    class HitCheckRet
    {
        List<RoomBulleHitRet> roomBulletHits;
    }
}