using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class DamageModifiers
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class ModiferDamageModifiers_Character_RPC_Damage_Patch
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (hit == null || __instance == null)
                return;

            if (hit.m_damage.GetTotalDamage() == 0)
                return;

            // 몬스터만 적용 (플레이어 제외)
            if (__instance.IsPlayer())
                return;

            var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
                return;

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.PierceImmunity))
                hit.m_damage.m_pierce *= (1f - MonsterModifiersPlugin.Cfg_PierceImmunity_DamageReduction.Value / 100f);

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.SlashImmunity))
                hit.m_damage.m_slash *= (1f - MonsterModifiersPlugin.Cfg_SlashImmunity_DamageReduction.Value / 100f);

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.BluntImmunity))
                hit.m_damage.m_blunt *= (1f - MonsterModifiersPlugin.Cfg_BluntImmunity_DamageReduction.Value / 100f);

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.ElementalImmunity))
            {
                float elemMult = 1f - MonsterModifiersPlugin.Cfg_ElementalImmunity_DamageReduction.Value / 100f;
                hit.m_damage.m_fire *= elemMult;
                hit.m_damage.m_frost *= elemMult;
                hit.m_damage.m_lightning *= elemMult;
                hit.m_damage.m_poison *= elemMult;
                hit.m_damage.m_spirit *= elemMult;
            }
        }
    }
}
