namespace AsgardianAudioAdjuster;

using HarmonyLib;

using System.Linq;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(AudioMan))]
internal class AudioManPatches
{
  [HarmonyPatch(nameof(AudioMan.RequestPlaySound))]
  [HarmonyPrefix]

  public static bool Sfxmuter(ZSFX sfx)
  {
    if (!IsModEnabled.Value
        || sfx?.m_audioClips == null)
    {
      return true;
    }

    foreach (AudioClip clip in sfx.m_audioClips)
    {
      if (soundsToIgnore.Any(pattern => clip.name.StartsWith(pattern)))
      {
        sfx.Stop();
        sfx.m_minVol = 0f;
        sfx.m_maxVol = 0f;
      }
    }
    return true;
  }
}