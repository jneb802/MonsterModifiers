using System;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace MonsterModifiers.Modifiers;

public class RemoveStatus
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

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.RemoveStatus))
            {
                return;
            }

            List<StatusEffect> playerStatusEffects = __instance.GetSEMan().GetStatusEffects();
            if (playerStatusEffects.Count > 1)
            {
                int randomIndex = Random.Range(0, playerStatusEffects.Count);
                __instance.GetSEMan().RemoveStatusEffect(playerStatusEffects[randomIndex]);
            }
        }
    }
}