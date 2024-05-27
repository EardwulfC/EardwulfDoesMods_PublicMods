using BepInEx.Configuration;

namespace ComfyAutoPicker
{
  public static class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static void BindConfig(ConfigFile config) {
        IsModEnabled = config.Bind("Global", "IsModEnabled", true, "Globally enable or disable this mod");
    }
  }
}
