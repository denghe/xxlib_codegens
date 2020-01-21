#pragma warning disable 0169, 0414
using TemplateLibrary;

[TypeIdMappings]
interface ITypeIdMappings
{
    WealthGold.WealthGoldBase _10001 { get; }

    List<WealthGold.Cell> _10002 { get; }

    WealthGold.Cell _10003 { get; }

    WealthGold.Line _10004 { get; }

    VegasNight.Cell _10005 { get; }

    VegasNight.Line _10006 { get; }

    List<VegasNight.Cell> _10007 { get; }

    VegasNight.NormalSpin _10008 { get; }

    List<VegasNight.Line> _10009 { get; }

    VegasNight.FreeSpin _10010 { get; }

    TigerWealth.map _10011 { get; }

    TigerWealth.TigerWealthBase _10012 { get; }

    List<TigerWealth.Cell> _10013 { get; }

    TigerWealth.Cell _10014 { get; }

    List<int> _10015 { get; }

    List<TigerWealth.map> _10016 { get; }

    TigerWealth.Line _10017 { get; }

    GrandDragons.GrandDragonsBase _10018 { get; }

    List<GrandDragons.Cell> _10019 { get; }

    GrandDragons.Cell _10040 { get; }

    GrandDragons.Line _10041 { get; }

    Sugar.SugarBase _10042 { get; }

    List<Sugar.Cell> _10043 { get; }

    Sugar.Cell _10044 { get; }

    Sugar.Line _10045 { get; }

    Slots_Lobby.RegisterSlotsService _10046 { get; }

    Slots_Lobby.EnterRet _10047 { get; }

    Slots_Lobby.Leave _10048 { get; }

    Slots_Lobby.LockLeave _10049 { get; }

    Slots_Lobby.PlayingGameMoneyChanges _10050 { get; }

    List<Generic.OnceChange> _10051 { get; }

    Generic.OnceChange _10052 { get; }

    Slots_Lobby.UpdateLogId _10053 { get; }

    Slots_Client.OfflineCheck _10054 { get; }

    Slots_Client.Leave_Success _10055 { get; }

    Slots_Client.LockLeaveSuccess _10056 { get; }

    Slots_Client.Enter_Success _10057 { get; }

    List<SlotsBase.BetRatio> _10058 { get; }

    SlotsBase.BetRatio _10059 { get; }

    List<SlotsBase.EntryConditions> _20000 { get; }

    SlotsBase.EntryConditions _20001 { get; }

    Slots_Client.CollectValue _20002 { get; }

    Slots_Client.SugarSpinNormalRet _20003 { get; }

    List<Sugar.Line> _20004 { get; }

    Slots_Client.SugarRainbowFreeRet _20005 { get; }

    Slots_Client.SugarResipnRet _20006 { get; }

    Slots_Client.SugarEnterResumed _20007 { get; }

    Slots_Client.FestivalSpinNormalRet _20008 { get; }

    Festival.FestivalBase _20009 { get; }

    Festival.Line _20010 { get; }

    List<Festival.Line> _20011 { get; }

    Slots_Client.FestivalLionFreeRet _20012 { get; }

    Slots_Client.FestivalResipnRet _20013 { get; }

    Slots_Client.FestivalEnterResumed _20014 { get; }

    Slots_Client.WealthGoldNormalRet _20015 { get; }

    List<WealthGold.Line> _20016 { get; }

    Slots_Client.WealthGoldFreeRet _20017 { get; }

    Slots_Client.WealthGoldFreeRetType _20018 { get; }

    Slots_Client.WealthGoldEnterResumed _20019 { get; }

    Slots_Client.LuckyDiamondSpinNormalRet _20020 { get; }

    LuckyDiamond.LuckyDiamondBase _20021 { get; }

    LuckyDiamond.Line _20022 { get; }

    List<LuckyDiamond.Line> _20023 { get; }

    Slots_Client.LuckyDiamondFreeRet _20024 { get; }

    Slots_Client.LuckyDiamondEnterResumed _20025 { get; }

    Slots_Client.LotteryPhoenixNormalRet _20026 { get; }

    LotteryPhoenix.LotteryPhoenixBase _20027 { get; }

    List<LotteryPhoenix.Line> _20028 { get; }

    LotteryPhoenix.Line _20029 { get; }

    Slots_Client.LotteryPhoenixFreeRet _20030 { get; }

    Slots_Client.LotteryPhoenixFreeRetType _20031 { get; }

    Slots_Client.LotteryPhoenixEnterResumed _20032 { get; }

    Slots_Client.LotteryPhoenixLotteryRet _20033 { get; }

    List<LotteryPhoenix.Lottery> _20034 { get; }

    LotteryPhoenix.Lottery _20035 { get; }

    Slots_Client.PandaTreasuresNormalRet _20036 { get; }

    PandaTreasures.PandaTreasuresBase _20037 { get; }

    List<PandaTreasures.Line> _20038 { get; }

    PandaTreasures.Line _20039 { get; }

    Slots_Client.PandaTreasuresFreeRet _20040 { get; }

    Slots_Client.PandaTreasuresFreeRetType _20041 { get; }

    Slots_Client.PandaTreasuresEnterResumed _20042 { get; }

    Slots_Client.PandaTreasuresLotteryRet _20043 { get; }

    List<PandaTreasures.Lottery> _20044 { get; }

    PandaTreasures.Lottery _20045 { get; }

    Slots_Client.GrandDragonsNormalRet _20046 { get; }

    List<GrandDragons.Line> _20047 { get; }

    Slots_Client.GrandDragonsFreeRet _20048 { get; }

    Slots_Client.GrandDragonsFreeRetType _20049 { get; }

    Slots_Client.GrandDragonsEnterResumed _20050 { get; }

    Slots_Client.TigerWealthNormalRet _20051 { get; }

    List<TigerWealth.Line> _20052 { get; }

    Slots_Client.TigerWealthFreeRet _20053 { get; }

    Slots_Client.TigerWealthFreeRetType _20054 { get; }

    Slots_Client.TigerWealthEnterResumed _20055 { get; }

    Slots_Client.LuckyDollarNormalRet _20056 { get; }

    LuckyDollar.LuckyDollarBase _20057 { get; }

    List<LuckyDollar.Line> _20058 { get; }

    LuckyDollar.Line _20059 { get; }

    Slots_Client.LuckyDollarSpecialRet _20060 { get; }

    LuckyDollar.LuckyDollarSpecial _20061 { get; }

    Slots_Client.LuckyDollarEnterResumed _20062 { get; }

    Slots_Client.Empire88NormalRet _20063 { get; }

    Empire88.NormalSpin _20064 { get; }

    Slots_Client.Empire88FreeRetType _20065 { get; }

    Empire88.FreeType _20066 { get; }

    Slots_Client.Empire88FreeRet _20067 { get; }

    Empire88.FreeSpin _20068 { get; }

    Slots_Client.Empire88SpecialRet _20069 { get; }

    Empire88.SpecialSpin _20070 { get; }

    Slots_Client.Empire88EnterResumed _20071 { get; }

    Slots_Client.AfricanBuffaloNormalRet _20072 { get; }

    AfricanBuffalo.NormalSpin _20073 { get; }

    Slots_Client.AfricanBuffaloFreeRet _20074 { get; }

    AfricanBuffalo.FreeSpin _20075 { get; }

    Slots_Client.AfricanBuffaloEnterResumed _20076 { get; }

    Slots_Client.ImmortalMonkeyNormalRet _20077 { get; }

    ImmortalMonkey.NormalSpin _20078 { get; }

    Slots_Client.ImmortalMonkeyFreeRet _20079 { get; }

    ImmortalMonkey.FreeSpin _20080 { get; }

    Slots_Client.ImmortalMonkeyEnterResumed _20081 { get; }

    Slots_Client.OceanPrincessNormalRet _20082 { get; }

    OceanPrincess.NormalSpin _20083 { get; }

    Slots_Client.OceanPrincessFreeRet _20084 { get; }

    OceanPrincess.FreeSpin _20085 { get; }

    Slots_Client.OceanPrincessEnterResumed _20086 { get; }

    Slots_Client.LeprechaunStackGoldNormalRet _20087 { get; }

    LeprechaunStackGold.NormalSpin _20088 { get; }

    Slots_Client.LeprechaunStackGoldFreeRet _20089 { get; }

    LeprechaunStackGold.FreeSpin _20090 { get; }

    Slots_Client.LeprechaunStackGoldFreeTypeRet _20091 { get; }

    LeprechaunStackGold.FreeType _20092 { get; }

    Slots_Client.LeprechaunStackGoldEnterResumed _20093 { get; }

    Slots_Client.VegasNightNormalRet _20094 { get; }

    Slots_Client.VegasNightFreeRet _20095 { get; }

    Slots_Client.VegasNightSpecialRet _20096 { get; }

    Slots_Client.VegasNightEnterResumed _20097 { get; }

    Slots_Client.EgyptianFantasyNormalRet _20098 { get; }

    EgyptianFantasy.NormalSpin _20099 { get; }

    Slots_Client.EgyptianFantasyFreeRet _20100 { get; }

    EgyptianFantasy.FreeSpin _20101 { get; }

    Slots_Client.EgyptianFantasyEnterResumed _20102 { get; }

    Slots_Client.FortuneTreeNormalRet _20103 { get; }

    FortuneTree.NormalSpin _20104 { get; }

    Slots_Client.FortuneTreeFreeRetType _20105 { get; }

    FortuneTree.FreeType _20106 { get; }

    Slots_Client.FortuneTreeFreeRet _20107 { get; }

    FortuneTree.FreeSpin _20108 { get; }

    Slots_Client.FortuneTreeEnterResumed _20109 { get; }

    Slots_Client.Golden88NormalRet _20110 { get; }

    Golden88.NormalSpin _20111 { get; }

    Slots_Client.Golden88FreeRetType _20112 { get; }

    Golden88.FreeType _20113 { get; }

    Slots_Client.Golden88FreeRet _20114 { get; }

    Golden88.FreeSpin _20115 { get; }

    Slots_Client.Golden88EnterResumed _20116 { get; }

    Slots_Client.FortuneCatNormalRet _20117 { get; }

    FortuneCat.NormalSpin _20118 { get; }

    Slots_Client.FortuneCatFreeRet _20119 { get; }

    FortuneCat.FreeSpin _20120 { get; }

    Slots_Client.FortuneCatSpecialRet _20121 { get; }

    Slots_Client.FortuneCatEnterResumed _20122 { get; }

    Robot_Lobby.RegisterRobotService _20123 { get; }

    Robot_Lobby.GetOnlines _20124 { get; }

    Robot_Lobby.UpdateGameConfig _20125 { get; }

    Robot_Lobby.UpdateRobot _20126 { get; }

    Robot_Lobby.GetAccount _20127 { get; }

    Robot_Lobby.MakeAccount _20128 { get; }

    Robot_Lobby.GetLobby _20129 { get; }

    Robot_Lobby.RobotManager _20130 { get; }

    Robot_Lobby.ChangeMoney _20131 { get; }

    Generic.Success _20132 { get; }

    Generic.Fail _20133 { get; }

    Generic.Error _20134 { get; }

    Generic.Ping _20135 { get; }

    Generic.Pong _20136 { get; }

    Generic.KeepHeartbeat _20137 { get; }

    Generic.UpdateGiftMoney _20138 { get; }

    Generic.ChangeKillPercent _20139 { get; }

    Generic.StopWinLoseControl _20140 { get; }

    Generic.NowWinControlMoney _20141 { get; }

    Generic.SlotsMarquee _20142 { get; }

    List<PandaTreasures.Cell> _20143 { get; }

    PandaTreasures.Cell _20144 { get; }

    OceanPrincess.map _20145 { get; }

    OceanPrincess.Cell _20146 { get; }

    OceanPrincess.Line _20147 { get; }

    List<OceanPrincess.Cell> _20148 { get; }

    List<OceanPrincess.Line> _20149 { get; }

    List<OceanPrincess.map> _20150 { get; }

    List<LuckyDollar.Cell> _20151 { get; }

    LuckyDollar.Cell _20152 { get; }

    LuckyDollar.RangeA _20153 { get; }

    LuckyDollar.RangeB _20154 { get; }

    List<LuckyDollar.RangeA> _20155 { get; }

    List<LuckyDollar.RangeB> _20156 { get; }

    LuckyDiamond.Cell _20157 { get; }

    List<LuckyDiamond.Cell> _20158 { get; }

    LotteryPhoenix.map _20159 { get; }

    List<LotteryPhoenix.Cell> _20160 { get; }

    LotteryPhoenix.Cell _20161 { get; }

    List<LotteryPhoenix.map> _20162 { get; }

    Login_Client.Auth_Success_Lobby _20163 { get; }

    Lobby_Client.selfAccount _20164 { get; }

    List<Lobby_Client.Vip> _20165 { get; }

    Lobby_Client.Vip _20166 { get; }

    Login_Client.Auth_Success_Game _20167 { get; }

    Login_Client.RequireVerifyCode_Success _20168 { get; }

    Login_Client.GetUpdatePath_Success _20169 { get; }

    List<Login_Client.GameUpdatePath> _20170 { get; }

    Login_Client.GameUpdatePath _20171 { get; }

    Login_Client.GameEnforceUpdatePath _20172 { get; }

    Login_Client.ReceivedPackagetType _20173 { get; }

    Login_Client.ReceivedPromotionCode _20174 { get; }

    Login_Client.ReceivedVerificationFacebook _20175 { get; }

    Lobby_Slots.Enter _20176 { get; }

    Lobby_Slots.KillPlayer _20177 { get; }

    Lobby_Slots.ReEnter _20178 { get; }

    Lobby_Robot.RobotAccount _20179 { get; }

    Lobby_Robot.GetRobotAccount _20180 { get; }

    List<Lobby_Robot.RobotAccount> _20181 { get; }

    Lobby_Robot.UpdateRobot _20182 { get; }

    Lobby_Robot.RobotInfo _20183 { get; }

    Lobby_Robot.PushRobot _20184 { get; }

    List<Lobby_Robot.RobotInfo> _20185 { get; }

    Lobby_Robot.PopRobot _20186 { get; }

    Lobby_Robot.PushRobotAccount _20187 { get; }

    Lobby_Robot.MakeRobotAccount _20188 { get; }

    Lobby_Robot.robot_manager _20189 { get; }

    Lobby_Robot.GetRobotManager _20190 { get; }

    List<Lobby_Robot.robot_manager> _20191 { get; }

    Lobby_Robot.ChangeMoney _20192 { get; }

    Lobby_Client.Move_Result _20193 { get; }

    Lobby_Client.Enter_Success _20194 { get; }

    List<Lobby_Client.GameEntryConditions> _20195 { get; }

    Lobby_Client.GameEntryConditions _20196 { get; }

    Lobby_Client.UserInfo _20197 { get; }

    List<Lobby_Client.PayChannelAccount> _20198 { get; }

    Lobby_Client.PayChannelAccount _20199 { get; }

    Lobby_Client.Enter_Success_For_RobotManager _20200 { get; }

    Lobby_Client.GameCatchFish_LevelInfo _20201 { get; }

    Lobby_Client.EnterGameCatchFish_Success _20202 { get; }

    List<Lobby_Client.GameCatchFish_LevelInfo> _20203 { get; }

    Lobby_Client.GameHuca_LevelInfo _20204 { get; }

    Lobby_Client.EnterGameHuca_Success _20205 { get; }

    List<Lobby_Client.GameHuca_LevelInfo> _20206 { get; }

    Lobby_Client.GameCatchFish_RoomInfo _20207 { get; }

    List<Lobby_Client.RoomPlayers> _20208 { get; }

    Lobby_Client.RoomPlayers _20209 { get; }

    Lobby_Client.EnterGameCatchFishLevel_Success _20210 { get; }

    List<Lobby_Client.GameCatchFish_RoomInfo> _20211 { get; }

    Lobby_Client.EnterGameCatchFishLevelRoomSit_Success _20212 { get; }

    Lobby_Client.EnterGameSlots_Success _20213 { get; }

    Lobby_Client.Restore_Success _20214 { get; }

    List<Lobby_Client.Move_Result> _20215 { get; }

    Lobby_Client.ChangeNickname_Success _20216 { get; }

    Lobby_Client.ChangeAvatar_Success _20217 { get; }

    Lobby_Client.RequireVerifyCode_Success _20218 { get; }

    Lobby_Client.withdrawals _20219 { get; }

    Lobby_Client.GetWithdrawalsList _20220 { get; }

    List<Lobby_Client.withdrawals> _20221 { get; }

    Lobby_Client.ChangeMoneySafe_Success _20222 { get; }

    Lobby_Client.TodayEarnMoney _20223 { get; }

    Lobby_Client.GetTodayEarnMoneys _20224 { get; }

    List<Lobby_Client.TodayEarnMoney> _20225 { get; }

    Lobby_Client.Notice _20226 { get; }

    Lobby_Client.GetNotices _20227 { get; }

    List<Lobby_Client.Notice> _20228 { get; }

    Lobby_Client.GetRechargeMoney _20229 { get; }

    Lobby_Client.PayChannel _20230 { get; }

    Lobby_Client.vip_channel _20231 { get; }

    Lobby_Client.config_channel_money _20232 { get; }

    Lobby_Client.GetRecharges _20233 { get; }

    List<Lobby_Client.PayChannel> _20234 { get; }

    List<Lobby_Client.vip_channel> _20235 { get; }

    List<Lobby_Client.config_channel_money> _20236 { get; }

    Lobby_Client.GetWithdrawals _20237 { get; }

    Lobby_Client.BindPhone_Success _20238 { get; }

    Lobby_Client.MoneyChanged _20239 { get; }

    Lobby_Client.WithdrawalsResultMoneyChanged _20240 { get; }

    Lobby_Client.FormalRegistMoney _20241 { get; }

    Lobby_Client.ClientRechargeRequestSuccess _20242 { get; }

    Lobby_Client.Website _20243 { get; }

    Lobby_Client.ClientRechargeResult _20244 { get; }

    Lobby_Client.OtherDeviceLogin _20245 { get; }

    Lobby_Client.GiftMoneyResult _20246 { get; }

    Lobby_Client.ReceivedMoneyResult _20247 { get; }

    Lobby_Client.ApplyPromotionResult _20248 { get; }

    Lobby_Client.PromotionDetail _20249 { get; }

    Lobby_Client.PromotionProfit _20250 { get; }

    List<Lobby_Client.PromotionUser> _20251 { get; }

    Lobby_Client.PromotionUser _20252 { get; }

    Lobby_Client.ReceivedPromotion _20253 { get; }

    Lobby_Client.PromotionWithdrawalsSuccess _20254 { get; }

    Lobby_Client.PromotionWithdrawalsList _20255 { get; }

    List<Lobby_Client.PromotionWithdrawals> _20256 { get; }

    Lobby_Client.PromotionWithdrawals _20257 { get; }

    Lobby_Client.PromotionWithdrawalsCycleLimitdSurplusTime _20258 { get; }

    Lobby_Client.VerificationFacebook_Success _20259 { get; }

    Lobby_Client.everyday_signin_monery_config _20260 { get; }

    Lobby_Client.EverydaySigninInfo _20261 { get; }

    List<Lobby_Client.everyday_signin_monery_config> _20262 { get; }

    Lobby_Client.SigninToDayResult _20263 { get; }

    Lobby_Client.log_gift_money_to_account _20264 { get; }

    Lobby_Client.ReceivedGiftRecord _20265 { get; }

    List<Lobby_Client.log_gift_money_to_account> _20266 { get; }

    Lobby_Client.ReturnLobby _20267 { get; }

    Lobby_Client.GetPlaceRet _20268 { get; }

    List<Lobby_Client.EntryConditions> _20269 { get; }

    Lobby_Client.EntryConditions _20270 { get; }

    Lobby_Client.ReRouteNetWork _20271 { get; }

    Lobby_Client.ChangePayChannelBank_Success _20272 { get; }

    Lobby_Client.UnLockAccount _20273 { get; }

    Lobby_Client.Marquee.Marquee _20274 { get; }

    Lobby_Client.Marquee.GetGMMarquees _20275 { get; }

    List<Lobby_Client.Marquee.Marquee> _20276 { get; }

    Lobby_Client.Marquee.GetMarquees _20277 { get; }

    List<object> _20278 { get; }

    Lobby_Client.Marquee.CatchFishMarquees _20279 { get; }

    Lobby_Client.Marquee.GetCatchFishMarquees _20280 { get; }

    Lobby_Client.Marquee.HucaMarquees _20281 { get; }

    Lobby_Client.Marquee.GetHucaMarquees _20282 { get; }

    Lobby_Client.Events.PlayerSitdown _20283 { get; }

    Lobby_Client.Events.PlayerStandup _20284 { get; }

    Lobby_CatchFish2.Enter _20285 { get; }

    List<CatchFish.FishType> _20286 { get; }

    CatchFish.FishType _20287 { get; }

    Lobby_CatchFish2.KillPlayer _20288 { get; }

    Lobby_CatchFish2.ChangeKillPercent _20289 { get; }

    Lobby_CatchFish2.UpdateGiftMoney _20290 { get; }

    Lobby_CatchFish2.UpdateLevelCannon _20291 { get; }

    List<CatchFish.game_level_cannon> _20292 { get; }

    CatchFish.game_level_cannon _20293 { get; }

    Lobby_CatchFish2.ReEnter _20294 { get; }

    Lobby_CatchFish2.Notice _20295 { get; }

    Lobby_CatchFish2.GetNotices _20296 { get; }

    List<Lobby_CatchFish2.Notice> _20297 { get; }

    Lobby_CatchFish2.push_game_config _20298 { get; }

    List<Lobby_CatchFish2.marqueeconfig> _20299 { get; }

    Lobby_CatchFish2.marqueeconfig _20300 { get; }

    Lobby_CatchFish.Register_Success _20301 { get; }

    Lobby_CatchFish.Enter _20302 { get; }

    Lobby_CatchFish.Restore _20303 { get; }

    Lobby_CatchFish.KillPlayer _20304 { get; }

    Lobby_CatchFish.ChangeKillPercent _20305 { get; }

    Lobby_CatchFish.UpdateGiftMoney _20306 { get; }

    Lobby_CatchFish.Leave _20307 { get; }

    Lobby_CatchFish.Notice _20308 { get; }

    Lobby_CatchFish.GetNotices _20309 { get; }

    List<Lobby_CatchFish.Notice> _20310 { get; }

    Lobby_CatchFish.KeepHeartbeat _20311 { get; }

    Lobby_CatchFish.UpdateLevelCannon _20312 { get; }

    Lobby_CatchFish.ReEnter _20313 { get; }

    LeprechaunStackGold.Cell _20314 { get; }

    LeprechaunStackGold.Freeinfo _20315 { get; }

    LeprechaunStackGold.Line _20316 { get; }

    List<LeprechaunStackGold.Cell> _20317 { get; }

    List<LeprechaunStackGold.Freeinfo> _20318 { get; }

    List<LeprechaunStackGold.Line> _20319 { get; }

    ImmortalMonkey.Cell _20320 { get; }

    ImmortalMonkey.Line _20321 { get; }

    List<ImmortalMonkey.Cell> _20322 { get; }

    List<ImmortalMonkey.Line> _20323 { get; }

    Golden88.Cell _20324 { get; }

    Golden88.Line _20325 { get; }

    List<Golden88.Cell> _20326 { get; }

    List<Golden88.Line> _20327 { get; }

    FortuneTree.Cell _20328 { get; }

    FortuneTree.Line _20329 { get; }

    List<FortuneTree.Cell> _20330 { get; }

    List<FortuneTree.Line> _20331 { get; }

    FortuneCat.Cell _20332 { get; }

    FortuneCat.Line _20333 { get; }

    List<FortuneCat.Cell> _20334 { get; }

    List<FortuneCat.Line> _20335 { get; }

    Festival.Cell _20336 { get; }

    List<Festival.Cell> _20337 { get; }

    Extral_Ex.Push _20338 { get; }

    Extral_Ex.Pop _20339 { get; }

    Extral_Ex.ReqHitCheck _20340 { get; }

    List<Extral_Ex.RoomBulletHit> _20341 { get; }

    Extral_Ex.HitCheckRet _20342 { get; }

    List<Extral_Ex.RoomBulleHitRet> _20343 { get; }

    Client_Slots.Enter _20344 { get; }

    Client_Slots.Leave _20345 { get; }

    Client_Slots.LockLeave _20346 { get; }

    Client_Slots.Sync _20347 { get; }

    Client_Slots.SugarNormalSpin _20348 { get; }

    Client_Slots.SugarRainbowFreeSpin _20349 { get; }

    Client_Slots.SugarResipnSpin _20350 { get; }

    Client_Slots.FestivalNormalSpin _20351 { get; }

    Client_Slots.FestivalLionFreeSpin _20352 { get; }

    Client_Slots.FestivalResipnSpin _20353 { get; }

    Client_Slots.WealthGoldNormalSpin _20354 { get; }

    Client_Slots.WealthGoldFreeType _20355 { get; }

    Client_Slots.WealthGoldFreeSpin _20356 { get; }

    Client_Slots.LuckyDiamondNormalSpin _20357 { get; }

    Client_Slots.LuckyDiamondFreeSpin _20358 { get; }

    Client_Slots.LotteryPhoenixNormalSpin _20359 { get; }

    Client_Slots.LotteryPhoenixFreeType _20360 { get; }

    Client_Slots.LotteryPhoenixFreeSpin _20361 { get; }

    Client_Slots.LotteryPhoenixLottery _20362 { get; }

    Client_Slots.PandaTreasuresNormalSpin _20363 { get; }

    Client_Slots.PandaTreasuresFreeType _20364 { get; }

    Client_Slots.PandaTreasuresFreeSpin _20365 { get; }

    Client_Slots.PandaTreasuresLottery _20366 { get; }

    Client_Slots.GrandDragonsNormalSpin _20367 { get; }

    Client_Slots.GrandDragonsFreeType _20368 { get; }

    Client_Slots.GrandDragonsFreeSpin _20369 { get; }

    Client_Slots.TigerWealthNormalSpin _20370 { get; }

    Client_Slots.TigerWealthFreeType _20371 { get; }

    Client_Slots.TigerWealthFreeSpin _20372 { get; }

    Client_Slots.LuckyDollarNormalSpin _20373 { get; }

    Client_Slots.LuckyDollarSpecialSpin _20374 { get; }

    Client_Slots.TestNormalSpin _20375 { get; }

    Client_Slots.pushType _20376 { get; }

    Client_Slots.CionInfo _20377 { get; }

    Client_Slots.Empire88NormalSpin _20378 { get; }

    Client_Slots.Empire88FreeType _20379 { get; }

    Client_Slots.Empire88FreeSpin _20380 { get; }

    Client_Slots.Empire88SpecialSpin _20381 { get; }

    Client_Slots.AfricanBuffaloNormalSpin _20382 { get; }

    Client_Slots.AfricanBuffaloFreeSpin _20383 { get; }

    Client_Slots.ImmortalMonkeyNormalSpin _20384 { get; }

    Client_Slots.ImmortalMonkeyFreeSpin _20385 { get; }

    Client_Slots.OceanPrincessNormalSpin _20386 { get; }

    Client_Slots.OceanPrincessFreeSpin _20387 { get; }

    Client_Slots.LeprechaunStackGoldNormalSpin _20388 { get; }

    Client_Slots.LeprechaunStackGoldFreeType _20389 { get; }

    Client_Slots.LeprechaunStackGoldFreeSpin _20390 { get; }

    Client_Slots.VegasNightNormalSpin _20391 { get; }

    Client_Slots.VegasNightFreeSpin _20392 { get; }

    Client_Slots.VegasNightSpecialSpin _20393 { get; }

    Client_Slots.EgyptianFantasyNormalSpin _20394 { get; }

    Client_Slots.EgyptianFantasyFreeSpin _20395 { get; }

    Client_Slots.FortuneTreeNormalSpin _20396 { get; }

    Client_Slots.FortuneTreeFreeType _20397 { get; }

    Client_Slots.FortuneTreeFreeSpin _20398 { get; }

    Client_Slots.Golden88NormalSpin _20399 { get; }

    Client_Slots.Golden88FreeType _20400 { get; }

    Client_Slots.Golden88FreeSpin _20401 { get; }

    Client_Slots.FortuneCatNormalSpin _20402 { get; }

    Client_Slots.FortuneCatFreeSpin _20403 { get; }

    Client_Slots.FortuneCatSpecialSpin _20404 { get; }

    Client_Login.ClientPackagetType _20405 { get; }

    Client_Login.ClientPromotionCode _20406 { get; }

    Client_Login.ClientType _20407 { get; }

    Client_Login.Auth _20408 { get; }

    Client_Login.AuthByPhone _20409 { get; }

    Client_Login.AuthByUsername _20410 { get; }

    Client_Login.AuthByPhonePassword _20411 { get; }

    Client_Login.RequireVerifyCode _20412 { get; }

    Client_Login.ResetPassword _20413 { get; }

    Client_Login.GetUpdatePath _20414 { get; }

    Client_Login.ClientVerificationFacebook _20415 { get; }

    Client_Lobby.Move _20416 { get; }

    Client_Lobby.Enter _20417 { get; }

    Client_Lobby.EnterGame _20418 { get; }

    Client_Lobby.EnterGameCatchFishLevel _20419 { get; }

    Client_Lobby.EnterGameCatchFishLevelRoomSit _20420 { get; }

    Client_Lobby.ReturnUp _20421 { get; }

    Client_Lobby.Restore _20422 { get; }

    List<Client_Lobby.Move> _20423 { get; }

    Client_Lobby.OwnInfo _20424 { get; }

    Client_Lobby.ChangeNickname _20425 { get; }

    Client_Lobby.ChangeAvatar _20426 { get; }

    Client_Lobby.RequireVerifyCodeForBindPhone _20427 { get; }

    Client_Lobby.RequireVerifyCodeForResetPassword _20428 { get; }

    Client_Lobby.BindPhone _20429 { get; }

    Client_Lobby.ResetPassword _20430 { get; }

    Client_Lobby.ChangePassword _20431 { get; }

    Client_Lobby.Withdrawals _20432 { get; }

    Client_Lobby.GetWithdrawalsList _20433 { get; }

    Client_Lobby.GetTodayEarnMoney _20434 { get; }

    Client_Lobby.ChangeMoneySafe _20435 { get; }

    Client_Lobby.GetNotice _20436 { get; }

    Client_Lobby.Chat _20437 { get; }

    Client_Lobby.ChangeReadState _20438 { get; }

    Client_Lobby.GetMarquee _20439 { get; }

    Client_Lobby.ChangePayChannelAlipay _20440 { get; }

    Client_Lobby.ChangePayChannelBank _20441 { get; }

    Client_Lobby.GetRecharge _20442 { get; }

    Client_Lobby.GetWithdrawals _20443 { get; }

    Client_Lobby.GetFormalRegistMoney _20444 { get; }

    Client_Lobby.ClientRechargeRequest _20445 { get; }

    Client_Lobby.Login_out _20446 { get; }

    Client_Lobby.GetWebsite _20447 { get; }

    Client_Lobby.GetClientRechargeResult _20448 { get; }

    Client_Lobby.ClientGiftPassword _20449 { get; }

    Client_Lobby.ClientGiftMoney _20450 { get; }

    Client_Lobby.ClientApplyPromotion _20451 { get; }

    Client_Lobby.ClientPromotion _20452 { get; }

    Client_Lobby.ClientBindPromotionCode _20453 { get; }

    Client_Lobby.ClientApplyPromotionWithdrawals _20454 { get; }

    Client_Lobby.ClientPromotionWithdrawalsList _20455 { get; }

    Client_Lobby.GetPromotionWithdrawalsCycleLimitdSurplusTime _20456 { get; }

    Client_Lobby.ClientVerificationFacebook _20457 { get; }

    Client_Lobby.Get_SigninGaveInfo _20458 { get; }

    Client_Lobby.SigninToDay _20459 { get; }

    Client_Lobby.ClientGiftRecord _20460 { get; }

    Client_Lobby.ClientRechargeRequestPayerInfo _20461 { get; }

    Client_Lobby.GetPlace _20462 { get; }

    Extral.ReqAllLottery _20463 { get; }

    Extral.ReqOneGameLottery _20464 { get; }

    Extral.ChangeGameLottery _20465 { get; }

    Extral.ReqSlotCounter _20466 { get; }

    Extral.ReqSlotCounters _20467 { get; }

    List<Extral.ReqSlotCounter> _20468 { get; }

    Extral.AllLotteryRet _20469 { get; }

    List<Extral.LotteryInfo> _20470 { get; }

    Extral.LotteryInfo _20471 { get; }

    Extral.OneGameLotteryRet _20472 { get; }

    Extral.SlotCountRet _20473 { get; }

    Extral.SlotCountRets _20474 { get; }

    List<Extral.SlotCountRet> _20475 { get; }

    Empire88.Cell _20476 { get; }

    Empire88.Line _20477 { get; }

    List<Empire88.Cell> _20478 { get; }

    List<Empire88.Line> _20479 { get; }

    Empire88.OneLine _20480 { get; }

    List<Empire88.OneLine> _20481 { get; }

    EgyptianFantasy.Cell _20482 { get; }

    EgyptianFantasy.Line _20483 { get; }

    List<EgyptianFantasy.Cell> _20484 { get; }

    List<EgyptianFantasy.Line> _20485 { get; }

    DB_CatchFish.push_fish_dead_init_param _20486 { get; }

    DB_CatchFish.game_catchfish_getcost _20487 { get; }

    DB_CatchFish.game_catchfish_dead_param _20488 { get; }

    DB_CatchFish.push_game_level_cannon _20489 { get; }

    List<DB_CatchFish.marqueeconfig> _20490 { get; }

    DB_CatchFish.marqueeconfig _20491 { get; }

    DB_CatchFish.push_fish_dead_change_param _20492 { get; }

    DB_CatchFish.Notice _20493 { get; }

    DB_CatchFish.GetNotices _20494 { get; }

    List<DB_CatchFish.Notice> _20495 { get; }

    DB_CatchFish.update_getcost_success _20496 { get; }

    Client_CatchFish.Enter _20497 { get; }

    Client_CatchFish.Leave _20498 { get; }

    Client_CatchFish.Sync _20499 { get; }

    Client_CatchFish.BattleCmd _20500 { get; }

    Client_CatchFish.BulletPower _20501 { get; }

    Client_CatchFish.FireLock _20502 { get; }

    Client_CatchFish.Rocker _20503 { get; }

    Client_CatchFish.BackgroundMusic _20504 { get; }

    Client_CatchFish.SoundEffect _20505 { get; }

    Client_CatchFish.FireAim _20506 { get; }

    Client_CatchFish.FireUnlock _20507 { get; }

    Client_CatchFish.FireAutomating _20508 { get; }

    Client_CatchFish.FireUnAutomating _20509 { get; }

    Client_CatchFish.FireLightning _20510 { get; }

    Client_CatchFish.FireUnLightning _20511 { get; }

    Client_CatchFish.Fire _20512 { get; }

    Client_CatchFish.CannonSwitch _20513 { get; }

    Client_CatchFish.Hit _20514 { get; }

    Client_CatchFish.SendDrillBullet _20515 { get; }

    Client_CatchFish.PlayPing _20516 { get; }

    Client_CatchFish.ChangeNoticeReadState _20517 { get; }

    CatchFish_Lobby.RegisterCatchFishService _20518 { get; }

    CatchFish_Lobby.Register _20519 { get; }

    CatchFish_Lobby.EnterRet _20520 { get; }

    CatchFish_Lobby.Restore_Result _20521 { get; }

    CatchFish_Lobby.KillPlayerSuccess _20522 { get; }

    CatchFish_Lobby.ChangeNoticeReadState _20523 { get; }

    CatchFish_Lobby.PlayerMoneyRecord _20524 { get; }

    CatchFish_DB.Leave _20525 { get; }

    CatchFish_DB.update_get_cost _20526 { get; }

    CatchFish_DB.get_dead_param_get_cost _20527 { get; }

    CatchFish_DB.UpdateGiftMoney _20528 { get; }

    CatchFish_DB.GetAllServerNotice _20529 { get; }

    CatchFish_DB.log_game_catchfish_fish_statistic _20530 { get; }

    CatchFish_DB.catchfish_statistics _20531 { get; }

    List<CatchFish_DB.log_game_catchfish_fish_statistic> _20532 { get; }

    CatchFish_Client.Leave_Success _20533 { get; }

    CatchFish_Client.Enter_Success _20534 { get; }

    CatchFish.Scene _20535 { get; }

    CatchFish_Client.Player_Syc _20536 { get; }

    CatchFish_Client.FramePackages _20537 { get; }

    List<CatchFish.Emitter> _20538 { get; }

    CatchFish.Emitter _20539 { get; }

    List<CatchFish.Events.EventBase> _20540 { get; }

    CatchFish.Events.EventBase _20541 { get; }

    CatchFish_Client.OtherDeviceLogin _20542 { get; }

    CatchFish_Client.Notice _20543 { get; }

    CatchFish_Client.GetNotices _20544 { get; }

    List<CatchFish_Client.Notice> _20545 { get; }

    CatchFish2_Lobby.RegisterCatchFishService _20546 { get; }

    CatchFish2_Lobby.EnterRet _20547 { get; }

    CatchFish2_Lobby.Leave _20548 { get; }

    CatchFish2_Lobby.PlayerMoneyRecord _20549 { get; }

    CatchFish2_Lobby.ChangeNoticeReadState _20550 { get; }

    CatchFish2_Lobby.UpdateGiftMoney _20551 { get; }

    CatchFish2_Lobby.GetAllServerNotice _20552 { get; }

    CatchFish.Player _20553 { get; }

    CatchFish.PlayerBase _20554 { get; }

    List<CatchFish.Bullet> _20555 { get; }

    CatchFish.Bullet _20556 { get; }

    List<Client_CatchFish.BattleCmd> _20557 { get; }

    CatchFish.MoveObject _20558 { get; }

    CatchFish.DrillBullet _20559 { get; }

    CatchFish.Fish _20560 { get; }

    CatchFish.FishConfig _20561 { get; }

    CatchFish.FishLine _20562 { get; }

    CatchFish.Background_Config _20563 { get; }

    CatchFish.Background_KingCrab_Common_Config _20564 { get; }

    CatchFish.Background_KingCrab_Boss_Config _20565 { get; }

    List<float> _20566 { get; }

    CatchFish.Background_Crocodile_Common_Config _20567 { get; }

    CatchFish.Background_Crocodile_Boss_Config _20568 { get; }

    CatchFish.Background_NightTorches_Common_Config _20569 { get; }

    CatchFish.Background_NightTorches_Boss_Config _20570 { get; }

    CatchFish.Background_Octopus_Common_Config _20571 { get; }

    CatchFish.Background_Octopus_Boss_Config _20572 { get; }

    List<CatchFish.BornPoint> _20573 { get; }

    CatchFish.BornPoint _20574 { get; }

    CatchFish.Background _20575 { get; }

    CatchFish.Background_KingCrab_Common _20576 { get; }

    CatchFish.Background_KingCrab_Boss _20577 { get; }

    CatchFish.Background_Crocodile_Common _20578 { get; }

    CatchFish.Background_Crocodile_Boss _20579 { get; }

    CatchFish.Background_NightTorches_Common _20580 { get; }

    CatchFish.Background_NightTorches_Boss _20581 { get; }

    CatchFish.Background_Octopus_Common _20582 { get; }

    CatchFish.Background_Octopus_Boss _20583 { get; }

    CatchFish.Background_Common _20584 { get; }

    xx.Random _20585 { get; }

    List<CatchFish.Player> _20586 { get; }

    List<List<CatchFish.Fish>> _20587 { get; }

    List<CatchFish.Fish> _20588 { get; }

    List<CatchFish.Events.MakeFish> _20589 { get; }

    CatchFish.Events.MakeFish _20590 { get; }

    CatchFish.Stage _20591 { get; }

    List<CatchFish.Background> _20592 { get; }

    CatchFish.Fish_KingCrab _20593 { get; }

    CatchFish.Fish_Crocodile _20594 { get; }

    CatchFish.Fish_NightTorches _20595 { get; }

    CatchFish.Fish_Tyrannosaurus _20596 { get; }

    CatchFish.Fish_Sepia _20597 { get; }

    CatchFish.Fish_DragonTurtle _20598 { get; }

    CatchFish.Fish_BombCrab _20599 { get; }

    CatchFish.Fish_ContinuityBombCrab _20600 { get; }

    List<xx.Pos> _20601 { get; }

    CatchFish.Fish_Octopus _20602 { get; }

    CatchFish.Fish_GeneralLobster _20603 { get; }

    CatchFish.Fish_ElectroJelly _20604 { get; }

    CatchFish.Fish_Grail _20605 { get; }

    CatchFish.Fish_TigerHeadShark _20606 { get; }

    CatchFish.FishSpriteFrameBase _20607 { get; }

    CatchFish.FishSpriteFrame _20608 { get; }

    List<List<xx.Pos>> _20609 { get; }

    List<CatchFish.FishLinePoint> _20610 { get; }

    List<CatchFish.FishLine> _20611 { get; }

    List<CatchFish.FishSpriteFrame> _20612 { get; }

    List<CatchFish.FishSpriteFrameBase> _20613 { get; }

    CatchFish.Config _20614 { get; }

    List<CatchFish.FishConfig> _20615 { get; }

    List<List<CatchFish.FishLine>> _20616 { get; }

    List<CatchFish.Stage> _20617 { get; }

    List<CatchFish.Background_Config> _20618 { get; }

    List<string> _20619 { get; }

    CatchFish.FishConfig_KingCrab _20620 { get; }

    CatchFish.FishConfig_Crocodile _20621 { get; }

    CatchFish.FishConfig_DragonTurtle _20622 { get; }

    CatchFish.FishConfig_NightTorches _20623 { get; }

    CatchFish.FishConfig_BombCrab _20624 { get; }

    CatchFish.FishConfig_ContinuityBombCrab _20625 { get; }

    CatchFish.FishConfig_BlazeGale _20626 { get; }

    CatchFish.FishConfig_Octopus _20627 { get; }

    CatchFish.FishConfig_GeneralLobster _20628 { get; }

    CatchFish.FishConfig_ElectroJelly _20629 { get; }

    CatchFish.FishConfig_CycloneFish _20630 { get; }

    CatchFish.FishConfig_TigerHeadShark _20631 { get; }

    CatchFish.Emitter_Group _20632 { get; }

    CatchFish.Emitter_LineArray _20633 { get; }

    CatchFish.Emitter_Array _20634 { get; }

    CatchFish.Emitter_SpecialLineArray _20635 { get; }

    CatchFish.Emitter_FishMonitor _20636 { get; }

    CatchFish.Emitter_SpecialFish _20637 { get; }

    CatchFish.Emitter_KingCrab _20638 { get; }

    CatchFish.Emitter_Crocodile _20639 { get; }

    CatchFish.Emitter_NightTorches _20640 { get; }

    CatchFish.Emitter_Tyrannosaurus _20641 { get; }

    CatchFish.Emitter_Sepia _20642 { get; }

    CatchFish.Emitter_DragonTurtle _20643 { get; }

    CatchFish.Emitter_Octopus _20644 { get; }

    CatchFish.Emitter_GeneralLobster _20645 { get; }

    CatchFish.Emitter_ElectroJelly _20646 { get; }

    CatchFish.Emitter_CycloneFish _20647 { get; }

    CatchFish.Emitter_CycloneFish_Group _20648 { get; }

    CatchFish.Emitter_Fish_Grail _20649 { get; }

    CatchFish.Emitter_Effect_Escape _20650 { get; }

    CatchFish.Emitter_Effect_Warning _20651 { get; }

    CatchFish.Emitter_Effect_KingCrab_Coming _20652 { get; }

    CatchFish.Emitter_Effect_Crocodile_Coming _20653 { get; }

    CatchFish.Emitter_Effect_NightTorches_Coming _20654 { get; }

    CatchFish.Emitter_Effect_Octopus_Coming _20655 { get; }

    CatchFish.Emitter_Effect_Earthquake1 _20656 { get; }

    CatchFish.Emitter_Effect_BloodFlash _20657 { get; }

    CatchFish.Emitter_Effect_Bubbles _20658 { get; }

    CatchFish.Emitter_Effect_CommonBoss _20659 { get; }

    CatchFish.Emitter_Effect_PinkLight _20660 { get; }

    CatchFish.Emitter_Effect_Background_Normal_To_Abyss _20661 { get; }

    CatchFish.Emitter_Effect_Background_Abyss_To_Normal _20662 { get; }

    CatchFish.Events.PlayerLeave _20663 { get; }

    CatchFish.Events.PlayerEnter _20664 { get; }

    CatchFish.Events.FireLock _20665 { get; }

    CatchFish.Events.FireAim _20666 { get; }

    CatchFish.Events.FireUnlock _20667 { get; }

    CatchFish.Events.FireAutomating _20668 { get; }

    CatchFish.Events.FireUnAutomating _20669 { get; }

    CatchFish.Events.FireLightning _20670 { get; }

    CatchFish.Events.FireUnLightning _20671 { get; }

    CatchFish.Events.Fire _20672 { get; }

    CatchFish.Events.CannonSwitch _20673 { get; }

    CatchFish.Events.BulletHit _20674 { get; }

    CatchFish.Events.BulletPower _20675 { get; }

    CatchFish.Events.FishDead _20676 { get; }

    CatchFish.Events.SpecialFish_Dead _20677 { get; }

    List<CatchFish.Events.FishDead> _20678 { get; }

    CatchFish.Events.ChangeState_Special _20679 { get; }

    CatchFish.Events.ContinuityBombCrab_Boom _20680 { get; }

    CatchFish.Events.Clean_Fish _20681 { get; }

    CatchFish.Events.Clean_Bullet _20682 { get; }

    CatchFish.Events.SendDrillBullet _20683 { get; }

    CatchFish.Events.DrillBoom _20684 { get; }

    CatchFish.Events.FireStormClear _20685 { get; }

    CatchFish.Events.FishBorn _20686 { get; }

    CatchFish.Events.FishEvent _20687 { get; }

    CatchFish.Events.BankRuptcy _20688 { get; }

    CatchFish.Events.PlayerCharge _20689 { get; }

    AfricanBuffalo.Cell _20690 { get; }

    AfricanBuffalo.Line _20691 { get; }

    List<AfricanBuffalo.Cell> _20692 { get; }

    List<AfricanBuffalo.Line> _20693 { get; }

}
