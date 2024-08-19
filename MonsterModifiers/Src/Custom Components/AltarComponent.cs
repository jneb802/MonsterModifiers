using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class AltarComponent : MonoBehaviour
{
    
    public void ApplyAllModifiers(List<Modifier> modifiers, List<CreatureSpawner> creatureSpawners)
    {
        foreach (var creatureSpawner in creatureSpawners)
        {
            foreach (var modifier in modifiers)
            {
                ApplyCreatureModifier(modifier, creatureSpawner);
            }
        }
    }

    public void ApplyCreatureModifier(Modifier modifier, CreatureSpawner creatureSpawner)
    {
        switch (modifier.ModifierType)
        {
            // Resistances
            case MonsterModifierTypes.AddBluntResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Blunt, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddBluntVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Blunt, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddSlashResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Slash, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddSlashVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Slash, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddPierceResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Pierce, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddPierceVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Pierce, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddChopResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Chop, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddChopVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Chop, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddPickaxeResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Pickaxe, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddPickaxeVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Pickaxe, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddFireResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Fire, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddFireVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Fire, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddFrostResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Frost, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddFrostVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Frost, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddLightningResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Lightning, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddLightningVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Lightning, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddPoisonResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Poison, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddPoisonVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Poison, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddSpiritResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Spirit, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddSpiritVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Spirit, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddDamageResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Damage, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddDamageVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Damage, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddPhysicalResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Physical, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddPhysicalVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Physical, HitData.DamageModifier.VeryResistant);
                break;

            case MonsterModifierTypes.AddElementalResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Elemental, HitData.DamageModifier.Resistant);
                break;

            case MonsterModifierTypes.AddElementalVeryResistant:
                Modifiers.ModifyDamageResistances.ModifyResistance(creatureSpawner.m_creaturePrefab, HitData.DamageType.Elemental, HitData.DamageModifier.VeryResistant);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(modifier.ModifierType), modifier.ModifierType, "Unexpected modifier type");
        }
    }
    
}