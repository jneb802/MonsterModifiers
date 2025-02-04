using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class ElementalInfusions
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class ElementalInfusions_Character_RPC_Damage_Patch
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
            float finalDamageAdd = addedDamage - nonPlayerDamage;

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.PoisonInfused))
            {
                hit.m_damage.m_poison += finalDamageAdd;
                // Debug.Log("Hit has additional poison damage added. Amount is: " + hit.m_damage.m_poison);
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FireInfused))
            {
                hit.m_damage.m_fire += finalDamageAdd;
                // Debug.Log("Hit has additional fire damage added. Amount is: " + hit.m_damage.m_fire);
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.LightningInfused))
            {
                hit.m_damage.m_lightning += finalDamageAdd;
                // Debug.Log("Hit has additional lightning damage added. Amount is: " + hit.m_damage.m_lightning);
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FrostInfused))
            {
                hit.m_damage.m_frost += finalDamageAdd;
                // Debug.Log("Hit has additional frost damage added. Amount is: " + hit.m_damage.m_frost);
            }
        }
    }
}