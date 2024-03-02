using System.Reflection;

using BepInEx;

using HarmonyLib;

using static YolkMe.PluginConfig;

namespace YolkMe {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public sealed class YolkMe : BaseUnityPlugin {
    public const string PluginGuid = "EardwulfC.valheim.YolkMe";
    public const string PluginName = "YolkMe";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    void Awake() {
      BindConfig(Config);

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}
