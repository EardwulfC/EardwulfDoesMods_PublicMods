namespace ComfyMassFarming;

using BepInEx;

using HarmonyLib;

using System.Reflection;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class MassFarming : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfDoesMods.Comfy.MassFarming";
  public const string PluginName = "Comfy.MassFarming";
  public const string PluginVersion = "1.6.1";

  public void Awake()
  {
    BindConfig(Config);
    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }
}