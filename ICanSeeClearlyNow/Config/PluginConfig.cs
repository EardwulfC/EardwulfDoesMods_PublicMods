using BepInEx.Configuration;
using ComfyLib;

namespace ICanSeeClearlyNow {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<float> GammaValue { get; private set; }
    public static ConfigEntry<bool> IsAshlandsAlwaysOn {  get; private set; }
    public static ConfigEntry<bool> IsCustomTimer { get; private set; }
    public static ConfigEntry<float> IsCustomNight {  get; private set; }
    public static ConfigEntry<float> IsCustomDay { get; private set; }

    public static void BindConfig(ConfigFile Config) {
      IsModEnabled = Config.BindInOrder<bool>("_Global", "isModEnabled", true, "Globally enable or disable the mode.");

      GammaValue = Config.BindInOrder("Gamma Setting", "GammaValue", defaultValue: 0.6f, "Set your Gamma Value between 0.3 and 0.99",
          new AcceptableValueRange<float>(0.3f, 0.99f));

      IsAshlandsAlwaysOn = Config.BindInOrder<bool>("Always On Options", "IsAshlandsAlwaysOn", false, "Always have Gamma Adjustment in the Ashlands");

      IsCustomTimer = Config.BindInOrder<bool>("Custom Time", "IsCustomTimer", false, "Enable or Disable custom Night/Day Values");

      IsCustomDay = Config.BindInOrder<float>("Custom Time", "IsCustomDay", 6f, "Set a custom Start of Day value.");

      IsCustomNight = Config.BindInOrder<float>("Custom Time", "IsCustomNight", 18f, "Set a custom Start of Night value.");
    }
  }
}