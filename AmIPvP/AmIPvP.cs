namespace AmIPvP;

using BepInEx;
using HarmonyLib;
using System.Reflection;


[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class AmIPvP : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfC.valheim.AmIPvP";
  public const string PluginName = "AmIPvP";
  public const string PluginVersion = "1.0.0";

  public static bool IsMyPvPEnabled = false;
  public static int hashPvPOff;
  public static int hashPvPOn;

  void Awake()
  {
    hashPvPOff = "AmIPvP_PvPOff".GetStableHashCode();
    hashPvPOn = "AmIPvP_PvPOn".GetStableHashCode();

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }
}