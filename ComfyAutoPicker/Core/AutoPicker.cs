namespace ComfyAutoPicker;

using UnityEngine;

using static PluginConfig;


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
    bool flag1 = Player.m_localPlayer.m_rightItem?.m_dropPrefab.name == "Scythe";
    //AutoHarvestRadiusAdjustmentValue (AHRAV) is used to modify the AutoHarvestRadius to make
    //it equivalent to the attack radius of the scythe.
    float AHRAV = 1.5f;

    if (!flag1)
    {
      if (Vector3.Distance(transform.position, Player.m_localPlayer.transform.position) > AutoPickRadius.Value)
      {
        return;
      }
    }

    if (flag1)
    {
      float harvestRadius = Player.m_localPlayer.m_rightItem.m_shared.m_attack.m_harvestRadius;
      float harvestRadiusMaxLevel = Player.m_localPlayer.m_rightItem.m_shared.m_attack.m_harvestRadiusMaxLevel;
      float skillFactor = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Farming);
      float AutoHarvestRadius = Mathf.Lerp(harvestRadius, harvestRadiusMaxLevel, skillFactor) + AHRAV;

      if (Vector3.Distance(transform.position, Player.m_localPlayer.transform.position) > AutoHarvestRadius)
      {
        return;
      }
    }

    if (!PrivateArea.CheckAccess(transform.position, 0f, true, false))
    {
      Chat.m_instance.AddString($"You are not on the ward for this area.");
      return;
    }

    if (Player.m_localPlayer.m_rightItem?.m_dropPrefab.name == "Cultivator")
    {
      return;
    }

    _pickable.Interact(Player.m_localPlayer, false, false);
  }
}
