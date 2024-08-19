using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class ModifyHealth
{
    public static void ModifyCreatureHealth(GameObject prefab, Modifier modifer)
    {
        if (prefab.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoid.m_health *= (1 + modifer.ModifierValue);
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Added resistance of type");
        }
    }
}