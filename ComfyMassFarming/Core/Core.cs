namespace ComfyMassFarming;

using HarmonyLib;

using Splatform;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.Rendering;

using static PluginConfig;

[HarmonyPatch]
public class ComfyMassPlant
{
  public static Vector3 placedPosition;
  public static Quaternion placedRotation;
  public static Piece placedPiece;
  public static bool placeSuccessful = false;
  public static Nullable<int> massFarmingRotation = null;

  [HarmonyPrefix]
  [HarmonyPatch(typeof(Player), "UpdatePlacement")]
  public static void UpdatePlacementPrefix(Player __instance, bool takeInput, float dt, ref int ___m_placeRotation)
  {
    placeSuccessful = false;

    if (HelperFunctions.ShouldTrackRotation(__instance) && massFarmingRotation.HasValue)
    {
      ___m_placeRotation = massFarmingRotation.Value;
    }
  }

  [HarmonyPostfix]
  [HarmonyPatch(typeof(Player), "UpdatePlacement")]
  public static void UpdatePlacementPostfix(Player __instance, bool takeInput, float dt, int ___m_placeRotation)
  {
    if (HelperFunctions.ShouldTrackRotation(__instance))
    {
      massFarmingRotation = ___m_placeRotation;
    }

    if (!placeSuccessful)
    {
      return;
    }

    Plant plant = placedPiece.gameObject.GetComponent<Plant>();
    if (!plant)
    {
      return;
    }

    Heightmap heightmap = Heightmap.FindHeightmap(placedPosition);
    if (!heightmap)
    {
      return;
    }

    if (!NoActionKeyNeeded.Value && !Input.GetKey(ControllerPickupHotkey.Value.MainKey)
            && !Input.GetKey(MassActionHotkey.Value.MainKey))
    {
      return;
    }

    foreach (Vector3 newPos in BuildPlantingGridPositions(placedPosition, plant, placedRotation))
    {
      if (placedPiece.m_cultivatedGroundOnly && !heightmap.IsCultivated(newPos))
      {
        continue;
      }

      if (placedPosition == newPos)
      {
        continue;
      }

      ItemDrop.ItemData tool = __instance.GetRightItem() as ItemDrop.ItemData;

      bool hasStamina = __instance.HaveStamina((tool.m_shared.m_attack.m_attackStamina + __instance.GetEquipmentHomeItemModifier()) * 1.7695f);

      if (!hasStamina)
      {
        Hud.instance.StaminaBarUppgradeFlash();
        return;
      }

      bool hasMats = __instance.m_noPlacementCost ||
        __instance.HaveRequirements(placedPiece, Player.RequirementMode.CanBuild);
      if (!hasMats)
      {
        return;
      }

      if (!HasGrowSpace(newPos, placedPiece.gameObject))
      {
        continue;
      }

      GameObject newPlaceObj = UnityEngine.Object.Instantiate(placedPiece.gameObject, newPos, placedRotation);
      Piece component = newPlaceObj.GetComponent<Piece>();
      if (component)
      {
        component.SetCreator(__instance.GetPlayerID(), PlatformManager.DistributionPlatform.LocalUser.PlatformUserID);
      }
      placedPiece.m_placeEffect.Create(newPos, placedRotation, newPlaceObj.transform);
      Game.instance.IncrementPlayerStat(PlayerStatType.Builds);
      __instance.RaiseSkill(Skills.SkillType.Farming);

      __instance.ConsumeResources(placedPiece.m_resources, 0, -1);

      __instance.UseStamina((tool.m_shared.m_attack.m_attackStamina + __instance.GetEquipmentHomeItemModifier()) * 1.7695f);

      tool.m_durability -= tool.m_shared.m_useDurabilityDrain;
      if (tool.m_durability <= 0f)
      {
        return;
      }
    }
  }

  private static List<Vector3> BuildPlantingGridPositions(Vector3 originPos, Plant placedPlant, Quaternion rotation)
  {
    float plantRadius = placedPlant.m_growRadius * 2;

    List<Vector3> gridPositions = new List<Vector3>(PlantGridWidth.Value * PlantGridLength.Value);
    Vector3 left = rotation * Vector3.left * plantRadius;
    Vector3 forward = rotation * Vector3.forward * plantRadius;
    Vector3 gridOrigin = originPos;
    if (GridAnchorLength.Value)
    {
      gridOrigin -= forward * (PlantGridLength.Value / 2);
    }
    if (GridAnchorWidth.Value)
    {
      gridOrigin -= left * (PlantGridWidth.Value / 2);
    }

    Vector3 newPos;
    for (int x = 0; x < PlantGridLength.Value; x++)
    {
      newPos = gridOrigin;
      for (int z = 0; z < PlantGridWidth.Value; z++)
      {
        newPos.y = ZoneSystem.instance.GetGroundHeight(newPos);
        gridPositions.Add(newPos);
        newPos += left;
      }
      gridOrigin += forward;
    }
    return gridPositions;
  }

  private static bool HasGrowSpace(Vector3 newPos, GameObject go)
  {
    if (go.GetComponent<Plant>() is Plant placingPlant)
    {
      Collider[] nearbyObjects = Physics.OverlapSphere(newPos, placingPlant.m_growRadius, HelperFunctions.plantSpaceMask);
      return nearbyObjects.Length == 0;
    }
    return true;
  }

  private static GameObject[] _placementGhosts = new GameObject[1];
  private static Piece _fakeResourcePiece;

  [HarmonyPrefix]
  [HarmonyPatch(typeof(Player), "SetupPlacementGhost")]
  public static void SetupPlacementGhostPrefix(Player __instance, int ___m_placeRotation)
  {
    if (HelperFunctions.ShouldTrackRotation(__instance) && massFarmingRotation is null)
    {
      massFarmingRotation = ___m_placeRotation;
    }
  }

  [HarmonyPostfix]
  [HarmonyPatch(typeof(Player), "SetupPlacementGhost")]
  public static void SetupPlacementGhostPostfix(Player __instance, ref int ___m_placeRotation)
  {
    if (HelperFunctions.ShouldTrackRotation(__instance) && massFarmingRotation.HasValue)
    {
      ___m_placeRotation = massFarmingRotation.Value;
    }
    DestroyGhosts();
  }

  [HarmonyPostfix]
  [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
  public static void UpdatePlacementGhostPostfix(Player __instance, bool flashGuardStone)
  {
    GameObject ghost = __instance.m_placementGhost;
    if (!ghost || !ghost.activeSelf)
    {
      SetGhostsActive(false);
      return;
    }

    if (!NoActionKeyNeeded.Value && !Input.GetKey(ControllerPickupHotkey.Value.MainKey)
           && !Input.GetKey(MassActionHotkey.Value.MainKey))
    {
      SetGhostsActive(false);
      return;
    }

    Plant plant = ghost.GetComponent<Plant>();
    if (!plant)
    {
      SetGhostsActive(false);
      return;
    }

    if (!EnsureGhostsBuilt(__instance))
    {
      SetGhostsActive(false);
      return;
    }

    //Find the required resource to plant the item
    //Assuming that for plants there is only a single resource requirement...
    Piece.Requirement requirement = ghost.GetComponent<Piece>().m_resources.FirstOrDefault(r => r.m_resItem && r.m_amount > 0);
    _fakeResourcePiece.m_resources[0].m_resItem = requirement.m_resItem;
    _fakeResourcePiece.m_resources[0].m_amount = requirement.m_amount;

    float currentStamina = __instance.GetStamina();
    ItemDrop.ItemData tool = __instance.GetRightItem() as ItemDrop.ItemData;
    Heightmap heightmap = Heightmap.FindHeightmap(ghost.transform.position);
    List<Vector3> positions = BuildPlantingGridPositions(ghost.transform.position, plant, ghost.transform.rotation);
    for (int i = 0; i < _placementGhosts.Length; i++)
    {
      Vector3 newPos = positions[i];
      if (ghost.transform.position == newPos)
      {
        //Trying to place around the origin point, so avoid placing a duplicate at the same location
        _placementGhosts[i].SetActive(false);
        continue;
      }

      //Track total cost of each placement
      _fakeResourcePiece.m_resources[0].m_amount += requirement.m_amount;

      _placementGhosts[i].transform.position = newPos;
      _placementGhosts[i].transform.rotation = ghost.transform.rotation;
      _placementGhosts[i].SetActive(true);

      bool invalid = false;
      if (ghost.GetComponent<Piece>().m_cultivatedGroundOnly && !heightmap.IsCultivated(newPos))
      {
        invalid = true;
      }
      else if (!HasGrowSpace(newPos, ghost.gameObject))
      {
        invalid = true;
      }
      else if (currentStamina < tool.m_shared.m_attack.m_attackStamina * 1.7999f)
      {
        Hud.instance.StaminaBarUppgradeFlash();
        invalid = true;
      }
      else if (!__instance.m_noPlacementCost &&
          !__instance.HaveRequirements(_fakeResourcePiece, Player.RequirementMode.CanBuild))
      {
        invalid = true;
      }
      currentStamina -= tool.m_shared.m_attack.m_attackStamina * 1.7999f;

      _placementGhosts[i].GetComponent<Piece>().SetInvalidPlacementHeightlight(invalid);
    }
  }

  private static bool EnsureGhostsBuilt(Player player)
  {
    int requiredSize = PlantGridWidth.Value * PlantGridLength.Value;
    bool needsRebuild = !_placementGhosts[0] || _placementGhosts.Length != requiredSize;
    if (needsRebuild)
    {
      DestroyGhosts();

      if (_placementGhosts.Length != requiredSize)
      {
        _placementGhosts = new GameObject[requiredSize];
      }

      if (player.m_buildPieces is PieceTable pieceTable && pieceTable.GetSelectedPrefab() is GameObject prefab)
      {
        if (prefab.GetComponent<Piece>().m_repairPiece)
        {
          //Repair piece doesn't have ghost
          return false;
        }

        for (int i = 0; i < _placementGhosts.Length; i++)
        {
          _placementGhosts[i] = SetupMyGhost(player, prefab);
        }
      }
      else
      {
        //No prefab, so don't need ghost (this probably shouldn't ever happen)
        return false;
      }

      if (!_fakeResourcePiece)
      {
        _fakeResourcePiece = _placementGhosts[0].GetComponent<Piece>();
        _fakeResourcePiece.m_dlc = string.Empty;
        _fakeResourcePiece.m_resources = new Piece.Requirement[]
        {
          new Piece.Requirement()
        };
      }

    }

    return true;
  }

  private static void DestroyGhosts()
  {
    for (int i = 0; i < _placementGhosts.Length; i++)
    {
      if (_placementGhosts[i])
      {
        UnityEngine.Object.Destroy(_placementGhosts[i]);
        _placementGhosts[i] = null;
      }
    }
    _fakeResourcePiece = null;
  }

  private static void SetGhostsActive(bool active)
  {
    foreach (GameObject ghost in _placementGhosts)
    {
      ghost?.SetActive(active);
    }
  }

  private static GameObject SetupMyGhost(Player player, GameObject prefab)
  {
    //This takes some shortcuts because it's only ever used for Plant pieces

    ZNetView.m_forceDisableInit = true;
    GameObject newGhost = UnityEngine.Object.Instantiate(prefab);
    ZNetView.m_forceDisableInit = false;
    newGhost.name = prefab.name;

    foreach (Joint joint in newGhost.GetComponentsInChildren<Joint>())
    {
      UnityEngine.Object.Destroy(joint);
    }

    foreach (Rigidbody rigidBody in newGhost.GetComponentsInChildren<Rigidbody>())
    {
      UnityEngine.Object.Destroy(rigidBody);
    }

    int layer = LayerMask.NameToLayer("ghost");
    foreach (Transform childTransform in newGhost.GetComponentsInChildren<Transform>())
    {
      childTransform.gameObject.layer = layer;
    }

    foreach (TerrainModifier terrainModifier in newGhost.GetComponentsInChildren<TerrainModifier>())
    {
      UnityEngine.Object.Destroy(terrainModifier);
    }

    foreach (GuidePoint guidepoint in newGhost.GetComponentsInChildren<GuidePoint>())
    {
      UnityEngine.Object.Destroy(guidepoint);
    }

    foreach (Light light in newGhost.GetComponentsInChildren<Light>())
    {
      UnityEngine.Object.Destroy(light);
    }

    Transform ghostOnlyTransform = newGhost.transform.Find("_GhostOnly");
    if ((bool)ghostOnlyTransform)
    {
      ghostOnlyTransform.gameObject.SetActive(value: true);
    }

    foreach (MeshRenderer meshRenderer in newGhost.GetComponentsInChildren<MeshRenderer>())
    {
      if (!(meshRenderer.sharedMaterial == null))
      {
        Material[] sharedMaterials = meshRenderer.sharedMaterials;
        for (int j = 0; j < sharedMaterials.Length; j++)
        {
          Material material = new Material(sharedMaterials[j]);
          material.SetFloat("_RippleDistance", 0f);
          material.SetFloat("_ValueNoise", 0f);
          sharedMaterials[j] = material;
        }
        meshRenderer.sharedMaterials = sharedMaterials;
        meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
      }
    }

    return newGhost;
  }
}

