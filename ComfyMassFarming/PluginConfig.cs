using BepInEx.Configuration;
using UnityEngine;

namespace MassFarming
{
    public class PluginConfig
    {
        public static ConfigEntry<KeyboardShortcut> MassActionHotkey { get; private set; }
        public static ConfigEntry<bool> NoActionKeyNeeded { get; private set; }
        public static ConfigEntry<KeyboardShortcut> ControllerPickupHotkey { get; private set; }
        public static ConfigEntry<int> PlantGridWidth { get; private set; }
        public static ConfigEntry<int> PlantGridLength { get; private set; }
        public static ConfigEntry<bool> GridAnchorWidth { get; private set; }
        public static ConfigEntry<bool> GridAnchorLength { get; private set; }

        public static void BindConfig(ConfigFile config)
        {
            MassActionHotkey = config.Bind("Hotkeys", nameof(MassActionHotkey),
                new KeyboardShortcut(KeyCode.LeftAlt),
                "Mass activation hotkey for multi-plant.");

            ControllerPickupHotkey = config.Bind("Hotkeys", nameof(ControllerPickupHotkey),
                new KeyboardShortcut(KeyCode.JoystickButton4),
                "Mass activation hotkey for multi-pickup/multi-plant for controller.");

            NoActionKeyNeeded = config.Bind("Override Grid Plant", nameof(NoActionKeyNeeded), false,
                "Enable or disable grid planting without an action key");

            PlantGridWidth = config.Bind("Plant Grid", "Plant Grid Width", 3,
                new ConfigDescription("The width of mass planting grid.  Default value is 3.",
                new AcceptableValueRange<int>(1, 3)));

            PlantGridLength = config.Bind("Plant Grid", "Plant Grid Length", 3,
                new ConfigDescription("The lenght of the mass planting grid.  Default value is 3.",
                new AcceptableValueRange<int>(1, 3)));

            GridAnchorWidth = config.Bind("PlantGrid", nameof(GridAnchorWidth), true,"Planting grid anchor point (width). Default is 'enabled' so the grid will extend left and right from the crosshairs. Disable to set the anchor to the side of the grid. Disable both anchors to set to corner.");

            GridAnchorLength = config.Bind("PlantGrid", nameof(GridAnchorLength), true, "Planting grid anchor point (length). Default is 'enabled' so the grid will extend forward and backward from the crosshairs. Disable to set the anchor to the side of the grid. Disable both anchors to set to corner.");
        }
    }
}