using HarmonyLib;

using UnityEngine;

using static YolkMe.PluginConfig;

namespace YolkMe {
  [HarmonyPatch(typeof(Humanoid))]
  static class HumanoidPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Humanoid.Pickup))]
    static void PickupPostfix(Humanoid __instance, GameObject go, bool __result) {
      if (IsModEnabled.Value
          && __result
          && __instance == Player.m_localPlayer
          && go.TryGetComponent(out ItemDrop itemDrop)
          && EggPickupController.IsEgg(itemDrop)) {
        EggPickupController.LastEggPickupTime = Time.time;
      }
    }
  }
}
