namespace AsgardianAudioAdjuster;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using System;
using System.Globalization;
using System.Reflection;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class AsgardianAudioAdjuster : BaseUnityPlugin
{
  public const string PluginGuid = "eardwulfc.comfy.valheim.asgardianaudioadjuster";
  public const string PluginName = "AsgardianAudioAdjuster";
  public const string PluginVersion = "1.0.0";

  static ManualLogSource _logger;

  void Awake()
  {
    BindConfig(Config);
    _logger = Logger;
    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    _logger.LogInfo("Your Asgardian Audio Adjuster is on the job!");
    UpdateSoundMuterList();
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

