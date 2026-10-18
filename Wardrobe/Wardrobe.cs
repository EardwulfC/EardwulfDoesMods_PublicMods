namespace Wardrobe;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using System;
using System.Globalization;
using System.Reflection;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Wardrobe : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfDoesMods.valheim.Wardrobe";
  public const string PluginName = "Wardrobe";
  public const string PluginVersion = "1.5.1";

  static ManualLogSource _logger;

  void Awake()
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
