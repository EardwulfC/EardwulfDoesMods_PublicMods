using HarmonyLib;

using static ComfyAutoPicker.PluginConfig;

namespace ComfyAutoPicker {
  [HarmonyPatch(typeof(Pickable))]
  static class PickablePatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Pickable.Awake))]
    static void AwakePostfix(Pickable __instance) {
      // Do check here to see if pickable is in the HashSet                    
      if (IsModEnabled.Value && Mod.PlantsToAutoPick.Contains(__instance.m_nview.m_zdo.m_prefab) && !__instance.m_picked) {

        __instance.gameObject.AddComponent<AutoPicker>();
      }
    }
  }
}
