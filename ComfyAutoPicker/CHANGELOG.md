# Changelog

### 1.5.0
  * Recompiled for 0.219.16
  * Adjusted the AutoPick Radius to be approximately the same radius as that of using the Scythe.

### 1.4.0
  * Added Pickable_Fiddlehead to the list of plants that will be auto-picked.
  * Bumped up <LangVersion> to C# 12

### 1.3.4

  * Added Pickable_Thistle to list of plants that will be auto-picked.

### 1.3.3

  * Thank you to shagartz from the Valheim Modding Discord for letting me know that the CHANGELOG was not publishing to Thunderstore, this is now fixed.

### 1.3.2

  * Changed the auto-picker max distance from a static 0.875f to an allowable range between 0.1f and 0.875f
  * Removed the Cultivator Spam in the chat log when attempting to auto-pick with a cultivator equipped
  * you still cannot pick with a cultivator equipped.

### 1.3.1

  * Removed logging statement that is spamming logs.

### 1.3.0

  * Bumped up <LangVersion> C# 10
  * Updated for Ashlands `v0.218.15`
  * Added Vine Berries (VineAsh) to the list of pickables

### 1.2.0

  * Refactored code to be much less expensive (Thank you Redseiko!)
  * Moved Changelog into CHANGELOG.md 
  * Removed Keybind for toggling if the mod is enabled or not.

### 1.1.0

  * Added a private area access check to prevent people from picking plants where they not included in the ward.

### 1.0.0

  * Initial release.
  * Restricted list of pickable items for use on Comfy includes Barley, Flax, Cloudberries, Jotunn Puffs, Mage Caps,
  * Onions, Onion Seeds, Turnips, Turnip Seeds, Carrots, and Carrot Seeds.
  * Removed a function that was removing a .5 second delay on pick up to restore vanilla behavior
  * Restricted the auto-pick radius