using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class Absorption
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class Absorption_Character_RPC_Damage_Patch
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (hit == null || __instance == null)
            {
                return;
            }
            
            var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.Absorption))
            {
                return;
            }
            
            if (hit.m_damage.GetTotalDamage() == 0)
            {
                return;
            }

            HitData.DamageModifiers monsterDamageModifiers = __instance.GetDamageModifiers();
            
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Blunt, hit.m_damage.m_blunt);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Slash, hit.m_damage.m_slash);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Pierce, hit.m_damage.m_pierce);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Chop, hit.m_damage.m_chop);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Pickaxe, hit.m_damage.m_pickaxe);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Fire, hit.m_damage.m_fire);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Frost, hit.m_damage.m_frost);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Lightning, hit.m_damage.m_lightning);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Poison, hit.m_damage.m_poison);
            HealIfImmune(__instance, monsterDamageModifiers, HitData.DamageType.Spirit, hit.m_damage.m_spirit);
        }

        public static void HealIfImmune(Character character, HitData.DamageModifiers modifiers, HitData.DamageType type, float damage)
        {
            if (damage > 0 && IsDamageTypeImmune(modifiers, type))
            {
                // Debug.Log("Monster is immune to damage of type: " + type + "healing monster for amount: " + damage);
                character.Heal(damage);
            }
        }

        public static bool IsDamageTypeImmune(HitData.DamageModifiers modifiers, HitData.DamageType type)
        {
            return type switch
            {
                HitData.DamageType.Blunt => modifiers.m_blunt == HitData.DamageModifier.Immune,
                HitData.DamageType.Slash => modifiers.m_slash == HitData.DamageModifier.Immune,
                HitData.DamageType.Pierce => modifiers.m_pierce == HitData.DamageModifier.Immune,
                HitData.DamageType.Chop => modifiers.m_chop == HitData.DamageModifier.Immune,
                HitData.DamageType.Pickaxe => modifiers.m_pickaxe == HitData.DamageModifier.Immune,
                HitData.DamageType.Fire => modifiers.m_fire == HitData.DamageModifier.Immune,
                HitData.DamageType.Frost => modifiers.m_frost == HitData.DamageModifier.Immune,
                HitData.DamageType.Lightning => modifiers.m_lightning == HitData.DamageModifier.Immune,
                HitData.DamageType.Poison => modifiers.m_poison == HitData.DamageModifier.Immune,
                HitData.DamageType.Spirit => modifiers.m_spirit == HitData.DamageModifier.Immune,
                _ => false,
            };
        }
    }
    
}