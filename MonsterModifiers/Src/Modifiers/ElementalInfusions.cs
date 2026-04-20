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

            float totalDamage = hit.GetTotalDamage();
            float nonPlayerDamage = hit.m_damage.m_chop + hit.m_damage.m_pickaxe + hit.m_damage.m_spirit;

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.PoisonInfused))
            {
                hit.m_damage.m_poison += Mathf.Max(0f, totalDamage * (MonsterModifiersPlugin.Cfg_PoisonInfused_DamagePercent.Value / 100f) - nonPlayerDamage);
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FireInfused))
            {
                hit.m_damage.m_fire += Mathf.Max(0f, totalDamage * (MonsterModifiersPlugin.Cfg_FireInfused_DamagePercent.Value / 100f) - nonPlayerDamage);
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.LightningInfused))
            {
                hit.m_damage.m_lightning += Mathf.Max(0f, totalDamage * (MonsterModifiersPlugin.Cfg_LightningInfused_DamagePercent.Value / 100f) - nonPlayerDamage);
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FrostInfused))
            {
                hit.m_damage.m_frost += Mathf.Max(0f, totalDamage * (MonsterModifiersPlugin.Cfg_FrostInfused_DamagePercent.Value / 100f) - nonPlayerDamage);
            }
        }
    }
}