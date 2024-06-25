using UnityEngine;

using static ComfyAutoPicker.PluginConfig;

namespace ComfyAutoPicker
{
  public sealed class AutoPicker : MonoBehaviour
  {
    Pickable _pickable;

    void Awake()
    {
      _pickable = GetComponent<Pickable>();

      if (IsModEnabled.Value)
      {
        InvokeRepeating(nameof(CheckAndPick), 1f, 0.425f);
      }
    }

    public void CheckAndPick()
    {
      if (!IsModEnabled.Value || !Player.m_localPlayer)
      {
        return;
      }

      // Distance check.
      if (Vector3.Distance(transform.position, Player.m_localPlayer.transform.position) > AutoPickRadius.Value)
      {
        return;
      }

      if (!PrivateArea.CheckAccess(transform.position, 0f, true, false))
      {
        Chat.m_instance.AddString($"You are not on the ward for this area.");
        return;
      }

      if (Player.m_localPlayer.m_rightItem?.m_dropPrefab.name == "Cultivator")
      {
        //spam the chat with unable to pick messages when holding a Cultivator
        //Chat.m_instance.AddString($"You cannot pick plants when you have a Cultivator equipped.");
        return;
      }

      _pickable.Interact(Player.m_localPlayer, false, false);
    }
  }
}