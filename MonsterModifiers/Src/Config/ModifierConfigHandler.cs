using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using Jotunn.Extensions;
using UnityEngine;

namespace MonsterModifiers.Config;

/// <summary>
/// Handler for the values determined by the Bepinex Config for this mod.
/// </summary>
public class ModifierConfigHandler
{
    ConfigFile? _configFile;
    readonly Dictionary<MonsterModifierTypes, ConfigEntry<int>> _configModifierWeights = new();

    static ModifierConfigHandler? _instance;

    /// <summary>
    /// Gets the singleton instance of the Handler.
    /// </summary>
    public static ModifierConfigHandler Instance
    {
        get
        {
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
        _configFile = configFile;
        InitConfigParams();
    }

    /// <summary>
    /// Initializes the weighting values of all the Monster Modifiers in the Mod. The intention is that, regardless of
    /// how many modifiers are added to the mod, the config will dynamically add the new modifiers to the weighting
    /// section.
    /// </summary>
    void InitConfigParams()
    {
        if (_configFile == null)
        {
            throw new InvalidOperationException(
                "The initialization of config parameters has been called, but the"
                    + " configuration file has not been initialized in the config handler!"
            );
        }
        foreach (MonsterModifierTypes modifierType in Enum.GetValues(typeof(MonsterModifierTypes)))
        {
            ConfigEntry<int> modifierEntry = _configFile.BindConfig(
                "Modifier Weights",
                modifierType.ToString(),
                1,
                "The application weight of the modifier. Value should be any positive integer."
            );
            if (modifierEntry.Value <= 0)
            {
                MonsterModifiersPlugin.MonsterModifiersLogger.LogWarning(
                    $"Weight for {modifierType.ToString()} modifier was set to less than 0 ({modifierEntry.Value}), weight has been ignored, please change this weight to an integer value that is higher than 0"
                );
                continue;
            }
            _configModifierWeights.Add(modifierType, modifierEntry);
        }
    }

    /// <summary>
    /// Takes in a dictionary of modifier type - modifier data key-value pairs and dynamically alters the weighting of
    /// the modifier data, if custom config values have been chosen by the user via the mod bepinex config.
    /// </summary>
    /// <param name="defaultModifiers">The default modifiers to attempt altering weighting data for.</param>
    /// <returns>The modified modifier data dictionary.</returns>
    public Dictionary<MonsterModifierTypes, ModifierData> InitCustomConfigModifiers(
        Dictionary<MonsterModifierTypes, ModifierData> defaultModifiers
    )
    {
        if (_configFile == null)
        {
            throw new InvalidOperationException(
                "Config modifier weights are being queried by the Modifier Config Handler, but the initialization of the Mod's config file has not succesfully executed!"
            );
        }
        foreach (var modiferEntry in _configModifierWeights)
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
