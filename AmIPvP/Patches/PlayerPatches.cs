namespace AmIPvP;

using HarmonyLib;

internal class PlayerPatches
{
  [HarmonyPatch(typeof(Player))]
  static class PlayerPatch
  {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.IsPVPEnabled))]
    static void IsPvPEnabledChecker(Player __instance, bool __result)
    {
      if (__instance != Player.m_localPlayer ||
          AmIPvP.IsMyPvPEnabled == __result)
      {
        return;
      }

      AmIPvP.IsMyPvPEnabled = __result;
    }
  }

  [HarmonyPatch(typeof(Player))]
  static class UpdatePatch
  {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Update))]

    static void UpdatePvPStatus(Player __instance)
    {
      if (__instance != Player.m_localPlayer)
      {
        return;
      }
      SEMan seman = __instance.GetSEMan();

      if (seman.m_character != Player.m_localPlayer)
      {
        return;
      }

      bool hasEffectPVPOff = seman.HaveStatusEffect(AmIPvP.hashPvPOff);
      bool hasEffectPVPOn = seman.HaveStatusEffect(AmIPvP.hashPvPOn);

      if (AmIPvP.IsMyPvPEnabled)
      {
        if (hasEffectPVPOn == true && hasEffectPVPOff == false)
        {
          return;
        }
        if (hasEffectPVPOn == false)
        {
          seman.AddStatusEffect(AmIPvP.hashPvPOn);
        }
        if (hasEffectPVPOff == true)
        {
          seman.RemoveStatusEffect(AmIPvP.hashPvPOff);
        }
      }

      if (!AmIPvP.IsMyPvPEnabled)
      {
        if (hasEffectPVPOff == true && hasEffectPVPOn == false)
        {
          return;
        }
        if (hasEffectPVPOff == false)
        {
          seman.AddStatusEffect(AmIPvP.hashPvPOff);
        }
        if (hasEffectPVPOn == true)
        {
          seman.RemoveStatusEffect(AmIPvP.hashPvPOn);
        }
      }
    }
  }
}
