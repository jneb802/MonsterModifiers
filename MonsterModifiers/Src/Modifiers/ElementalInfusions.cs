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

            var attacker = hit.GetAttacker();
            if (attacker.IsPlayer() || attacker == null)
            {
                return;
            }

            var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.PoisonInfused))
            {
                hit.m_damage.m_poison += hit.GetTotalPhysicalDamage();
                Debug.Log("Hit has additional poison damage added. Amount is: " + hit.m_damage.m_poison);
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FireInfused))
            {
                hit.m_damage.m_fire += hit.GetTotalPhysicalDamage();
                Debug.Log("Hit has additional fire damage added. Amount is: " + hit.m_damage.m_fire);
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.LightningInfused))
            {
                hit.m_damage.m_lightning += hit.GetTotalPhysicalDamage();
                Debug.Log("Hit has additional lightning damage added. Amount is: " + hit.m_damage.m_lightning);
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FrostInfused))
            {
                hit.m_damage.m_frost += hit.GetTotalPhysicalDamage();
                Debug.Log("Hit has additional frost damage added. Amount is: " + hit.m_damage.m_frost);
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.SpiritInfused))
            {
                hit.m_damage.m_spirit += hit.GetTotalPhysicalDamage();
                Debug.Log("Hit has additional spirit damage added. Amount is: " + hit.m_damage.m_spirit);
            }
        }
    }
}