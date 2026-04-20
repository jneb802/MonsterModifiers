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

             List<Character> characters = WorldUtils.GetAllCharacter(__instance.transform.position,5f);
             foreach (var character in characters)
             {
                 if (character == null || character.m_nview == null || character.m_nview.GetZDO() == null || !character.m_nview.GetZDO().IsOwner())
                 {
                     continue;
                 }
                 
                 var modiferComponent = character.GetComponent<MonsterModifier>(); 
                 if (modiferComponent != null && modiferComponent.Modifiers.Contains(MonsterModifierTypes.SoulEater))
                 {
                     if (character.m_nview.GetZDO().GetInt("MM_soulEaterCount") < 3)
                     {
                         // Debug.Log("Monster with name " + character.m_name + " has been incremented via soulEater");
                         
                         int soulEaterCount = character.m_nview.GetZDO().GetInt("MM_soulEaterCount") + 1;
                         character.m_nview.GetZDO().Set("MM_soulEaterCount",soulEaterCount);
                         
                         float growthMult = 1f + MonsterModifiersPlugin.Cfg_SoulEater_GrowthPerStack.Value / 100f;
                         character.transform.localScale *= growthMult;
                         Physics.SyncTransforms();

                         character.m_health *= growthMult;
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
             
             if (!ModifierUtils.RunHitChecks(hit, true))
             {
                 return;
             }

             Character attacker = hit.GetAttacker();

             var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
             if (modiferComponent == null)
             {
                 return;
             }

             if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.SoulEater))
             {
                 int soulEaterCount = attacker.m_nview.GetZDO().GetInt("MM_soulEaterCount");
                 if (soulEaterCount > 0)
                 {
                     float growthPct = MonsterModifiersPlugin.Cfg_SoulEater_GrowthPerStack.Value / 100f;
                     float damageModifier = 1f + growthPct * Mathf.Clamp(soulEaterCount, 1, 3);
                     hit.ApplyModifier(damageModifier);
                 }
             }
         }
     }
}