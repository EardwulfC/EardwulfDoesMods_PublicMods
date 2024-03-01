﻿using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

using BepInEx;
using System.Reflection;

using static ComfyAutoPicker.PluginConfig;

namespace ComfyAutoPicker
{

    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    internal class Mod : BaseUnityPlugin
    {
        public const string PluginGuid = "EardwulfDoesMods.valheim.ComfyAutoPicker";
        public const string PluginName = "ComfyAutoPicker";
        public const string PluginVersion = "1.2.0";

        Harmony _harmony;

        public void Awake()
        {
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
            BindConfig(Config);
        }

        // removing the toggle hot key, open the F1 menu and toggle manually!
        //public void Update()
        //{
        //    if (Input.GetKey(ToggleEnabledShortcut.Value.MainKey) && ToggleEnabledShortcut.Value.Modifiers.All(new Func<KeyCode, bool>(Input.GetKeyDown)))
        //    {
        //        IsModEnabled.Value = !IsModEnabled.Value;
        //    }
        //}

        public class Patches
        {
            // add to every Pickable
            [HarmonyPatch(typeof(Pickable), "Awake")]
            public static class PickableAwakePatch
            {

                // Make HashSet Here
                public static readonly HashSet<int> PlantsToAutoPick = new HashSet<int>() {
                  "Pickable_Barley".GetStableHashCode(), "Pickable_Barley_Wild".GetStableHashCode(), "Pickable_Flax".GetStableHashCode(),
                  "Pickable_Flax_Wild".GetStableHashCode(), "Pickable_Carrot".GetStableHashCode(), "Pickable_SeedCarrot".GetStableHashCode(),
                  "Pickable_Turnip".GetStableHashCode(), "Pickable_SeedTurnip".GetStableHashCode(), "Pickable_Onion".GetStableHashCode(),
                  "Pickable_SeedOnion".GetStableHashCode(), "CloudberryBush".GetStableHashCode(), "Pickable_Mushroom_JotunPuffs".GetStableHashCode(),
                  "Pickable_Mushroom_Magecap".GetStableHashCode(),
                };

                public static void Postfix(Pickable __instance)
                {
                    // Do check here to see if pickable is in the HashSet                    
                    if (IsModEnabled.Value && !PlantsToAutoPick.Contains(__instance.m_nview.m_zdo.m_prefab) && !__instance.m_picked)
                    {
                        __instance.gameObject.AddComponent<ComfyAutoPicker>();
                    }
                }
            }
        }
    }


    internal class ComfyAutoPicker : MonoBehaviour
    {
        private static readonly AccessTools.FieldRef<Pickable, bool> m_picked =
           AccessTools.FieldRefAccess<Pickable, bool>("m_picked");

        private void Awake()
        {
            if (IsModEnabled.Value) {

                InvokeRepeating(nameof(CheckAndPick), 1.0f, 0.5f);
            }
        }

        public void CheckAndPick()
        {

            // both float values should be the same, please do not change one without changing the other.
            if (IsModEnabled.Value && Player.m_localPlayer is not null
                && (transform.position - Player.m_localPlayer.transform.position).sqrMagnitude < 0.8f * 0.8f) 
            {

                List<string> PickableItems = new List<string>()
                { "Pickable_Barley","Pickable_Barley_Wild", "Pickable_Flax", "Pickable_Flax_Wild", "Pickable_Carrot", "Pickable_SeedCarrot",
                  "Pickable_Turnip", "Pickable_SeedTurnip", "Pickable_Onion", "Pickable_SeedOnion", "CloudberryBush", "Pickable_Mushroom_JotunPuffs",
                  "Pickable_Mushroom_Magecap" };

                var pickable = GetComponentInChildren<Pickable>();
                if (m_picked(pickable))
                {
                    return;
                }

                if (!Player.m_localPlayer || ((Player.m_localPlayer.m_rightItem) != null && Player.m_localPlayer.m_rightItem.m_shared.m_name.Contains("$item_cultivator")))
                {
                    //spam the chat with unable to pick messages when holding a Cultivator
                    Chat.m_instance.AddString($"You cannot pick plants when you have a Cultivator Equipped");
                    return;
                }

                if (PickableItems.Contains(Utils.GetPrefabName(pickable.name)) && !PrivateArea.CheckAccess(pickable.transform.position, 0f, true, false))
                {
                    Chat.m_instance.AddString($"You are not on the ward for this area");
                    return;
                }

                if (PickableItems.Contains(Utils.GetPrefabName(pickable.name)))
                {
                    //If its in the list, go ahead and pick it.
                    pickable.Interact(Player.m_localPlayer, false, false);
                }
            }
        }
    }
}