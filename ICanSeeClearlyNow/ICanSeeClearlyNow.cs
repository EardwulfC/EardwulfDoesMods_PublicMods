using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

using static ICanSeeClearlyNow.PluginConfig;

namespace ICanSeeClearlyNow {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public sealed class ICanSeeClearlyNow : BaseUnityPlugin {
    public const string PluginGuid = "EardwulfC.valheim.ICanSeeClearlyNow";
    public const string PluginName = "ICanSeeClearlyNow";
    public const string PluginVersion = "1.1.3";

    Harmony _harmony;

    void Awake() {
      BindConfig(Config);
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    void OnDestroy()
    {
      _harmony?.UnpatchSelf();
    }
  }
}
