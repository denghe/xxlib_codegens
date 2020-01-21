#pragma warning disable 0169, 0414
using TemplateLibrary;

// 引用到外部类型

namespace xx
{
    // 坐标
    [External]
    struct Pos { }

    // 随机数发生器
    [External]
    class Random { }
}

//// 玩家网络连接（仅服务器端）
//[External(false, "")]
//class ClientPeerRef { }

//// 服务器网络连接（仅服务器端）
//[External(false, "")]
//class ServerPeerRef { }

//// 显示用节点（仅客户端）
//[External(false)]
//class NodePointer { }

//// 显示用精灵（仅客户端）
//[External(false)]
//class SpritePointer { }

//// 显示用精灵帧（仅客户端）
//[External(false)]
//class SpriteFramePointer { }

//// 显示用动画（仅客户端）
//[External(false)]
//class AnimationPointer { }

//// 物理计算用空间（仅客户端）
//[External(false)]
//class CpSpacePointer { }

//// （仅客户端）
//[External(false, "")]
//class Dict_Int_Int_p { }

//// 服务器使用
//[External(false, "")]
//class Player_Ext
//{
//    /// <summary>
//    /// 点放鱼死比例
//    /// </summary>
//    int fish_dead_proportion;

//    /// <summary>
//    /// 是否记录总压总得
//    /// </summary>
//    int is_cost_get;
//}
//[External(false, "")]
//class MoveObject_Ext { }
//[External(false, "")]
//class Background_Config_Ext { }
//[External(false, "")]
//class BornPoint_Ext { }
//[External(false, "")]
//class Background_Ext { }
//[External(false, "")]
//class Scene_Ext { }
//[External(false, "")]
//class FishType_Ext { }
//[External(false, "")]
//class Enter_Ext { }
//[External(false, "")]
//class Leave_Ext { }
//[External(false, "")]
//class update_get_cost_Ext { }
//[External(false, "")]
//class UpdateGiftMoney_Ext { }
//[External(false, "")]
//class GetAllServerNotice_Ext { }
//[External(false, "")]
//class log_game_catchfish_fish_statistic_Ext { }
//[External(false, "")]
//class catchfish_statistics_Ext { }
//[External(false, "")]
//class Register_Ext { }
//[External(false, "")]
//class Enter_Lobby_Success_Ext { }
//[External(false, "")]
//class Leave_Lobby_Ext { }
//[External(false, "")]
//class KillPlayerSuccess_Ext { }
//[External(false, "")]
//class ChangeNoticeReadState_Ext { }
//[External(false, "")]
//class PushLog_Ext { }
//[External(false, "")]
//class FishSpriteFrameBase_Ext { }
//[External(false, "")]
//class FishLine_Ext { }
//[External(false, "")]
//class FishConfig_Ext { }
//[External(false, "")]
//class Config_Ext { }
//[External(false, "")]
//class EventBase_Ext { }
//[External(false, "")]
//class Stage_Ext { }
//[External(false, "")]
//class Emitter_Ext { }
