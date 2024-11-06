using System.Collections.Generic;
using HarmonyLib;
using MonsterModifiers.StatusEffects;
using UnityEngine;

namespace MonsterModifiers.Modifiers;


public class BloodLoss
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class RemoveMead_Character_RPC_Damage_Patch
    {
        public static void Postfix(Character __instance, HitData hit)
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

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.BloodLoss))
            {
                return;
            }
            
            Debug.Log("All checks passed, ready to apply bloodLoss");

            if (!__instance.GetSEMan().HaveStatusEffect("BloodLossStatusEffect".GetStableHashCode()))
            {
                Debug.Log("Player does not have bloodLoss, now adding status effect");
                __instance.GetSEMan().AddStatusEffect("BloodLossStatusEffect".GetStableHashCode());  
            }
        }
    }
}