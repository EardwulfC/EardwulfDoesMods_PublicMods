namespace OhBeehive;

using System.Collections.Generic;
using System.Reflection;

using BepInEx;

using HarmonyLib;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class OhBeehive : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfDoesMods.valheim.OhBeehive";
  public const string PluginName = "OhBeehive";
  public const string PluginVersion = "1.0.0";

  Harmony _harmony;

  public void Awake()
  {
    BindConfig(Config);
    _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }
}


