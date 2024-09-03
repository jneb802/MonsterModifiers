using System;
using System.Collections.Generic;
using System.Linq;
using Jotunn.Utils;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MonsterModifiers;

public class ModifierConfigData
{
    public List<string> rarities { get; set; }
    public int weight { get; set; }
    public bool hasValues { get; set; }
    public double increment { get; set; }
    public Dictionary<string, List<double>> values { get; set; }
    public string displayName { get; set; }
}

public class ModiferUtils
{
    public static Dictionary<string, ModifierConfigData> ModifierConfigs = new Dictionary<string, ModifierConfigData>();
    
    public static void InitializeModifiers()
    {
        string yamlContent = AssetUtils.LoadTextFromResources("modifierValues.yml");
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        
        var modifierConfigsFromYaml = deserializer.Deserialize<Dictionary<string, ModifierConfigData>>(yamlContent);
        
        ModifierConfigs.Clear();
        
        foreach (var modifierConfig in modifierConfigsFromYaml)
        {
            string modifierKey = modifierConfig.Key;
            ModifierConfigData modifierConfigData = modifierConfig.Value;
        
            ModifierConfigs[modifierKey] = modifierConfigData;
            
            Debug.Log($"Modifier added: {modifierKey} with data: {modifierConfigData}");
        }
    }
    
    public static string GetModifierDisplayName(MonsterModifierTypes modifierType)
    {
        string modifierKey = modifierType.ToString();

        if (ModifierConfigs.TryGetValue(modifierKey, out var modifierData))
        {
            return modifierData.displayName ?? "Unknown Modifier";
        }

        return "Modifier Not Found";
    }

    public static List<Modifier> SetModifiers(ModifierRarity rarity)
    {
        var selectedModifiers = new List<Modifier>();
        var availableModifiers = ModifierConfigs
            .Where(config => config.Value.rarities.Contains(rarity.ToString()))
            .ToList();
        var modifierCount = GetRarityModifierCount(rarity);

        if (availableModifiers.Count == 0)
        {
            // No available modifiers for the given rarity
            return selectedModifiers;
        }

        for (int i = 0; i < modifierCount; i++)
        {
            // Select a modifier based on its weight
            var selectedConfig = GetWeightedRandomModifier(availableModifiers);
            var modifierType = (MonsterModifierTypes)System.Enum.Parse(typeof(MonsterModifierTypes), selectedConfig.Key);

            // Set the modifier value using the SetModifierValue method
            float modifierValue = SetModifierValue(modifierType, rarity);

            // Create a new Modifier instance and add it to the list
            var modifier = new Modifier(modifierType, modifierValue);

            selectedModifiers.Add(modifier);

            // Remove the selected modifier to avoid duplicate selections if necessary
            availableModifiers.Remove(selectedConfig);
        }

        return selectedModifiers;
    }

    private static KeyValuePair<string, ModifierConfigData> GetWeightedRandomModifier(List<KeyValuePair<string, ModifierConfigData>> availableModifiers)
    {
        int totalWeight = availableModifiers.Sum(modifier => modifier.Value.weight);
        int randomWeight = UnityEngine.Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var modifier in availableModifiers)
        {
            currentWeight += modifier.Value.weight;
            if (randomWeight < currentWeight)
            {
                return modifier;
            }
        }

        // Fallback in case no modifier was selected (shouldn't happen)
        return availableModifiers.Last();
    }
        
    public static float SetModifierValue(MonsterModifierTypes modifierType, ModifierRarity rarity)
    {
        string modifierKey = modifierType.ToString();
        
        if (ModifierConfigs.TryGetValue(modifierKey, out var modifierData))
        {
            if (modifierData.hasValues)
            {
                string rarityKey = rarity.ToString();
                
                if (modifierData.values != null && modifierData.values.TryGetValue(rarityKey, out var valuesList))
                {
                    if (valuesList.Count == 2)
                    {
                        double minValue = valuesList[0];
                        double maxValue = valuesList[1];
                        
                        float randomValue = UnityEngine.Random.Range((float)minValue, (float)maxValue);
                        
                        return randomValue;
                    }
                    else
                    {
                        Debug.LogWarning($"Values list for rarity '{rarityKey}' does not contain exactly 2 elements.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Values for rarity '{rarityKey}' not found or values are not initialized.");
                }
            }
            else
            {
                // Handle cases where `hasValues` is false
                return 0f; // Default value or specific logic if needed
            }
        }
        else
        {
            Debug.LogWarning($"Modifier key '{modifierKey}' not found in ModifierConfigs.");
        }

        return 0f; // Return a default value or handle the error as needed
    }

    public static int GetRarityModifierCount(ModifierRarity rarity)
    {
        return 2 + (int)rarity;
    }
    
    public static ModifierRarity GetSigilRarityFromName(string prefabName)
    {
        switch (prefabName)
        {
            case "Sigil_magic_warp":
                return ModifierRarity.Magic;

            case "Rare Sigil":
                return ModifierRarity.Rare;

            case "Epic Sigil":
                return ModifierRarity.Epic;

            case "Legendary Sigil":
                return ModifierRarity.Legendary;

            case "Mythic Sigil":
                return ModifierRarity.Mythic;

            default:
                Debug.LogError($"Unknown prefab name: {prefabName}");
                throw new ArgumentException($"Unknown prefab name: {prefabName}");
        }
    }

}

