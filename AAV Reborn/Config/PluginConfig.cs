namespace AAV_Reborn;

using BepInEx.Configuration;


public static class PluginConfig
{
  public static ConfigEntry<string> ErrorMessage { get; private set; }

  public static void BindConfig(ConfigFile config)
  {
    ErrorMessage = config.Bind("ErrorMessage", "Custom Error Message", "You've had enough", 
      "Message to show when you don't need to drink any mead.");
  }
}
