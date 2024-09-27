using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class ShieldBreaker
{
    [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
    public class ShieldBreaker_Humanoid_BlockAttack_Patch
    {
        public static void Postfix(Humanoid __instance, HitData hit, Character attacker)
        {
            if (hit == null || __instance == null || attacker == null)
            {
                return;
            }

            if (attacker.IsPlayer())
            {
                return;
            }

            var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.ShieldBreaker))
            {
                return;
            }
            
            ItemDrop.ItemData shield = __instance.GetCurrentBlocker();
            if (shield != null)
            {
                shield.m_durability *= 0.5f;
            }
        }
    }
}