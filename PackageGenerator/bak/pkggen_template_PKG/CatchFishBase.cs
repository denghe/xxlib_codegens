#pragma warning disable 0169, 0414
using TemplateLibrary;
namespace CatchFishBase
{
    enum PlayerStates
    {
        [Desc("等待连接中( 大厅通知游戏有玩家要进来, 此时创建的玩家就是这个状态. 或是玩家掉线, 也是这个状态. )")]
        Wait,
        [Desc("游戏中")]
        Play,
        [Desc("退出中( 准备向 db 发起 Leave 请求 )")]
        Leave0,
        [Desc("退出中( 正在等 db 的操作结果 )")]
        Leave1,
        [Desc("退出中( 正要向 lobby 发起请求 )")]
        Leave2,
        [Desc("退出中( 正在等 lobby 的操作结果 )")]
        Leave3,
        [Desc("退出中( 正要 Cleanup player )")]
        Leave4,
    }

}
