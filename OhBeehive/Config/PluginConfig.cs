namespace OhBeehive;

using BepInEx.Configuration;

public static class PluginConfig
{
  public static ConfigEntry<bool> IsModEnabled { get; private set; }
  public static ConfigEntry<float> AutoExtractRange { get; private set; }

  public static void BindConfig(ConfigFile config)
  {
    IsModEnabled = config.Bind("_Global", "IsModEnabled", true, "Enable or Disable this mod");
    AutoExtractRange = config.Bind("Extract", "AutoExtractRange", 1f,
      new ConfigDescription("Range to extract Honey.",
        new AcceptableValueRange<float>(0.875f, 3f)));
  }
}
