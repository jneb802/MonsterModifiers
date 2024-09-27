using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class DistantDetection
{
    public static void AddDistantDetection(Character character)
    {
        Debug.Log("Monster with name " + character.m_name + " has modifier Stagger Immune");
        BaseAI baseAI = character.m_baseAI;
        if (baseAI != null)
        {
            baseAI.m_hearRange *= 2.0f;
            baseAI.m_viewRange *= 2.0f;
        }
    }
}