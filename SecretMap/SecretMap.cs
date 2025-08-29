namespace SecretMap;

using System.Reflection;

using BepInEx;

using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using static PluginConfig;


[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class SecretMap : BaseUnityPlugin
{
  public const string PluginGuid = "EardwulfC.valheim.secretmap";
  public const string PluginName = "SecretMap";
  public const string PluginVersion = "1.0.0";

  Harmony _harmony;

  internal static BepInEx.Logging.ManualLogSource _logger;

  void Awake()
  {
    _logger = Logger;
    BindConfig(Config);
    _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    _logger.LogInfo("Keeping your map secret since 2025.");
  }

  [HarmonyPatch(typeof(Player))]
  static class PlayerOnSpawnedPatch
  {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.OnSpawned))]
    static void OnSpawnedPostFix()
    {
      if (IsModEnabled.Value)
      {
        Transform sharedPanelTransform = Minimap.m_instance.m_largeRoot.transform.Find("SharedPanel");

        if (sharedPanelTransform == null)
        {
          _logger.LogError("Could not find SharedPanel.  SecretMap will not run.");
          return;
        }
        Toggle cartographyTableToggle = sharedPanelTransform.GetComponentInChildren<Toggle>();

        if (cartographyTableToggle == null)
        {
          _logger.LogError("Could not find Toggle component of SharedPanel.  SecretMap will not run.");
          return;
        }

        if (cartographyTableToggle.isOn == true)
        {
          cartographyTableToggle.isOn = false;
          sharedPanelTransform.gameObject.SetActive(true);
        }
      }
    }
  }
}
