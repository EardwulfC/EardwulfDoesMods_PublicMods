using BepInEx.Configuration;
using UnityEngine;

namespace ComfyAutoPicker
{
    public static class PluginConfig
    {
        public static ConfigEntry<bool> IsModEnabled { get; private set; }

        public static ConfigEntry<KeyboardShortcut> ToggleEnabledShortcut;

        public static void BindConfig(ConfigFile config)
        {
            IsModEnabled = config.Bind("Global", "IsModEnabled", true, "Globally enable or disable this mod");

            //ToggleEnabledShortcut = config.Bind("Movement", "Enable/Disable Shortcut",
            //    new KeyboardShortcut(KeyCode.R, KeyCode.RightShift), "Keyboard Shortcut to toggle on/off Auto Picking");
        }
    }
}