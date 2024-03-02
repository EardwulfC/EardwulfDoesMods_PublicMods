using HarmonyLib;

using static YolkMe.PluginConfig;

namespace YolkMe {
  [HarmonyPatch(typeof(EggGrow))]
  static class EggGrowPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(EggGrow.GrowUpdate))]
    static void GrowUpdatePostfix(EggGrow __instance) {
      if (IsModEnabled.Value && __instance.m_nview && __instance.m_nview.IsValid()) {
        float growStart = __instance.m_nview.m_zdo.GetFloat(ZDOVars.s_growStart, 0f);
        // growStart is reset to 0 during GrowUpdate() if CanGrow() returns false.
        __instance.m_item.m_autoPickup = growStart <= 0f;
      }
    }
  }
}
