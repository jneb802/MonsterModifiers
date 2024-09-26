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
    AddElementalImmunity,
    AddPhysicalImmunity,
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
        Dictionary<MonsterModifierTypes, ModifierData> availableModifiers = new Dictionary<MonsterModifierTypes, ModifierData>(modifiers);

        for (int i = 0; i < numModifiers; i++)
        {
            int totalWeight = 0;
        
            // Calculate the total weight of available modifiers
            foreach (var modifier in availableModifiers.Values)
            {
                totalWeight += modifier.weight;
            }

            // Select a random value within the total weight range
            int randomValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeWeight = 0;

            MonsterModifierTypes selected = MonsterModifierTypes.StaminaSiphon; // Fallback
            
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

}





