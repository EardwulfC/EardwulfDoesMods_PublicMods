using BepInEx.Configuration;

namespace ComfyAutoPicker
{
  public static class PluginConfig
  {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<float> AutoPickRadius { get; private set; }
    public static void BindConfig(ConfigFile config)
    {
      IsModEnabled = config.Bind("_Global", "IsModEnabled", true, "Globally enable or disable this mod");

      AutoPickRadius = config.Bind("AutoPickerProperties", "AutoPickRadius", 0.875f, new ConfigDescription("Adjust Auto-Picker Radius",
        new AcceptableValueRange<float>(0.1f, 0.875f)));
    }
  }
}
