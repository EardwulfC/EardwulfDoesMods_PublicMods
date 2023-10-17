using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace YolkMe
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class YolkMe : BaseUnityPlugin
    {
        public const string PluginGuid = "comfy.valheim.modname";
        public const string PluginName = "YolkMe";
        public const string PluginVersion = "1.0.0";

        Harmony _harmony;

        void Awake()
        {
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
        }

        [HarmonyPatch(typeof(ObjectDB))]

        public static class Patch_ObjectDB_Awake
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(ObjectDB.Awake))]

            public static void PostfixEgg(ObjectDB __instance)
            {
                ItemEgg(__instance, "ChickenEgg");
            }

            static void ItemEgg(ObjectDB __instance, string prefab)
            {
                GameObject egg = __instance.GetItemPrefab(prefab);

                if (!egg || !Player.m_localPlayer)
                {
                    return;
                }

                var GetItemData = egg.GetComponent<ItemDrop>();
                if (!GetItemData)
                {
                    return;
                }

                
                GetItemData.m_autoPickup = true;
            }

        }

        void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
