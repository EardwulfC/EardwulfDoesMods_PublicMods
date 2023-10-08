using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace MassFarming
{
    //[BepInPlugin("EardwulfDoesMods.Comfy.MassFarming", "Comfy.MassFarming", "1.2.3")]
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class MassFarming : BaseUnityPlugin
    {
        public const string PluginGuid = "EardwulfDoesMods.Comfy.MassFarming";
        public const string PluginName = "Comfy.MassFarming";
        public const string PluginVersion = "1.2.3";

        public static ConfigEntry<KeyboardShortcut> MassActionHotkey { get; private set; }
        public static ConfigEntry<bool> NoActionKeyNeeded { get; private set; }
        public static ConfigEntry<KeyboardShortcut> ControllerPickupHotkey { get; private set; }
        public static ConfigEntry<int> PlantGridWidth { get; private set; }
        public static ConfigEntry<int> PlantGridLength { get; private set; }
        public static ConfigEntry<bool> GridAnchorWidth { get; private set; }
        public static ConfigEntry<bool> GridAnchorLength { get; private set; }

        public void Awake()
        {
            MassActionHotkey = Config.Bind("Hotkeys", nameof(MassActionHotkey), new KeyboardShortcut(KeyCode.LeftAlt), "Mass activation hotkey for multi-plant.");
          
            ControllerPickupHotkey = Config.Bind("Hotkeys", nameof(ControllerPickupHotkey), new KeyboardShortcut(KeyCode.JoystickButton4), "Mass activation hotkey for multi-pickup/multi-plant for controller.");

            NoActionKeyNeeded = Config.Bind("Override Grid Plant", nameof(NoActionKeyNeeded), false, "Enable or disable grid planting without an action key");

            PlantGridWidth = Config.Bind("Plant Grid", "Plant Grid Width", 3, new ConfigDescription("The width of mass planting grid.  Default value is 3.",
                new AcceptableValueRange<int>(1, 3)));

            PlantGridLength = Config.Bind("Plant Grid", "Plant Grid Length", 3, new ConfigDescription("The lenght of the mass planting grid.  Default value is 3.",
                new AcceptableValueRange<int>(1, 3)));

            GridAnchorWidth = Config.Bind("PlantGrid", nameof(GridAnchorWidth), true, "Planting grid anchor point (width). Default is 'enabled' so the grid will extend left and right from the crosshairs. Disable to set the anchor to the side of the grid. Disable both anchors to set to corner.");
            
            GridAnchorLength = Config.Bind("PlantGrid", nameof(GridAnchorLength), true, "Planting grid anchor point (length). Default is 'enabled' so the grid will extend forward and backward from the crosshairs. Disable to set the anchor to the side of the grid. Disable both anchors to set to corner.");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }
    }
}