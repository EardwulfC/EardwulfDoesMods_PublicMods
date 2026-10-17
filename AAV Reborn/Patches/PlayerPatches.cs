namespace AAV_Reborn;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Player))]
internal class PlayerPatches
{
  [HarmonyPatch(nameof(Player.CanConsumeItem))]
  [HarmonyPostfix]

  public static void SaveTheMeads(Player __instance, ref bool __result, ItemDrop.ItemData item)
  {
    float hpPercent = __instance.GetHealthPercentage();
    float stamPercent = __instance.GetHealthPercentage();
    float eitrPercent = __instance.GetEitrPercentage();
    string name = item.m_shared.m_name;

    if ((name == "$item_mead_hp_major" || name == "$item_mead_hp_medium" || 
        name == "$item_mead_hp_minor") && hpPercent >= 0.9f)
    {
      AAV_Reborn.LogInfo($"Health Percentage is {hpPercent * 100} so {name} should not be consumed");
      __instance.Message(MessageHud.MessageType.Center, ErrorMessage.Value, 0, null, false);
      __result = false;
    }

    if ((name == "$item_mead_stamina_medium" || name == "$item_mead_stamina_minor") && stamPercent >= 0.9f)
    {
      AAV_Reborn.LogInfo($"Stamina Percentage is {stamPercent * 100} so {name} should not be consumed");
      __instance.Message(MessageHud.MessageType.Center, ErrorMessage.Value, 0, null, false);
      __result = false;
    }

    if ((name == "$item_mead_eitr_minor") && eitrPercent >= 0.9f)
    {
      AAV_Reborn.LogInfo($"Eitr Percentage is {eitrPercent * 100} so {name} should not be consumed");
      __instance.Message(MessageHud.MessageType.Center, ErrorMessage.Value, 0, null, false);
      __result = false;
    }
  }
}

