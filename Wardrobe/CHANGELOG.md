## Changelog

### 1.4.1
  * Recompiled for 0.221.4
  * Added the Crafter Name to the logging statements when items are sent to, and picked up from, the ArmorStand
  * Updated the BepInEx Dependancy to `denikson-BepInExPack_Valheim-5.4.2333`
  * Moved the bulk of the mod code into Core.cs as step 1 of refactoring this mod

### 1.4.0
  * Made swapping your cloak/cape option a with a new Config Entry SwapCloakItem, defaults to true

### 1.3.0
  * Bumped up <LangVersion> to C# 12
  * Made swapping your Utility Item optional with a new Config Entry SwapUtilityItem, defaults to true

### 1.2.0

  * Added a check to drop items if your inventory is full
  * Bumped up <LangVersion> to C# 10
  * Special thank you to OrianaVenture for helping with the update to prevent item deletion!

### 1.0.1

  * Added logging statements to confirm items are being queued correclty.

### 1.0.2

* Patch to resolve issue with belts and wisplights being stacked on top of each other resulting in the loss of items.
* Belts and Wisplights will now be swapped with the ArmorStand.  Future Me will address this with a proper fix.

### 1.0.0

  * Initial release.