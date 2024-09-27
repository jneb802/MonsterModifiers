using System;
using System.Collections.Generic;
using HarmonyLib;
using Jotunn.Managers;
using MonsterModifiers.Custom_Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MonsterModifiers.Modifiers;

public class SoulEater : MonoBehaviour
{
    public static List<Character> GetAllCharacter(Vector3 position, float range)
    {
        Collider[] hits = Physics.OverlapBox(position, Vector3.one * range, Quaternion.identity);
        List<Character> characters = new List<Character>();

        foreach (var hit in hits)
        {
            var npc = hit.transform.root.gameObject.GetComponentInChildren<Character>();
            if (npc != null)
            {
                characters.Add(npc);
            }
        }

        return characters;
    }
    
     [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
     public class SoulEater_Character_OnDeath_Patch
     {
         public static void Prefix(Character __instance)
         {
             if (__instance == null || __instance.IsPlayer())
             {
                 return;  
             }

             if (__instance.TryGetComponent(out MonsterModifier monsterModifier))
             {
                 if (monsterModifier.Modifiers.Contains(MonsterModifierTypes.SoulEater))
                 {
                     return;
                 }
             }

             List<Character> characters = GetAllCharacter(__instance.transform.position,5f);
             foreach (var character in characters)
             {
                 if (character != null)
                 {
                     var modiferComponent = character.GetComponent<MonsterModifier>(); 
                     if (modiferComponent != null && modiferComponent.Modifiers.Contains(MonsterModifierTypes.SoulEater) && character.m_nview.GetZDO().IsOwner())
                     {
                         if (character.m_nview.GetZDO().GetInt("MM_soulEaterCount") < 3)
                         {
                             int soulEaterCount = character.m_nview.GetZDO().GetInt("MM_soulEaterCount") + 1;
                             character.m_nview.GetZDO().Set("MM_soulEaterCount",soulEaterCount);
                         
                             character.transform.localScale *= 1.1f;
                             Physics.SyncTransforms();

                             character.m_health *= 1.1f;
                             
                             Debug.Log("Monster with name " + __instance.m_name + " has been incremented via soulEater");
                         }
                     }
                 }
             }
         }
     }
     
     [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
     public class SoulEater_Character_RPC_Damage_Patch
     {
         public static void Prefix(Character __instance, HitData hit)
         {
             if (!ModifierUtils.RunRPCDamageChecks(__instance,hit))
             {
                 return;
             }

             Character attacker = hit.GetAttacker();
             if (attacker.IsPlayer() || attacker == null)
             {
                 return;
             }

             var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
             if (modiferComponent == null)
             {
                 return;
             }

             if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.SoulEater))
             {
                 if (attacker.m_nview.GetZDO().GetInt("MM_soulEaterCount") > 0)
                 {
                     int soulEaterCount = attacker.m_nview.GetZDO().GetInt("MM_soulEaterCount");
                     float baseDamage = hit.GetTotalDamage();
                     float finalDamage = baseDamage;
                     switch (soulEaterCount)
                     {
                         case 1:
                             hit.ApplyModifier(1.1f);
                             finalDamage = baseDamage * 1.1f;
                             Debug.Log($"SoulEater modifier applied: 1.1. Final damage: {finalDamage}");
                             break;
                         case 2:
                             hit.ApplyModifier(1.2f);
                             finalDamage = baseDamage * 1.2f;
                             Debug.Log($"SoulEater modifier applied: 1.2. Final damage: {finalDamage}");
                             break;
                         case 3:
                             hit.ApplyModifier(1.3f);
                             finalDamage = baseDamage * 1.3f;
                             Debug.Log($"SoulEater modifier applied: 1.3. Final damage: {finalDamage}");
                             break;
                         default:
                             // Handle cases where soulEaterCount is outside the range 1-3
                             hit.ApplyModifier(1.0f); // No modifier or a default modifier
                             finalDamage = baseDamage * 1.0f;
                             Debug.Log($"SoulEater modifier applied: 1.0 (default). Final damage: {finalDamage}");
                             break;
                     }
                 }
             }
         }
     }
}