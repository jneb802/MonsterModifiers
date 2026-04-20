using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class FastMovement
{
    public static void AddFastMovement(Character character)
    {
        float speedMult = 1f + MonsterModifiersPlugin.Cfg_FastMovement_SpeedPercent.Value / 100f;
        character.m_speed *= speedMult;
        character.m_runSpeed *= speedMult;
        character.m_walkSpeed *= speedMult;
    }

    public static void RemoveFastMovement(Character character)
    {
        float speedMult = 1f + MonsterModifiersPlugin.Cfg_FastMovement_SpeedPercent.Value / 100f;
        character.m_speed /= speedMult;
        character.m_runSpeed /= speedMult;
        character.m_walkSpeed /= speedMult;
    }
}