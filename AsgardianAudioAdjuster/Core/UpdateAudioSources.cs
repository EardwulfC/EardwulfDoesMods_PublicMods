namespace AsgardianAudioAdjuster;

using System.Linq;

using UnityEngine;

using static PluginConfig;

internal class UpdateAudioSources
{
  public static void UpdateAudioList()
  {
    AudioSource[] allAudioSources = Object.FindObjectsOfType<AudioSource>();

    foreach (AudioSource source in allAudioSources)
    {
      if (!source.isPlaying || source == null || source.clip == null)
      {
        continue;
      }

      if (soundsToIgnore.Any(pattern => source.clip.name.StartsWith(pattern)))
      {
        source.mute = true;
      }
      else
      {
        source.mute = false;
      }
    }
  }
}