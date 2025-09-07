namespace AsgardianAudioAdjuster;

using BepInEx.Configuration;

using System.Collections.Generic;

public class PluginConfig
{
  public static ConfigEntry<bool> IsModEnabled { get; private set; }
  public static ConfigEntry<bool> MuteDeathsquito { get; private set; }
  public static ConfigEntry<bool> MuteOvenDoors { get; private set; }
  public static ConfigEntry<bool> MuteShieldGenerator { get; private set; }


  public static void BindConfig(ConfigFile config)
  {
    IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable the mod.");

    MuteDeathsquito = config.Bind("MuteList", "muteDeathsquito", false, "Mute or unmute the Deathsquito");

    MuteOvenDoors = config.Bind("MuteList", "muteOvenDoors", false, "Mute or unmute oven doors");

    MuteShieldGenerator = config.Bind("MuteList", "muteShieldGenerator", false, "Mute or unmute the Shield Generator");

    MuteDeathsquito.SettingChanged += (_, _) => UpdateSoundMuterList();
    MuteOvenDoors.SettingChanged += (_, _) => UpdateSoundMuterList();
    MuteShieldGenerator.SettingChanged += (_, _) => UpdateSoundMuterList();
  }

  public static HashSet<string> soundsToIgnore = new HashSet<string>();
  public static void UpdateSoundMuterList()
  {
    soundsToIgnore.Clear();

    if (MuteDeathsquito.Value)
    {
      soundsToIgnore.Add("Insect_Wasp_Wings");
      soundsToIgnore.Add("sfx_deathsquito_attack");
    }

    if (MuteOvenDoors.Value)
    {
      soundsToIgnore.Add("Objects_StoneOven_Door");
    }

    if (MuteShieldGenerator.Value)
    {
      soundsToIgnore.Add("Shield_Generator_Engine");
    }
    UpdateAudioSources.UpdateAudioList();
  }
}

