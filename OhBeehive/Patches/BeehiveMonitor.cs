namespace OhBeehive;

using UnityEngine;

public sealed class BeehiveMonitor : MonoBehaviour
{
  private Beehive _beehive;

  void Awake()
  {
    _beehive = GetComponent<Beehive>();
  }

  void OnDestroy()
  {
    if (HoneyExtractionManager.Instance != null)
    {
      HoneyExtractionManager.Instance.UnregisterBeehive(_beehive);
      OhBeehive._logger.LogInfo($"A Beehive has unregistered via its monitor.");
    }
  }
}