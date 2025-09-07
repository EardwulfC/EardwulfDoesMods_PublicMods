namespace AsgardianAudioAdjuster;

using HarmonyLib;

using System.Linq;

using UnityEngine;

[HarmonyPatch(typeof(Player))]
internal class PlayerPatches
{
  [HarmonyPatch(nameof(Player.OnSpawned))]
  [HarmonyPostfix]

  public static void SfxPatchOnSpawned(Player __instance)
  {

    if (__instance != Player.m_localPlayer)
    {
      return;
    }
    UpdateAudioSources.UpdateAudioList();
  }
}