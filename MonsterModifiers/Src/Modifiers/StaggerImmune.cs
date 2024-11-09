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

    [HarmonyPatch(typeof(Character), nameof(Character.Stagger))]
    public class StaminaSiphon_Character_RPC_Damage_Patch
    {
        public static bool Prefix(Character __instance)
        {
            if (__instance == null)
            {
                return true;
            }

            var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return true;
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.StaggerImmune))
            {
                return false;
            }
            
            return true;
        }
    }
}
