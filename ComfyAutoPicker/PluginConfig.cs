using BepInEx.Configuration;
using UnityEngine;

namespace ComfyAutoPicker
{
    public static class PluginConfig
    {
        public static ConfigEntry<bool> IsModEnabled { get; private set; }
        //public static ConfigEntry<float> Radius { get; private set; }

        public static ConfigEntry<KeyboardShortcut> ToggleEnabledShortcut;

        public static void BindConfig(ConfigFile config)
        {
            // Replacing the Radius variable with a static 0.875f
            //Radius = config.Bind("Settings", "AutoPick Radius - Game auto pick up radius is 2.", 0.8f, 
            //    new ConfigDescription("Radius at which pickables will be auto-picked.",
            //    new AcceptableValueRange<float>(0.8f, 0.875f)));

            IsModEnabled = config.Bind("Global", "IsModEnabled", true, "Globally enable or disable this mod");

            ToggleEnabledShortcut = config.Bind("Movement", "Enable/Disable Shortcut",
                new KeyboardShortcut(KeyCode.R, KeyCode.RightShift), "Keyboard Shortcut to toggle on/off Auto Picking");

        }
    }
}