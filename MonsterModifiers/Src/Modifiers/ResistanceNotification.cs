using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

// 저항 몬스터 첫 타격/두 번째 타격 시 화면 중앙 메시지 표시, 세 번째부터 표시 없음
public class ResistanceNotification
{
    // 몬스터 ZDOID → 해당 속성별 경고 표시 횟수 (최대 2회)
    private static readonly Dictionary<ZDOID, Dictionary<string, int>> HitWarningCounts = new();

    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class ResistanceNotification_Character_RPC_Damage_Patch
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (hit == null || __instance == null)
                return;

            if (hit.m_damage.GetTotalDamage() == 0)
                return;

            // 피격 대상이 플레이어면 제외 (몬스터가 피격 대상이어야 함)
            if (__instance.IsPlayer())
                return;

            var attacker = hit.GetAttacker();
            if (attacker == null || !attacker.IsPlayer())
                return;

            // 로컬 플레이어가 공격한 경우만 메시지 표시
            if (attacker != Player.m_localPlayer)
                return;

            var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null || modiferComponent.Modifiers.Count == 0)
                return;

            ZDOID zdoid = __instance.GetZDOID();

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.PierceImmunity) && hit.m_damage.m_pierce > 0)
                TryShowWarning(zdoid, "pierce", "이 몬스터는 관통 공격에 강합니다! 참격 또는 둔기 공격을 사용하세요.");

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.SlashImmunity) && hit.m_damage.m_slash > 0)
                TryShowWarning(zdoid, "slash", "이 몬스터는 베기 공격에 강합니다! 관통 또는 둔기 공격을 사용하세요.");

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.BluntImmunity) && hit.m_damage.m_blunt > 0)
                TryShowWarning(zdoid, "blunt", "이 몬스터는 둔기 공격에 강합니다! 관통 또는 베기 공격을 사용하세요.");

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.ElementalImmunity))
            {
                float elemDmg = hit.m_damage.m_fire + hit.m_damage.m_frost + hit.m_damage.m_lightning
                    + hit.m_damage.m_poison + hit.m_damage.m_spirit;
                if (elemDmg > 0)
                    TryShowWarning(zdoid, "elemental", "이 몬스터는 원소 공격에 강합니다! 물리 공격을 사용하세요.");
            }
        }

        private static void TryShowWarning(ZDOID zdoid, string damageKey, string message)
        {
            if (!HitWarningCounts.ContainsKey(zdoid))
                HitWarningCounts[zdoid] = new Dictionary<string, int>();

            var counts = HitWarningCounts[zdoid];
            if (!counts.ContainsKey(damageKey))
                counts[damageKey] = 0;

            if (counts[damageKey] < 2)
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, message);
                counts[damageKey]++;
            }
        }
    }

    // 몬스터 사망 시 카운트 초기화
    [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
    public class ResistanceNotification_Character_OnDeath_Patch
    {
        public static void Prefix(Character __instance)
        {
            if (__instance == null) return;
            ZDOID zdoid = __instance.GetZDOID();
            HitWarningCounts.Remove(zdoid);
        }
    }
}
