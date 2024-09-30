// using System;
// using System.Collections.Generic;
// using HarmonyLib;
// using MonsterModifiers.Custom_Components;
// using UnityEngine;
//
// namespace MonsterModifiers.DungeonPatches
// {
//     [HarmonyPatch(typeof(Incinerator), "Awake")]
//     public static class Incinerator_Awake_Patch
//     {
//         static bool Prefix(Incinerator __instance, ref Switch ___m_incinerateSwitch, ref ZNetView ___m_nview)
//         {
//             if (___m_incinerateSwitch == null)
//             {
//                 Debug.LogError("m_incinerateSwitch is not assigned in Incinerator!");
//                 return false;
//             }
//             
//             ___m_nview = __instance.GetComponent<ZNetView>();
//             if (___m_nview == null)
//             {
//                 Debug.LogError("ZNetView component is missing in Incinerator!");
//                 return false; 
//             }
//             
//             return true;
//         }
//     }
//     
//     [HarmonyPatch(typeof(Incinerator), nameof(Incinerator.OnIncinerate))]
//     public class Altar_Incinerator_OnIncinerate_Patch
//     {
//         public static bool Prefix(Incinerator __instance, Switch sw, Humanoid user, ItemDrop.ItemData item)
//         {
//             if (IsCustomAltar(__instance))
//             {
//                 MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Player and invoked custom altar");
//                 ItemDrop.ItemData sigilObject = __instance.m_container.GetInventory().GetItem(0);
//                 if (sigilObject ==  null)
//                 {
//                     MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Player and invoked custom altar but inventory is empty");
//                     return false;
//                 }
//         
//                 DungeonGenerator dungeonGenerator = DungeonGenUtils.GetDungeonInterior(__instance.gameObject);
//                 if (dungeonGenerator == null)
//                 {
//                     Debug.Log("Dungeon Generator is null");
//                     return false;
//                 }
//
//                 if (item == null)
//                 {
//                     Debug.Log("Item is null");
//                     return false;
//                 }
//                 
//                 List<MonsterModifierTypes> modifierList = new List<MonsterModifierTypes>();
//                 if (item.m_customData.TryGetValue(Sigil.CustomDataKey, out string serializedModifiers))
//                 {
//                     modifierList = new List<MonsterModifierTypes>(
//                         Array.ConvertAll(serializedModifiers.Split(','), 
//                             str => (MonsterModifierTypes)Enum.Parse(typeof(MonsterModifierTypes), str))
//                     );
//                 }
//                 
//                 List<Character> interiorCharactersList = WorldUtils.GetAllCharacter(dungeonGenerator.transform.position, 64f);
//                 if (interiorCharactersList == null)
//                 {
//                     Debug.Log("interiorCharacterList is null");
//                     return false;
//                 }
//                 
//                 if (interiorCharactersList.Count > 0)
//                 {
//                     foreach (var character in interiorCharactersList)
//                     {
//                         character.SetLevel(modifierList.Count);
//                         MonsterModifier monsterModifier = character.GetComponentInParent<MonsterModifier>();
//                         if (monsterModifier != null)
//                         {
//                             monsterModifier.ChangeModifiers(modifierList, modifierList.Count);
//                         }
//                         
//                     }
//                 }
//                 return true;
//             }
//             return true;
//         }
//
//         private static bool IsCustomAltar(Incinerator instance)
//         {
//             return instance != null && instance.gameObject.name.Contains("DungeonAltar");
//         }
//     }
// }