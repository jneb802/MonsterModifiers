using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class ModiferDamageModifiers
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class ModiferDamageModifiers_Character_RPC_Damage_Patch
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (!ModifierUtils.RunRPCDamageChecks(__instance,hit))
            {
                return;
            }

            var attacker = hit.GetAttacker();
            if (!attacker.IsPlayer() || attacker == null)
            {
                return;
            }
            
            var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.AddPhysicalImmunity))
            {
                __instance.m_damageModifiers.m_blunt = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_pierce = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_slash = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_chop = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_pickaxe = HitData.DamageModifier.Immune;
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.AddElementalImmunity))
            {
                __instance.m_damageModifiers.m_fire = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_frost = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_lightning = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_poison = HitData.DamageModifier.Immune;
                __instance.m_damageModifiers.m_spirit = HitData.DamageModifier.Immune;
            }
        }
    }
}