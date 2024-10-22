namespace FightMeBro;

using HarmonyLib;

using static PluginConfig;


internal class InventoryGUIPatches
{
  [HarmonyPatch(typeof(InventoryGui))]
  static class GuiPVPPatch
  {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.UpdateCharacterStats))]

    static void GuiPVPUpdater(InventoryGui __instance)
    {
      if (!InventoryGui.m_instance || !IsModEnabled.Value)
      {
        return;
      }
      __instance.m_pvp.isOn = true;
    }
  }
}



