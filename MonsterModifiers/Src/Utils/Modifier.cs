using System.Collections.Generic;
using Jotunn.Utils;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MonsterModifiers;

public enum MonsterModifierTypes
{
    // Vanilla resistances
    AddBluntResistant,
    AddBluntVeryResistant,
    AddSlashResistant,
    AddSlashVeryResistant,
    AddPierceResistant,
    AddPierceVeryResistant,
    AddChopResistant,
    AddChopVeryResistant,
    AddPickaxeResistant,
    AddPickaxeVeryResistant,
    AddFireResistant,
    AddFireVeryResistant,
    AddFrostResistant,
    AddFrostVeryResistant,
    AddLightningResistant,
    AddLightningVeryResistant,
    AddPoisonResistant,
    AddPoisonVeryResistant,
    AddSpiritResistant,
    AddSpiritVeryResistant,
    AddDamageResistant,
    AddDamageVeryResistant,
    AddPhysicalResistant,
    AddPhysicalVeryResistant,
    AddElementalResistant,
    AddElementalVeryResistant,
    StaminaSiphon,
    AddElementalImmunity,
    AddPhysicalImmunity
}

public enum ModifierRarity
{
    Magic,
    Rare,
    Epic,
    Legendary,
    Mythic
}

public class Modifier
{
    public const float DefaultValue = 1;
    public MonsterModifierTypes ModifierType { get; set; }
    public float ModifierValue { get; set; }
    
    public float ModifierWeight { get; set; }

    public Modifier(MonsterModifierTypes modifierType, int weight, float value = DefaultValue)
    {
        ModifierType = modifierType;
        ModifierValue = value;
        ModifierWeight = weight;
    }
}
