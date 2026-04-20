using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class Absorption
{
    [HarmonyPriority(Priority.High)]
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class Absorption_Character_RPC_Damage_Patch
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (hit == null || __instance == null)
                return;

            var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
                return;

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.Absorption))
                return;

            if (hit.m_damage.GetTotalDamage() == 0)
                return;

            float healRatio = MonsterModifiersPlugin.Cfg_Absorption_HealPercent.Value / 100f;

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.BluntImmunity) && hit.m_damage.m_blunt > 0)
                __instance.Heal(hit.m_damage.m_blunt * healRatio);

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.SlashImmunity) && hit.m_damage.m_slash > 0)
                __instance.Heal(hit.m_damage.m_slash * healRatio);

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.PierceImmunity) && hit.m_damage.m_pierce > 0)
                __instance.Heal(hit.m_damage.m_pierce * healRatio);

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.ElementalImmunity))
            {
                float elementalHeal = (hit.m_damage.m_fire + hit.m_damage.m_frost + hit.m_damage.m_lightning
                    + hit.m_damage.m_poison + hit.m_damage.m_spirit) * healRatio;
                if (elementalHeal > 0)
                    __instance.Heal(elementalHeal);
            }
        }
    }
}
