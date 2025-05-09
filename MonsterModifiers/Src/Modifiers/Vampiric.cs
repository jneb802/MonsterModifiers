using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class Vampiric
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class Vampiric_Character_RPC_Damage_Patch
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
            
            if (__instance.IsBlocking())
            {
                return;
            }

            var attacker = hit.GetAttacker();
            var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            float addedDamage = hit.GetTotalDamage() * 0.5f;
            float nonPlayerDamage = hit.m_damage.m_chop + hit.m_damage.m_pickaxe + hit.m_damage.m_spirit;
            float finalDamage = addedDamage - nonPlayerDamage;
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.Vampiric))
            {
                attacker.Heal(finalDamage);
                // Debug.Log("Hit has additional frost damage added. Amount is: " + hit.m_damage.m_frost);
            }
        }
    }
}