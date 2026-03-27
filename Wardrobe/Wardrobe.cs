namespace Wardrobe;

using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Wardrobe : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfDoesMods.valheim.Wardrobe";
  public const string PluginName = "Wardrobe";
  public const string PluginVersion = "1.4.1";

  void Awake()
  {
    BindConfig(Config);
    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }
}
