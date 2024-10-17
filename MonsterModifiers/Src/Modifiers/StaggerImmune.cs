using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class StaggerImmune
{
    public static void AddStaggerImmune(Character character)
    {
        // Debug.Log("Monster with name " + character.m_name + " has modifier Stagger Immune");
        character.m_staggerWhenBlocked = false;
    }
    
    public static void RemoveStaggerImmune(Character character)
    {
        // Debug.Log("Monster with name " + character.m_name + " has modifier Stagger Immune");
        character.m_staggerWhenBlocked = true;
    }
}