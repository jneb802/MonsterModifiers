using System.Collections.Generic;
using HarmonyLib;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.GamePatches
{
    [HarmonyPatch(typeof(Incinerator), "Awake")]
    public static class Incinerator_Awake_Patch
    {
        static bool Prefix(Incinerator __instance, ref Switch ___m_incinerateSwitch, ref ZNetView ___m_nview)
        {
            // Check if m_incinerateSwitch is null
            if (___m_incinerateSwitch == null)
            {
                Debug.LogError("m_incinerateSwitch is not assigned in Incinerator!");
                return false; // Skip the original Awake method to prevent the NRE
            }

            // Get the ZNetView component
            ___m_nview = __instance.GetComponent<ZNetView>();
            if (___m_nview == null)
            {
                Debug.LogError("ZNetView component is missing in Incinerator!");
                return false; // Skip the original Awake method to prevent the NRE
            }

            // Allow the original Awake method to run
            return true;
        }
    }
    
    [HarmonyPatch(typeof(Incinerator), nameof(Incinerator.OnIncinerate))]
    public class Altar_Incinerator_OnIncinerate_Patch
    {
        public static bool Prefix(Incinerator __instance, Switch sw, Humanoid user, ItemDrop.ItemData item)
        {
            if (IsCustomAltar(__instance))
            {
                MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Player and invoked custom altar");
                ItemDrop.ItemData sigilObject = __instance.m_container.GetInventory().GetItem(0);
                if (sigilObject ==  null)
                {
                    MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Player and invoked custom altar but inventory is empty");
                    return false;
                }
        
                var dungeonGenerator = DungeonGenUtils.GetDungeonInterior(__instance.gameObject);
                
                var creatureSpawners = CreatureSpawnerUtils.GetCreatureSpawnersInDungeon(dungeonGenerator);
                
                SigilComponent sigilComponent = sigilObject.m_dropPrefab.GetComponent<SigilComponent>();
                if (sigilComponent != null)
                {
                    MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Found sigil component in item with name: " + sigilObject.m_dropPrefab);
                    sigilComponent.AddModifier(new Modifier(MonsterModifierTypes.AddPierceResistant));
                    sigilComponent.ApplyAllModifiers(creatureSpawners);
                }
                else
                {
                    MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Failed to find sigil component in item with name: " + sigilObject.m_dropPrefab);
                }
                
                return false;
            }
            return true;
        }

        private static bool IsCustomAltar(Incinerator instance)
        {
            return instance != null && instance.gameObject.name.Contains("DungeonAltar");
        }
    }
}