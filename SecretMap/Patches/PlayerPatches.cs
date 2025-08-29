namespace SecretMap;

using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static PluginConfig;

internal class PlayerPatches
{
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
          SecretMap.LogError("Could not find SharedPanel.  SecretMap will not run.");
          return;
        }
        Toggle cartographyTableToggle = sharedPanelTransform.GetComponentInChildren<Toggle>();

        if (cartographyTableToggle == null)
        {
          SecretMap.LogError("Could not find Toggle component of SharedPanel.  SecretMap will not run.");
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
