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
    
}
public class Modifier
{
    public const float DefaultValue = 1;
    public MonsterModifierTypes ModifierType { get; set; }
    public float ModifierValue;

    public Modifier(MonsterModifierTypes modifierType, float value = DefaultValue)
    {
        ModifierType = modifierType;
        ModifierValue = value;
    }
}