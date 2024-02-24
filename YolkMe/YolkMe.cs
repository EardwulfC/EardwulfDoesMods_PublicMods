using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

using static YolkMe.PluginConfig;

namespace YolkMe
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class YolkMe : BaseUnityPlugin
    {
        public const string PluginGuid = "eardwulfdoesmods.valheim.modname";
        public const string PluginName = "YolkMe";
        public const string PluginVersion = "1.0.0";

        Harmony _harmony;

        void Awake()
        {
            BindConfig(Config);
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
        }


        [HarmonyPatch(typeof(EggGrow))]
        static class EggGrowPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(EggGrow.GrowUpdate))]
            static void GrowUpdatePostfix(EggGrow __instance)
            {
                if (__instance.m_nview && __instance.m_nview.IsValid())
                {
                    float growStart = __instance.m_nview.m_zdo.GetFloat(ZDOVars.s_growStart, 0f);
                    // growStart is reset to 0 during GrowUpdate() if CanGrow() returns false.
                    __instance.m_item.m_autoPickup = growStart <= 0f;
                }
            }
        }

        void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
