using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class FastMovement
{
    public static void AddFastMovement(Character character)
    {
        // Debug.Log("Monster with name " + character.m_name + " has modifier Fast Movement");
        
        character.m_speed *= 1.5f;
        character.m_runSpeed *= 1.5f;
        character.m_walkSpeed *= 1.5f;
    }
    
    public static void RemoveFastMovement(Character character)
    {
        // Debug.Log("Monster with name " + character.m_name + " has modifier Fast Movement");
        character.m_speed /= 1.5f;
        character.m_runSpeed /= 1.5f;
        character.m_walkSpeed /= 1.5f;
    }
}