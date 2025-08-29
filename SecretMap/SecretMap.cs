namespace SecretMap;

using BepInEx;
using HarmonyLib;
using System;
using System.Globalization;
using System.Reflection;
using static PluginConfig;


[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class SecretMap : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfC.valheim.secretmap";
  public const string PluginName = "SecretMap";
  public const string PluginVersion = "1.0.0";

  internal static BepInEx.Logging.ManualLogSource _logger;

  void Awake()
  {
    _logger = Logger;
    BindConfig(Config);
    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    _logger.LogInfo("Keeping your map secret since 2025.");
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
