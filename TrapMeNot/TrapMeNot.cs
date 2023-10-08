using BepInEx;

using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using static Interpolate;

namespace TrapMeNot
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class TrapMeNot : BaseUnityPlugin
    {
        public const string PluginGuid = "Eardwulf.TrapMeNot";
        public const string PluginName = "TrapMeNot";
        public const string PluginVersion = "1.1.1";

        Harmony _harmony;

        public void Awake()
        {
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
        }

        [HarmonyPatch(typeof(Trap))]
        static class TrapPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Trap.OnTriggerEnter))]
            static bool OnTriggerEnterPrefix(ref Trap __instance, ref Collider collider)
            {
                if (__instance.m_triggeredByPlayers)
                {
                    Player player = collider.GetComponentInParent<Player>();

                    if (player && !player.IsPVPEnabled())
                    {

                        bool IsComfyDeepNorth = TrapMeNot.IsComfyWorld() && Heightmap.FindBiome(player.transform.position) == Heightmap.Biome.DeepNorth;
                        bool IsComfyAshlands = TrapMeNot.IsComfyWorld() && Heightmap.FindBiome(player.transform.position) == Heightmap.Biome.AshLands;
                        /*bool IsComfySpaceBiome = TrapMeNot.IsComfyWorld() && TrapMeNot.IsInSpace();*/

                        if (IsComfyDeepNorth || IsComfyAshlands /*|| IsComfySpaceBiome*/)
                        {
                            return true;
                        }
                        return false;
                    }
                }

                return true;
            }
        }
        public void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        public static bool IsComfyWorld()
        {
            return
                ZNet.m_instance
                && ZNet.m_world != null
                && ZNet.m_world.m_name.StartsWith("ComfyEra");
        }

        /*public static bool IsInSpace()
        {
            return
            Utils.DistanceXZ(Player.m_localPlayer.transform.position, Vector3.zero) > 10500;
        }*/
    }
}