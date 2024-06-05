using HarmonyLib;

using UnityEngine;

using static ICanSeeClearlyNow.PluginConfig;

namespace ICanSeeClearlyNow
{
  [HarmonyPatch]

  public class AmbientLightPatch
  {

    static Color originalColor;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RenderSettings), "set_ambientLight")]
    static void SetLightPrefix(ref Color value)
    {
      if (!IsModEnabled.Value || EnvMan.instance == null)
      {
        return;
      }

      originalColor = value;

      value = LightenColor(value);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RenderSettings), "get_ambientLight")]
    static void GetLight(ref Color __result)
    {
      if (!IsModEnabled.Value)
      {
        return;
      }
      __result = originalColor;
    }

    static Color LightenColor(Color sourceColor)
    {
      float avgColor = (sourceColor.r + sourceColor.g + sourceColor.b) / 3;
      bool isNight;
      bool IsAshlandsOn = EnvMan.instance.GetBiome() == Heightmap.Biome.AshLands && IsAshlandsAlwaysOn.Value;

      if (IsCustomTimer.Value)
      {
        float timeofday = EnvMan.instance.GetDayFraction();
        float icn = IsCustomNight.Value / 24f;
        float icd = IsCustomDay.Value / 24f;

        isNight = (timeofday >= icn && IsCustomTimer.Value)
               || (timeofday <= icd && IsCustomTimer.Value)
               || EnvMan.instance.GetCurrentEnvironment().m_alwaysDark
               || EnvMan.IsNight()
               || IsAshlandsOn;

        if (isNight)
        {
          sourceColor *= GammaValue.Value / avgColor;
        }
      }

      if (!IsCustomTimer.Value)
      {
        if (EnvMan.IsNight() || EnvMan.instance.GetCurrentEnvironment().m_alwaysDark || IsAshlandsOn)
        {
          sourceColor *= GammaValue.Value / avgColor;
        }
      }
      return sourceColor;
    }
  }
}
