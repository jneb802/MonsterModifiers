using System.Collections.Generic;
using Jotunn.Utils;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MonsterModifiers;

public enum MonsterModifierTypes
{
    StaminaSiphon,
    EitrSiphon,
    ShieldBreaker,
    FoodDrain,
    IgnoreArmor,
    PoisonDeath,
    FrostDeath,
    FireDeath,
    HealDeath,
    PersonalShield,
    ShieldDome,
    SoulEater,
    RemoveStatusEffect,
    StaggerImmune,
    FireInfused,
    PoisonInfused,
    FrostInfused,
    LightningInfused,
    ElementalImmunity,
    PhysicalImmunity,
    FastMovement,
    FastAttackSpeed,
    DistantDetection,
    BloodLoss
}

public class ModifierData
{
    public int weight { get; set; }
    public List<float> color { get; set; }
}

public class ModifierUtils
{
    public static Dictionary<MonsterModifierTypes, ModifierData> modifiers;

    public static Color GetModifierColor(MonsterModifierTypes modifier)
    {
        List<float> rgb = modifiers[modifier].color;
        Color color = new Color(rgb[0], rgb[1], rgb[2], rgb[3]);

        return color;
    }

    public static Sprite GetModifierIcon(MonsterModifierTypes modifier)
    {
        
        if (modifier == MonsterModifierTypes.FireInfused ||
            modifier == MonsterModifierTypes.FrostInfused ||
            modifier == MonsterModifierTypes.PoisonInfused ||
            modifier == MonsterModifierTypes.LightningInfused ||
            modifier == MonsterModifierTypes.RemoveStatusEffect)
        {
            return ModifierAssetUtils.swordIcon;
        }
        
        if (modifier == MonsterModifierTypes.ShieldBreaker ||
            modifier == MonsterModifierTypes.IgnoreArmor)
        {
            return ModifierAssetUtils.shieldBrokenIcon;
        }
        
        if (modifier == MonsterModifierTypes.StaminaSiphon ||
            modifier == MonsterModifierTypes.EitrSiphon)
        {
            return ModifierAssetUtils.potionIcon;
        }
        
        if (modifier == MonsterModifierTypes.FoodDrain)
        {
            return ModifierAssetUtils.appleIcon;
        }
        
        if (modifier == MonsterModifierTypes.PoisonDeath ||
            modifier == MonsterModifierTypes.FireDeath ||
            modifier == MonsterModifierTypes.FrostDeath ||
            modifier == MonsterModifierTypes.HealDeath)
        {
            return ModifierAssetUtils.skullIcon;
        }
        
        if (modifier == MonsterModifierTypes.ElementalImmunity ||
            modifier == MonsterModifierTypes.PhysicalImmunity ||
            modifier == MonsterModifierTypes.StaggerImmune)
        {
            return ModifierAssetUtils.shieldIcon;
        }
        
        if (modifier == MonsterModifierTypes.PersonalShield ||
            modifier == MonsterModifierTypes.ShieldDome)

        {
            return ModifierAssetUtils.circleIcon;
        }
        
        if (modifier == MonsterModifierTypes.SoulEater)

        {
            return ModifierAssetUtils.soulIcon;
        }
        
        if (modifier == MonsterModifierTypes.FastAttackSpeed ||
            modifier == MonsterModifierTypes.FastMovement ||
            modifier == MonsterModifierTypes.DistantDetection)

        {
            return ModifierAssetUtils.plusSquareIcon;
        }
        
        if (modifier == MonsterModifierTypes.BloodLoss)

        {
            return ModifierAssetUtils.bloodIcon;
        }
        
        Debug.Log("Could not find icon for modifier");
        return ModifierAssetUtils.plusSquareIcon;;
    }

    public static int GetModifierWeight(MonsterModifierTypes modifier)
    {
        return modifiers[modifier].weight;
    }

    public static List<MonsterModifierTypes> RollRandomModifiers(int numModifiers)
    {
        List<MonsterModifierTypes> selectedModifiers = new List<MonsterModifierTypes>();
        Dictionary<MonsterModifierTypes, ModifierData> availableModifiers =
            new Dictionary<MonsterModifierTypes, ModifierData>(modifiers);

        for (int i = 0; i < numModifiers; i++)
        {
            int totalWeight = 0;

            foreach (var modifier in availableModifiers.Values)
            {
                totalWeight += modifier.weight;
            }

            int randomValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeWeight = 0;

            MonsterModifierTypes selected = MonsterModifierTypes.StaminaSiphon;

            foreach (var entry in availableModifiers)
            {
                cumulativeWeight += entry.Value.weight;
                if (randomValue < cumulativeWeight)
                {
                    selected = entry.Key;
                    break;
                }
            }

            selectedModifiers.Add(selected);
            availableModifiers.Remove(selected);
            if (selected == MonsterModifierTypes.ElementalImmunity)
            {
                availableModifiers.Remove(MonsterModifierTypes.PhysicalImmunity);
            }
            if (selected == MonsterModifierTypes.PhysicalImmunity)
            {
                availableModifiers.Remove(MonsterModifierTypes.ElementalImmunity);
            }
        }

        return selectedModifiers;
    }

    public static void RunBiomeChecks()
    {

    }

    public static bool RunRPCDamageChecks(Character character, HitData hit)
    {
        if (hit == null || character == null)
        {
            return false;
        }

        // This avoids any hits that have no damage
        if (hit.m_damage.GetTotalDamage() == 0)
        {
            return false;
        }

        // This avoids all non-enemy-attacking-player hits. Ex: fall damage
        if (hit.m_hitType != HitData.HitType.EnemyHit)
        {
            return false;
        }

        return true;
    }

    public static bool RunHitChecks(HitData hit, bool isMonsterAttacking)
    {
        Character attacker = hit.GetAttacker();
        if (attacker == null)
        {
            return false;
        }

        if (isMonsterAttacking)
        {
            if (attacker.IsPlayer())
            {
                return false;
            }
        }

        if (!isMonsterAttacking)
        {
            if (attacker.IsPlayer())
            {
                return true;
            }
        }
        return true;
    }
}






