namespace ComfyMassFarming;

using HarmonyLib;

using Splatform;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.Rendering;


[HarmonyPatch]
public class PlayerPatches
{
  [HarmonyPrefix]
  [HarmonyPatch(typeof(Player), "TryPlacePiece")]
  public static void PlacePiecePrefix(Player __instance, int ___m_placeRotation)
  {
    if (HelperFunctions.ShouldTrackRotation(__instance) && ComfyMassPlant.massFarmingRotation is null)
    {
      ComfyMassPlant.massFarmingRotation = ___m_placeRotation;
    }
  }

  [HarmonyPostfix]
  [HarmonyPatch(typeof(Player), "TryPlacePiece")]
  public static void PlacePiecePostfix(Player __instance, ref bool __result, Piece piece)
  {
    ComfyMassPlant.placeSuccessful = __result;
    if (__result)
    {
      GameObject placeGhost = __instance.m_placementGhost;
      ComfyMassPlant.placedPosition = placeGhost.transform.position;
      ComfyMassPlant.placedRotation = placeGhost.transform.rotation;
      ComfyMassPlant.placedPiece = piece;
    }
  }
}
