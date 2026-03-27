namespace AmIPvP;

using HarmonyLib;

using System.Linq;

using UnityEngine;

[HarmonyPatch(typeof(ObjectDB))]
internal class ObjectDBPatches
{
  [HarmonyPatch(nameof(ObjectDB.Awake))]
  [HarmonyPostfix]

  public static void SetupStatusEffect(ObjectDB __instance)
  {
    bool flag1 = __instance.m_StatusEffects.Any((StatusEffect effect) => effect.name == "AmIPvP_PvPOff");

    if (flag1)
    {
      return;
    }

    string spriteRequested1 = "pvp_off";
    
    Sprite spriteToUse1 = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault((Sprite s1) => s1.name == spriteRequested1);
    if (spriteToUse1 == null)
    {
      ZLog.Log($"AmIPvP: Could not find sprite {spriteRequested1} in loaded game resources.");
      return;
    }

    StatusEffect AmIPvPEffect1 = ScriptableObject.CreateInstance<StatusEffect>();
    AmIPvPEffect1.name = "AmIPvP_PvPOff";
    AmIPvPEffect1.m_name = "PvP Off";
    AmIPvPEffect1.m_tooltip = "Your PvP is currently off.";
    AmIPvPEffect1.m_icon = spriteToUse1;
    __instance.m_StatusEffects.Add(AmIPvPEffect1);

    bool flag2 = __instance.m_StatusEffects.Any((StatusEffect effect) => effect.name == "AmIPvP_PvPOn");

    if (flag2)
    {
      return;
    }

    string spriteRequested2 = "pvp_on";

    Sprite spriteToUse2 = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault((Sprite s2) => s2.name == spriteRequested2);
    if (spriteToUse2 == null)
    {
      ZLog.Log($"AmIPvP: Could not find sprite {spriteRequested2} in loaded game resources.");
      return;
    }

    StatusEffect AmIPvPEffect2 = ScriptableObject.CreateInstance<StatusEffect>();
    AmIPvPEffect2.name = "AmIPvP_PvPOn";
    AmIPvPEffect2.m_name = "PvP On";
    AmIPvPEffect2.m_tooltip = "Your PvP is currently on.";
    AmIPvPEffect2.m_icon = spriteToUse2;
    __instance.m_StatusEffects.Add(AmIPvPEffect2);
  }
}