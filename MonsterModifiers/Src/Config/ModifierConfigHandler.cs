using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using Jotunn.Extensions;

namespace MonsterModifiers.Config;

/// <summary>
/// Handler for the values determined by the Bepinex Config for this mod. 
/// </summary>
public class ModifierConfigHandler
{
    ConfigFile _configFile;
    Dictionary<MonsterModifierTypes, ConfigEntry<int>> configModifierWeights = new();

    static ModifierConfigHandler? _instance;

    /// <summary>
    /// Gets the singleton instance of the Handler.
    /// </summary>
    public static ModifierConfigHandler Instance
    {
        get{
            if (_instance == null)
            {
                _instance = new ModifierConfigHandler();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Initializes the config by setting the ConfigFile of the mod, allowing for calling of the config API methods.
    /// </summary>
    /// <param name="configFile">The Config File the handler will be responsible for.</param>
    public void InitConfig(ConfigFile configFile)
    {
        this._configFile = configFile;
        InitConfigParams();
    }

    /// <summary>
    /// Initializes the weighting values of all the Monster Modifiers in the Mod. The intention is that, regardless of
    /// how many modifiers are added to the mod, the config will dynamically add the new modifiers to the weighting
    /// section.
    /// </summary>
    void InitConfigParams()
    {
        foreach (MonsterModifierTypes modifierType in Enum.GetValues(typeof(MonsterModifierTypes)))
        {
            ConfigEntry<int> modifierEntry = _configFile.BindConfig("Modifier Weights",
                modifierType.ToString(),
                1,
                "The weighting of this modifier to be applied to monsters.",
                true);
            configModifierWeights.Add(modifierType, modifierEntry);
            
        }
    }

    /// <summary>
    /// Takes in a dictionary of modifier type - modifier data key-value pairs and dynamically alters the weighting of
    /// the modifier data, if custom config values have been chosen by the user via the mod bepinex config. 
    /// </summary>
    /// <param name="defaultModifiers">The default modifiers to attempt altering weighting data for.</param>
    /// <returns>The modified modifier data dictionary.</returns>
    public Dictionary<MonsterModifierTypes, ModifierData> InitCustomConfigModifiers(
        Dictionary<MonsterModifierTypes, ModifierData> defaultModifiers)
    {
        foreach (var modiferEntry in configModifierWeights)
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
