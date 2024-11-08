using Mono.Security.X509;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.StatusEffects;

public class HealDeath_SE : SE_Stats
{
    public override void SetLevel(int itemLevel, float skillLevel)
    {
        this.m_healthPerTick = skillLevel / 10;
    }
}