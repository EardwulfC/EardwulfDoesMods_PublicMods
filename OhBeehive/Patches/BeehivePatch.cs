namespace OhBeehive;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Beehive))]
static class BeehivePatch
{
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Beehive.Awake))]

  static void AwakePostFix(Beehive __instance)
  {
    if (IsModEnabled.Value) // && __instance.GetHoneyLevel() > 0)
    {
      __instance.gameObject.AddComponent<BeehiveMonitor>();
      HoneyExtractionManager.Instance.RegisterBeehive(__instance);
      OhBeehive._logger.LogInfo($"A Beehive has registered itself to the manager.");
    }
  }
}