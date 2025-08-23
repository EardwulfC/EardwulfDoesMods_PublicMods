namespace OhBeehive;

using System.Collections.Generic;
using UnityEngine;

using static PluginConfig;

public class HoneyExtractionManager : MonoBehaviour
{
  private static HoneyExtractionManager _instance;

  private List<Beehive> _allBeehives = new List<Beehive>();

  //private static bool _isQuitting = false;

  // A lock object for thread safety, which is good practice although not strictly required for this specific case
  private static readonly object _lock = new object();

  public static HoneyExtractionManager Instance
  {
    get
    {
      //if (_isQuitting)
      //{
      //  OhBeehive._logger.LogInfo("Singleton 'Instance' already destroyed on application quit. Won't create again - returning null.");
      //  return null;
      //}

      // The lock ensures that only one thread can be in this block of code at a time.
      lock (_lock)
      {
        if (_instance != null)
        {
          return _instance;
        }

        // If not, try to find an existing one in the scene.
        _instance = FindObjectOfType<HoneyExtractionManager>();

        // If one doesn't exist, we create it.
        if (_instance == null)
        {
          ZLog.Log("HoneyExtractionManager instance is required. Creating a new one.");
          GameObject managerObject = new GameObject("HoneyExtractionManager");
          _instance = managerObject.AddComponent<HoneyExtractionManager>();
        }

        return _instance;
      }
    }
  }

  void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else if (_instance != this)
    {
      ZLog.LogWarning("Duplicate HoneyExtractionManager detected. Destroying the duplicate.");
      Destroy(gameObject);
    }
  }

  //void OnApplicationQuit()
  //{
  //  _isQuitting = true;
  //}

  public void RegisterBeehive(Beehive beehive)
  {
    if (!_allBeehives.Contains(beehive))
    {
      _allBeehives.Add(beehive);
    }
  }

  public void UnregisterBeehive(Beehive beehive)
  {
    _allBeehives.Remove(beehive);
  }

  void Start()
  {
    if (IsModEnabled.Value)
    {
      InvokeRepeating(nameof(LootBeehive), 1f, 1f);
    }
  }

  public void LootBeehive()
  {
    if (!IsModEnabled.Value || !Player.m_localPlayer)
    {
      return;
    }

    foreach (Beehive currentBeehive in _allBeehives)
    {

      if (!currentBeehive.gameObject.activeInHierarchy)
      {
        continue;
      }

      if (Vector3.Distance(currentBeehive.transform.position, Player.m_localPlayer.transform.position) > AutoExtractRange.Value)
      {
        continue;
      }

      if (currentBeehive.GetHoneyLevel() == 0)
      {
        continue;
      }

      if (!PrivateArea.CheckAccess(currentBeehive.transform.position, 0f, true, false))
      {
        Chat.m_instance.AddString("You are not on the ward for this area.");
        OhBeehive._logger.LogInfo("You are not in the ward for this area.");
        continue;
      }
      currentBeehive.Extract();
      OhBeehive._logger.LogInfo("Attempting Honey Extraction");
    }
  }
}