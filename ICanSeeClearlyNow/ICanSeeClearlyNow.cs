using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

using static ICanSeeClearlyNow.PluginConfig;

namespace ICanSeeClearlyNow {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public sealed class ICanSeeClearlyNow : BaseUnityPlugin {
    public const string PluginGuid = "comfy.valheim.ICanSeeClearlyNow";
    public const string PluginName = "ICanSeeClearlyNow";
    public const string PluginVersion = "0.0.2";

    Harmony _harmony;

    void Awake() {
      BindConfig(Config);
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    [HarmonyPatch]

    public class AmbientLightPatch {

      static Color originalColor;

      [HarmonyPrefix]
      [HarmonyPatch(typeof(RenderSettings), "set_ambientLight")]

      static void SetLightPrefix(ref Color value) {
        if (!IsModEnabled.Value) {
          return;
        }

        originalColor = value;

        value = LightenColor(value);
      }

      [HarmonyPostfix]
      [HarmonyPatch(typeof(RenderSettings), "get_ambientLight")]
      static void GetLight(ref Color __result) {
        if (!IsModEnabled.Value) {
          return;
        }
        __result = originalColor;
      }

      static Color LightenColor(Color sourceColor) {

        float avgColor = (sourceColor.r + sourceColor.g + sourceColor.b) / 3;

        if (GammaValue.Value > avgColor) {
          sourceColor *= GammaValue.Value / avgColor;
        }
        return sourceColor;
      }
    }

    void OnDestroy()
    {
      _harmony?.UnpatchSelf();
    }
  }
}
