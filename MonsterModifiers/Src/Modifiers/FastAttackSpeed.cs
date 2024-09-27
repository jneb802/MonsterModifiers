using System.Security.Cryptography.X509Certificates;
using HarmonyLib;
using Jotunn.Configs;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class FastAttackSpeed
{
    [HarmonyPatch(typeof(CharacterAnimEvent), nameof(CharacterAnimEvent.CustomFixedUpdate))]
    public static class FastAttackSpeed_CharacterAnimEvent_CustomFixedUpdate_Patch
    {
        public static void Prefix(Character ___m_character, ref Animator ___m_animator)
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
                float attackSpeedModifier = 0.5f;
                if ((!(currentAttackSpeed < 30.0) || !(currentAttackSpeed > 10.0)) && !(___m_animator.speed <= 0.001f))
                {
                    ___m_animator.speed = ___m_animator.speed * (1f + attackSpeedModifier) + 1.9E-06f;
                }
            }
        }
    }
    
    [HarmonyPatch(typeof(Attack), nameof(Attack.Start))]
    public static class FastAttackSpeed_Attack_Start_Patch
    {
        private static void Postfix(Humanoid character, ItemDrop.ItemData weapon, bool __result)
        {
            if (!character.InAttack() || character.IsPlayer())
            {
                return;
            }
            
            var modiferComponent = character.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FastAttackSpeed))
            {
                if (!character.IsPlayer() && __result && !character.IsBoss())
                {
                    float speedModifier = 0.5f;
                    weapon.m_lastAttackTime -= weapon.m_shared.m_aiAttackInterval * Mathf.Max(0f, speedModifier);
                }
            }
        }
    }
}