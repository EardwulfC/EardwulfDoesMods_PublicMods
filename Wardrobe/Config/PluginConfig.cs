using BepInEx.Configuration;
using UnityEngine;


namespace Wardrobe
{
  public static class PluginConfig
  {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<KeyboardShortcut> QuickSwapKey;
    public static ConfigEntry<bool> SwapUtilityItem { get; private set; }

    public static void BindConfig(ConfigFile config)
    {
      IsModEnabled = config.Bind("Global", "IsModEnabled", true, "Enable or Disable this mod");

      QuickSwapKey = config.Bind("Quick Swap Keybind", "QuickSwapKey",
          new KeyboardShortcut(KeyCode.E, KeyCode.LeftAlt), "Keyboard Shortcut to swap equipment.  You must `E` with a modifier key (LeftAlt, RightCtrl, etc.");

      SwapUtilityItem = config.Bind("Swap Options", "SwapUtilityItem", true, "Enable or disable swapping your Utility Item");
    }
  }
}