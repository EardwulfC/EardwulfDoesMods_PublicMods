namespace Wardrobe;

using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using UnityEngine;
using static PluginConfig;


[HarmonyPatch(typeof(ArmorStand))]

static class ArmorStandSwapPatch
{
  public static readonly int ArmorStandHashCode = "ArmorStand".GetStableHashCode();

  [HarmonyPrefix]
  [HarmonyPatch(nameof(ArmorStand.UseItem))]

  static bool ArmorStandSwapGear(ArmorStand __instance, Humanoid user)
  {
    if (!QuickSwapKey.Value.IsDown() || !IsModEnabled.Value)
    {
      return true;
    }

    if (!PrivateArea.CheckAccess(__instance.transform.position, 0f, true, false))
    {
      ZLog.Log("You are not on the ward this armor stand.");
      return false;
    }

    if (__instance.m_nview && __instance.m_nview.m_zdo.m_prefab == ArmorStandHashCode)
    {

      List<ItemDrop.ItemData> itemList = ArmorStandEquip(__instance);
      PlayerEquipmentToArmorStand(__instance, user);
      ArmorStandEquipmentToPlayer(itemList, user);
      ZLog.Log($"Swapping Gear with Armor Stand!");
      Chat.m_instance.AddString("Swapping gear with ArmorStand");
      return false;
    }
    else
    {
      Chat.m_instance.AddString("<color=red>Failed to swap armor because this is not an armor stand.</color>");
      ZLog.Log("ArmorStandSwapGear() failed beacuse the target is not an ArmorStand.");
      return false;
    }
  }

  public static List<ItemDrop.ItemData> ArmorStandEquip(ArmorStand stand)
  {
    List<ItemDrop.ItemData> itemlist = new List<ItemDrop.ItemData>();
    for (int i = 0; i < stand.m_slots.Count; i++)
    {
      if (stand.HaveAttachment(i))
      {
        // The ArmorStand stores each slot as the stable hash of the item's prefab name.
        GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(stand.GetAttachedItem(i));
        if (!itemPrefab)
        {
          ZLog.LogWarning($"Wardrobe could not find the item prefab in ArmorStand slot {i}, leaving it on the stand.");
          continue;
        }
        // Load into a copy, so the stand's quality and crafter don't overwrite the shared prefab data.
        ItemDrop.ItemData itemData = itemPrefab.GetComponent<ItemDrop>().m_itemData.Clone();
        itemData.m_dropPrefab = itemPrefab;
        ItemDrop.LoadFromZDO(itemData, stand.m_nview.GetZDO(), i);
        itemData.m_equipped = false;
        itemlist.Add(itemData);
        ZLog.Log($"Wardrobe says I removed item {itemPrefab.name} From the ArmorStand. Level: {itemData.m_quality} " +
          $"Crafted By: {itemData.m_crafterName}");
        stand.DestroyAttachment(i);
        stand.UpdateSupports();
        stand.m_cloths = stand.GetComponentsInChildren<Cloth>();
      }
    }
    return itemlist;
  }

  public static void PlayerEquipmentToArmorStand(ArmorStand stand, Humanoid player)
  {
    List<ItemDrop.ItemData> playerItems = new List<ItemDrop.ItemData>();
    playerItems.Add(player.m_rightItem);
    playerItems.Add(player.m_leftItem);
    playerItems.Add(player.m_chestItem);
    playerItems.Add(player.m_legItem);
    playerItems.Add(player.m_helmetItem);
    if (SwapCloakItem.Value)
    {
      playerItems.Add(player.m_shoulderItem);
    }
    if (SwapUtilityItem.Value)
    {
      playerItems.Add(player.m_utilityItem);
    }
    stand.CancelInvoke("UpdateAttach");
    foreach (ItemDrop.ItemData equip in playerItems)
    {
      if (equip != null)
      {
        int slot = FindFreeSlot(stand, equip);
        if (slot < 0)
        {
          throw new IndexOutOfRangeException("Error!");
        }
        stand.m_queuedItem = equip;
        ZLog.Log($"Sending {equip.m_shared.m_name} to the Armor Stand.  Level: {equip.m_quality} Crafted By: {equip.m_crafterName}");
        stand.m_queuedSlot = slot;
        stand.UpdateAttach();
      }
    }
    stand.InvokeRepeating("UpdateAttach", 0f, 0.1f);
  }

  public static void ArmorStandEquipmentToPlayer(List<ItemDrop.ItemData> itemlist, Humanoid human)
  {
    Player player = Player.m_localPlayer;

    if (human == player)
    {

      foreach (ItemDrop.ItemData item in itemlist)
      {
        bool flag = player.m_inventory.AddItem(item);
        if (!flag)
        {
          ItemDrop.DropItem(item, 1, player.transform.position, player.transform.rotation);
        }
        else
        {
          player.QueueEquipAction(item);
        }
      }
    }
  }

  private static int FindFreeSlot(ArmorStand stand, ItemDrop.ItemData item)
  {
    for (int i = 0; i < stand.m_slots.Count; i++)
    {
      if (stand.CanAttach(stand.m_slots[i], item))
      {
        return i;
      }
    }
    return -1;
  }
}