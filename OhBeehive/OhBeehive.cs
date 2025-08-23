namespace OhBeehive;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using System;
using System.Reflection;
using System.Globalization;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class OhBeehive : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfDoesMods.valheim.OhBeehive";
  public const string PluginName = "OhBeehive";
  public const string PluginVersion = "1.0.1";

  public static ManualLogSource _logger;

  public void Awake()
  {
    _logger = Logger;
    BindConfig(Config);
    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  public static void LogInfo(object obj)
  {
    _logger.LogInfo($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }

  public static void LogError(object obj)
  {
    _logger.LogError($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }

}


