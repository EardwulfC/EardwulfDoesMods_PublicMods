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

        //[HarmonyPatch(typeof(ObjectDB))]

        //public static class Patch_ObjectDB_Awake
        //{
        //    [HarmonyPostfix]
        //    [HarmonyPatch(nameof(ObjectDB.Awake))]

        //    public static void PostfixEgg(ObjectDB __instance)
        //    {
        //        ItemEgg(__instance, "ChickenEgg");
        //    }

        //    static void ItemEgg(ObjectDB __instance, string prefab)
        //    {
        //        GameObject egg = __instance.GetItemPrefab(prefab);

        //        if (!egg)
        //        {
        //            return;
        //        }

        //        var GetItemData = egg.GetComponent<ItemDrop>();

        //        if (!GetItemData)
        //        {
        //            return;
        //        }

        //        if(!IsModEnabled.Value)
        //        { 
        //            return;
        //        }

        //        ZLog.Log("Adding auto pickup to eggs.");

        //        GetItemData.m_autoPickup = true;
        //    }
        //}

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
