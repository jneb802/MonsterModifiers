using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

// 시너지: 관통+베기+둔기 저항 3종 + FastMovement = "철갑 돌격" 타입
// 효과: 이동속도 추가 +30%, 공격 시 데미지 +50%
public class ArmorCharge
{
    private const float ExtraSpeedMult = 1.3f;
    private const float ExtraDamageMult = 1.5f;

    public static bool IsArmorChargeType(Custom_Components.MonsterModifier modifier)
    {
        return modifier.Modifiers.Contains(MonsterModifierTypes.PierceImmunity)
            && modifier.Modifiers.Contains(MonsterModifierTypes.SlashImmunity)
            && modifier.Modifiers.Contains(MonsterModifierTypes.BluntImmunity)
            && modifier.Modifiers.Contains(MonsterModifierTypes.FastMovement);
    }

    public static void ApplyArmorCharge(Character character)
    {
        character.m_speed *= ExtraSpeedMult;
        character.m_runSpeed *= ExtraSpeedMult;
        character.m_walkSpeed *= ExtraSpeedMult;
    }

    // 철갑 돌격 타입 몬스터의 공격 데미지 +50%
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class ArmorCharge_Character_RPC_Damage_Patch
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (!ModifierUtils.RunRPCDamageChecks(__instance, hit))
                return;

            if (!ModifierUtils.RunHitChecks(hit, true))
                return;

            var attacker = hit.GetAttacker();
            var modiferComponent = attacker?.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
                return;

            if (!IsArmorChargeType(modiferComponent))
                return;

            hit.m_damage.m_blunt *= ExtraDamageMult;
            hit.m_damage.m_slash *= ExtraDamageMult;
            hit.m_damage.m_pierce *= ExtraDamageMult;
            hit.m_damage.m_fire *= ExtraDamageMult;
            hit.m_damage.m_frost *= ExtraDamageMult;
            hit.m_damage.m_lightning *= ExtraDamageMult;
        }
    }
}
