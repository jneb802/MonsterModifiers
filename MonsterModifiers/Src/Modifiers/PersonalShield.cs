using HarmonyLib;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class PersonalShield
{
    public static void AddPersonalShield(Character character)
    {
        Debug.Log("Monster with name " + character.m_name + " has modifier personal Shield");
        int hashName = "GoblinShaman_shield".GetStableHashCode();
        character.GetSEMan().AddStatusEffect(hashName, true, 10, 2.0f);
    }
}