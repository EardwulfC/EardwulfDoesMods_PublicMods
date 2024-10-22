namespace FightMeBro;

using BepInEx;

using HarmonyLib;

using System.Reflection;

using static PluginConfig;


[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class FightMeBro : BaseUnityPlugin
{
  public const string PluginGuid = "eardwulf.valheim.FightMeBro";
  public const string PluginName = "FightMeBro";
  public const string PluginVersion = "1.0.0";

  Harmony _harmony;

  void Awake()
  {
    BindConfig(Config);
    _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }
}