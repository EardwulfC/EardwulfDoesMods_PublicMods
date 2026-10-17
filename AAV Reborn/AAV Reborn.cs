namespace AAV_Reborn;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using System;
using System.Globalization;
using System.Reflection;

using static PluginConfig;


[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class AAV_Reborn : BaseUnityPlugin
{
  public const string PluginGuid = "Eardwulf.valheim.aavreborn";
  public const string PluginName = "AAV_Reborn";
  public const string PluginVersion = "1.0.0";

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
}
