namespace FightMeBro;

using BepInEx.Configuration;

public class PluginConfig
{
  public static ConfigEntry<bool> IsModEnabled { get; private set; }

  public static void BindConfig(ConfigFile config)
  {
    IsModEnabled = config.Bind<bool>("Global", "isModEnabled", true, "Globally enable or disable this mod.");
  }
}