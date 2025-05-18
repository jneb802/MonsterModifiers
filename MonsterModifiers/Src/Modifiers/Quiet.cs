using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class Quiet
{
    public static void AddQuiet(Character character)
    {
        BaseAI baseAI = character.m_baseAI;
        if (baseAI != null)
        {
            if (baseAI.m_alertedEffects.HasEffects())
            {
                foreach (var effectPrefab in baseAI.m_alertedEffects.m_effectPrefabs)
                {
                    effectPrefab.m_enabled = false;
                }
            }
            
            if (baseAI.m_idleSound.HasEffects())
            {
                foreach (var effectPrefab in baseAI.m_idleSound.m_effectPrefabs)
                {
                    effectPrefab.m_enabled = false;
                }
            }
                
        }
    }
}