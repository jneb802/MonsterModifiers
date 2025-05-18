using System;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace MonsterModifiers.Modifiers;

public class Wet
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class Wet_Character_RPC_Damage_Patch
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

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.Wet))
            {
                return;
            }
            
            __instance.GetSEMan().AddStatusEffect("Wet".GetStableHashCode());
        }
    }
}