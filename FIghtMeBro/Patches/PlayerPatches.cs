namespace FightMeBro;


using HarmonyLib;

using static PluginConfig;

internal class PlayerPatches
{
  [HarmonyPatch(typeof(Player))]

  static class PlayerPvPPatch
  {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.OnSpawned))]

    static void PVPUpdaterPostfix(Player __instance)
    {
      if (!Player.m_localPlayer || !IsModEnabled.Value)
      {
        return;
      }
      __instance.SetPVP(true);
      Chat.m_instance.AddString("<color=red>You are now flagged for PVP!</color>");
    }
  }
}
