namespace ComfyMassFarming;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static PluginConfig;


internal class HelperFunctions
{
  public static bool IsPlacingPlant(Player player)
  {
    GameObject ghost = player.m_placementGhost;
    return ghost && ghost.GetComponent<Plant>();
  }

  public static bool ShouldTrackRotation(Player player)
  {
    if (isHotKeyPressed && IsPlacingPlant(player))
    {
      return true;
    }
    return false;
  }

  public static bool isHotKeyPressed = Input.GetKey(ControllerPickupHotkey.Value.MainKey) ||
   Input.GetKey(MassActionHotkey.Value.MainKey);

  public static int plantSpaceMask = LayerMask.GetMask("Default", "static_solid", "Default_small", "piece", "piece_nonsolid");
}
