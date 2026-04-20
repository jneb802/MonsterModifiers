using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class DistantDetection
{
    public static void AddDistantDetection(Character character)
    {
        BaseAI baseAI = character.m_baseAI;
        if (baseAI != null)
        {
            float rangeMult = MonsterModifiersPlugin.Cfg_DistantDetection_RangeMultiplier.Value;
            baseAI.m_hearRange *= rangeMult;
            baseAI.m_viewRange *= rangeMult;
        }
    }

    public static void RemoveDistantDetection(Character character)
    {
        BaseAI baseAI = character.m_baseAI;
        if (baseAI != null)
        {
            float rangeMult = MonsterModifiersPlugin.Cfg_DistantDetection_RangeMultiplier.Value;
            baseAI.m_hearRange /= rangeMult;
            baseAI.m_viewRange /= rangeMult;
        }
    }
}