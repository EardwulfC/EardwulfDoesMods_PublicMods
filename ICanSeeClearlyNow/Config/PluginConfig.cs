using BepInEx.Configuration;

namespace ICanSeeClearlyNow {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<float> GammaValue { get; private set; }

    public static void BindConfig(ConfigFile Config) {
      IsModEnabled = Config.Bind<bool>("_Global", "isModEnabled", true, "Globally enable or disable the mode.");
      GammaValue = Config.Bind("Gamma Setting", "GammaValue", 0.6f, new ConfigDescription("Set your Gamma Value between 0.3 and 0.99",
          new AcceptableValueRange<float>(0.3f, 0.99f)));
    }
  }
}