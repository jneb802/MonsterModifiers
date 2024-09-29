using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class IgnoreArmor
{
    
    public static bool shouldIgnoreArmor = false;
    
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class IgnoreArmor_Character_RPC_Damage_Patch
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

            var attacker = hit.GetAttacker();

            var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.IgnoreArmor))
            {
                // Debug.Log("Ignore amror is true");
                shouldIgnoreArmor = true;
            }
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.GetBodyArmor))]
    public class IgnoreArmor_Player_GetBodyArmor_Patch
    {
        public static void Postfix(ref float __result)
        {
            if (shouldIgnoreArmor)
            {
                __result *= 0.5f;
                // Debug.Log("Get body armor has reduced the armor");
                shouldIgnoreArmor = false;
            }
        }
    }
}