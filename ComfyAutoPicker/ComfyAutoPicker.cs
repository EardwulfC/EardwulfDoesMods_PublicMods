namespace ComfyAutoPicker;

using System.Collections.Generic;
using System.Reflection;

using BepInEx;

using HarmonyLib;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Mod : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfDoesMods.valheim.ComfyAutoPicker";
  public const string PluginName = "ComfyAutoPicker";
  public const string PluginVersion = "1.5.0";

  public static readonly HashSet<int> PlantsToAutoPick = new() {
            "CloudberryBush".GetStableHashCode(),
            "Pickable_Barley".GetStableHashCode(),
            "Pickable_Barley_Wild".GetStableHashCode(),
            "Pickable_Flax".GetStableHashCode(),
            "Pickable_Flax_Wild".GetStableHashCode(),
            "Pickable_Carrot".GetStableHashCode(),
            "Pickable_SeedCarrot".GetStableHashCode(),
            "Pickable_Turnip".GetStableHashCode(),
            "Pickable_SeedTurnip".GetStableHashCode(),
            "Pickable_Onion".GetStableHashCode(),
            "Pickable_SeedOnion".GetStableHashCode(),
            "Pickable_Mushroom_JotunPuffs".GetStableHashCode(),
            "Pickable_Mushroom_Magecap".GetStableHashCode(),
            "VineAsh".GetHashCode(),
            "Pickable_Thistle".GetStableHashCode(),
            "LuredWisp".GetStableHashCode(),
            "Pickable_Fiddlehead".GetStableHashCode()
        };

  void Awake()
  {
    BindConfig(Config);
    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }
}