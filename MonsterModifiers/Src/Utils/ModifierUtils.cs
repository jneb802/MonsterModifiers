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
    PersonalShield,
    ShieldDome,
    SoulEater,
    RemoveStatus,
    StaggerImmune,
    FireInfused,
    PoisonInfused,
    FrostInfused,
    SpiritInfused,
    LightningInfused,
    AddElementalImmunity,
    AddPhysicalImmunity,
    FastMovement,
    FastAttackSpeed
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
        }

        return selectedModifiers;
    }

    public static bool RunRPCDamageChecks(Character character, HitData hit)
    {
        if (hit == null || character == null)
        {
            return false;
        }

        if (hit.m_damage.GetTotalDamage() == 0)
        {
            return false;
        }

        if (hit.m_hitType != HitData.HitType.EnemyHit)
        {
            return false;
        }

        return true;
    }
}





