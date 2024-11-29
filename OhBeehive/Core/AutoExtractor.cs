namespace OhBeehive;

using UnityEngine;

using static PluginConfig;

public sealed class HoneyExtractor : MonoBehaviour
{
  Beehive _beehive;

  void Awake()
  {
    _beehive = GetComponent<Beehive>();
    if (IsModEnabled.Value)
    {
      InvokeRepeating(nameof(LootBeehive), 1f, 0.6f);
    }
  }

  public void LootBeehive()
  {
    if (!IsModEnabled.Value || !Player.m_localPlayer)
    {
      return;
    }

    if (Vector3.Distance(transform.position, Player.m_localPlayer.transform.position) > AutoExtractRange.Value)
    {
      return;
    }

    if (!PrivateArea.CheckAccess(transform.position, 0f, true, false))
    {
      Chat.m_instance.AddString($"You are not on the ward for this area.");
      return;
    }
    _beehive.Extract();
  }
}