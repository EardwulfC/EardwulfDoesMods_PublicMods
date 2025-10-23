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

  private int GetPlantsPerSwing(float skillLevel)
  {
    return skillLevel switch
    {
      < 25             => 7,
      >= 25 and < 50  => 10,
      >= 50 and < 75  => 13,
      >= 75 and < 100 => 16,
      >= 100          => 20,
      _                => 7
    };
  }

  private float GetStaminaModifier(float skillLevel)
  {
    return skillLevel switch
    {
      >= 100 => 0.67f, // 33% discount
      >= 75 => 0.75f,  // 25% discount
      >= 50 => 0.83f,  // 17% discount
      >= 25 => 0.92f,  //  8% discount
      _ => 1.0f        //  0% discount
    };
  }

  public void CheckAndPick()
  {
    if (!IsModEnabled.Value || !Player.m_localPlayer)
    {
      return;
    }

    Player player = Player.m_localPlayer;
    bool hasScythe = player.m_rightItem?.m_dropPrefab.name == "Scythe";
    bool hasCultivator = player.m_rightItem?.m_dropPrefab.name == "Cultivator";

    //AutoHarvestRadiusAdjustmentValue (AHRAV) is used to modify the AutoHarvestRadius to make
    //it equivalent to the attack radius of the scythe.
    //float AHRAV = 1.5f;

    if (!hasScythe)
    {
      if (Vector3.Distance(transform.position, player.transform.position) > AutoPickRadius.Value)
      {
        return;
      }
    }

    if (hasScythe)
    {
      float harvestRadius = Player.m_localPlayer.m_rightItem.m_shared.m_attack.m_harvestRadius;
      float harvestRadiusMaxLevel = Player.m_localPlayer.m_rightItem.m_shared.m_attack.m_harvestRadiusMaxLevel;
      float skillFactor = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Farming);
      float AutoHarvestRadius = Mathf.Lerp(harvestRadius, harvestRadiusMaxLevel, skillFactor);

      if (Vector3.Distance(transform.position, player.transform.position) > AutoHarvestRadius)
      {
        return;
      }
    }

    if (!PrivateArea.CheckAccess(transform.position, 0f, true, false))
    {
      return;
    }

    if (hasCultivator)
    {
      return;
    }

    if (hasScythe)
    {
      float farmingSkill = player.m_skills.GetSkill(Skills.SkillType.Farming).m_level;
      int plantsPerSwing = GetPlantsPerSwing(farmingSkill);
      ItemDrop.ItemData scythe = player.m_rightItem;
      bool hasStamina = false;
      float staminaPerPlant = 0f;

      if (plantsPerSwing > 0)
      {

        float staminaPerSwing = scythe.m_shared.m_attack.m_attackStamina *
          GetStaminaModifier(farmingSkill);

        staminaPerPlant = staminaPerSwing / plantsPerSwing;

        hasStamina = player.HaveStamina(staminaPerPlant);
      }

      if (scythe.m_durability <= 0f || plantsPerSwing <= 0 || !hasStamina)
      {
        return;
      }

      if (_pickable.Interact(Player.m_localPlayer, false, false))
      {
        float durabilityPerPlant = (scythe.m_shared.m_useDurabilityDrain / plantsPerSwing) * 1.05f;

        scythe.m_durability -= durabilityPerPlant;
        player.UseStamina(staminaPerPlant);
      }
    }
    else
    {
      _pickable.Interact(Player.m_localPlayer, false, false);
    }
  }
}
