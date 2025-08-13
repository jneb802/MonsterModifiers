using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using Jotunn.Extensions;

namespace MonsterModifiers.Config;
public class ModifierConfigLogic
{
    ConfigFile _configFile;
    Dictionary<MonsterModifierTypes, ConfigEntry<int>> modifierWeights;

    static ModifierConfigLogic? _instance;

    public static ModifierConfigLogic Instance
    {
        get{
            if (_instance == null)
            {
                _instance = new ModifierConfigLogic();
            }
            return _instance;
        }
    }

    public void InitConfig(ConfigFile configFile)
    {
        this._configFile = configFile;
        InitConfigParams();
    }

    void InitConfigParams()
    {
        foreach (MonsterModifierTypes modifierType in Enum.GetValues(typeof(MonsterModifierTypes)))
        {
            ConfigEntry<int> modifierEntry = _configFile.BindConfig("Modifier Weights",
                modifierType.ToString(),
                1,
                "The liklihood (between 0.0 and 1.0) of if the modifier will be applied to monsters.",
                true);
            modifierWeights.Add(modifierType, modifierEntry);
            
        }
    }

    public Dictionary<MonsterModifierTypes, ModifierData> InitCustomConfigModifiers(
        Dictionary<MonsterModifierTypes, ModifierData> defaultModifiers)
    {
        foreach (var modiferEntry in modifierWeights)
        {
            if (!defaultModifiers.TryGetValue(modiferEntry.Key, out ModifierData defaultData))
            {
                continue;
            }
            defaultData.weight = modiferEntry.Value.Value;
        }

        return defaultModifiers;
    }
}
