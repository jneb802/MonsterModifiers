using Jotunn.Entities;
using Jotunn.Managers;
using MonsterModifiers.StatusEffects;
using UnityEngine;

namespace MonsterModifiers;

public class StatusEffectUtils
{
    public static void CreateCustomStatusEffects()
    {
        BloodLoss_SE bloodLossEffect = ScriptableObject.CreateInstance<BloodLoss_SE>();
        bloodLossEffect.name = "BloodLossStatusEffect";
        bloodLossEffect.m_name = "$se_bloodLoss";
        bloodLossEffect.m_icon =  ModifierAssetUtils.bloodIconRed;
        CustomStatusEffect bloodLoss = new CustomStatusEffect(bloodLossEffect, false);
        
        HealDeath_SE healDeathEffect = ScriptableObject.CreateInstance<HealDeath_SE>();
        healDeathEffect.name = "HealDeathStatusEffect";
        healDeathEffect.m_name = "$se_healDeath";
        healDeathEffect.m_ttl = 5;
        healDeathEffect.m_tickInterval = 1f;
        healDeathEffect.m_healthOverTimeDuration = 5;
        healDeathEffect.m_healthOverTimeInterval = 0.5f;
        healDeathEffect.m_healthOverTimeTicks = 10;
        healDeathEffect.m_healthOverTime = 0;
        CustomStatusEffect healDeath = new CustomStatusEffect(healDeathEffect, false);

        ItemManager.Instance.AddStatusEffect(bloodLoss);
        ItemManager.Instance.AddStatusEffect(healDeath);
    }
}