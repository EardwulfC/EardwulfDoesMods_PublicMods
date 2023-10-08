using BepInEx;
using HarmonyLib;
using System.Reflection;

namespace EardwulfDoesMods_PublicMods
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class EardwulfDoesMods_PublicMods : BaseUnityPlugin
    {
        public const string PluginGuid = "comfy.valheim.modname";
        public const string PluginName = "EardwulfDoesMods_PublicMods";
        public const string PluginVersion = "1.0.0";

        Harmony _harmony;

        void Awake()
        {
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
        }

        void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
