# ComfyMassFarming

  * A fork of Xeio's Mass Farming Mod for use on Comfy Valheim.
  * Grid size is locked to a maximum of 3x3, you may select any combination between 1x1 through 3x3.
  * Stamina usage has been increased by 76.95% when mass planting to help reduce the advantage over Vanilla players
  * Mass Picking feature has been removed
  * Mod Name has been updated to differentiate it from the Mass Farming Mod
  * Logging messages added to display stamina usage v. normal stamina usage.

## Installation

### Manual

  * Un-zip `ComfyMassFarming.dll` to your `/Valheim/BepInEx/plugins/` folder.

### Thunderstore (manual)

  2. Go to Settings > Import local mod > Select `ComfyMassFarming_v1.3.0.zip`.
  3. Click "OK/Import local mod" on the pop-up for information.

## Instructions

  * Using a keybind (default Left Alt) you can plant a grid of crops up to the maximum allowed size in the config file.
  * Red plants indicate that either the plant is too close to another plant, too close to another object (build piece or natural object).
  * A red plant may also indicate that you lack the resources (seeds) or Stamina to plant it.

## Changelog

### 1.3.1

* Bug Fix to account to reduced stamaina usage with the cultivator when wearing Hildir Clothing.

### 1.3.0

* Moved code around and created PluginCongfig.cs file to match standard practices on Comfy.

### 1.2.3

* Added accessability option to enable grid planting only.

### 1.2.2

* Updated for compatibility with patch 0.217.24

### 1.2.1

* Updated build count increment function for new IncrementPlayerstats
* Recompiled against dlls for version 0.217.14

### 1.2.0

* Bumped BepInEx dependancy to version 5.4.2105
* Include correction to GetRightItem missing from Xeio's main version of Mass Farming.
* Full compatibility with Valheim v0.216.9

### 1.1.0

* Initial release on Comfy

### 1.0.1

* corrected a bug that was forcing the mod into mass planting mode and would not disable it.

### 1.0.0

  * Initial fork from Xeio's Mass Farming Mod.  This is a Comfy Valheim specific port.
  * Compatible with Farm Grid to allow grids of plants to be snapped together. 