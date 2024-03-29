using BepInEx.Configuration;

namespace YolkMe {
  public static class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<bool> PickUpAllEnabled {  get; private set; }

    public static void BindConfig(ConfigFile config) {
      IsModEnabled =
          config.Bind("_Global", "IsModEnabled", true, "Globally enable or disable this mod. Requires restart to work.");
            PickUpAllEnabled =
          config.Bind("Egg", "PickUpAllEnabled", false, "enable or disable picking up warm eggs.  Requires restart to work.");
    }
  }
}