using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using static Wardrobe.PluginConfig;

namespace Wardrobe
{
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class Wardrobe : BaseUnityPlugin
  {
    public const string PluginGuid = "EardwulfDoesMods.valheim.Wardrobe";
    public const string PluginName = "Wardrobe";
    public const string PluginVersion = "1.1.0";

    Harmony _harmony;

    void Awake()
    {
      BindConfig(Config);
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

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
          ZLog.Log("You do you have access to this armor stand.");
          return false;
        }

        if (__instance.m_nview && __instance.m_nview.m_zdo.m_prefab == ArmorStandHashCode)
        {

          List<ItemDrop> itemList = ArmorStandEquip(__instance);
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

    }

    public static List<ItemDrop> ArmorStandEquip(ArmorStand stand)
    {
      List<ItemDrop> itemlist = new List<ItemDrop>();
      for (int i = 0; i < stand.m_slots.Count; i++)
      {
        if (stand.HaveAttachment(i))
        {
          string itemstring = stand.m_nview.GetZDO().GetString(i.ToString() + "_item");
          ItemDrop itemDrop = ObjectDB.instance.GetItemPrefab(itemstring).GetComponent<ItemDrop>();
          ItemDrop.LoadFromZDO(i, itemDrop.m_itemData, stand.m_nview.GetZDO());
          itemlist.Add(itemDrop);
          ZLog.Log($"Wardrobe says I removed item {itemDrop.name} From the ArmorStand. Level: {itemDrop.m_itemData.m_quality}");
          stand.DestroyAttachment(i);
          stand.m_nview.GetZDO().Set(i.ToString() + "_item", "");
          stand.m_nview.InvokeRPC(ZNetView.Everybody, "RPC_SetVisualItem", new object[]
          {
                      i,
                      "",
                      0
          });
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
      playerItems.Add(player.m_shoulderItem);
      playerItems.Add(player.m_utilityItem);
      stand.CancelInvoke("UpdateAttach");
      foreach (ItemDrop.ItemData equip in playerItems)
      {
        if (equip != null)
        {
          int slot = FindFreeSlot(stand, equip);
          if (slot < 0)
          {
            throw new IndexOutOfRangeException("This should never happen! Call a grownup!");
          }
          stand.m_queuedItem = equip;
          ZLog.Log($"Sending {equip.m_shared.m_name} to the Armor Stand.  Level: {equip.m_quality}");
          stand.m_queuedSlot = slot;
          stand.UpdateAttach();
        }
      }
      stand.InvokeRepeating("UpdateAttach", 0f, 0.1f);
    }

    public static void ArmorStandEquipmentToPlayer(List<ItemDrop> itemlist, Humanoid human)
    {
      Player player = Player.m_localPlayer;

      if (human == player)
      {

        foreach (ItemDrop itemdrop in itemlist)
        {
          bool flag = player.m_inventory.AddItem(itemdrop.m_itemData);
          if (!flag)
          {
            GameObject.Instantiate(itemdrop.m_itemData.m_dropPrefab, player.transform.position, player.transform.rotation);
          }
          else
          {
            player.QueueEquipAction(itemdrop.m_itemData);
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

    void OnDestroy()
    {
      _harmony?.UnpatchSelf();
    }
  }
}
