using HarmonyLib;
using Jotunn.Configs;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class FastAttackSpeed
{
    [HarmonyPatch(typeof(CharacterAnimEvent), "CustomFixedUpdate")]
    private static class FastAttackSpeed_CharacterAnimEvent_CustomFixedUpdate_Patch
    {
        private static void Prefix(Character ___m_character, ref Animator ___m_animator)
        {
            if (!___m_character.InAttack() || ___m_character.IsPlayer())
            {
                return;
            }
            
            var modiferComponent = ___m_character.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FastAttackSpeed))
            {
                Debug.Log("Monster has fast attack speed");
                double currentAttackSpeed = ___m_animator.speed * 10000000.0 % 100.0;
                float attackSpeedModifier = 0.25f;
                if ((!(currentAttackSpeed < 30.0) || !(currentAttackSpeed > 10.0)) && !(___m_animator.speed <= 0.001f))
                {
                    ___m_animator.speed = ___m_animator.speed * (1f + attackSpeedModifier) + 1.9E-06f;
                }
            }
            
        }
    }
}