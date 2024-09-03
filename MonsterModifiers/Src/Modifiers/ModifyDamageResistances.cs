using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class ModifyDamageResistances
{
    public static void ModifyResistance(GameObject prefab, HitData.DamageType damageType, HitData.DamageModifier modifier)
    {
        if (prefab.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            var monsterModifiers = humanoid.m_damageModifiers;
            
            List<HitData.DamageModPair> modifiers = new List<HitData.DamageModPair>
            {
                new HitData.DamageModPair { m_type = damageType, m_modifier = modifier }
            };
            
            monsterModifiers.Apply(modifiers);
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Added resistance of type " + modifier + " to damage type " + damageType);
        }
    }
}