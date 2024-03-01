using BepInEx.Configuration;

namespace YolkMe
{
    public static class PluginConfig
    {
        public static ConfigEntry<bool> IsModEnabled { get; private set; }

        public static void BindConfig(ConfigFile config)
        {
            IsModEnabled = config.Bind("Global", "IsModEnabled", true, "Globally enable or disable this mod.  Requires restart to work.");
        }
    }
}