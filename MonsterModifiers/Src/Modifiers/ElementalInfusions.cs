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

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.PoisonInfused))
            {
                return;
            }
            
            hit.m_damage.m_poison += hit.GetTotalPhysicalDamage();
            Debug.Log("Hit has additional poison damage added. Amount is: " + hit.m_damage.m_poison);
            
        }
    }
}