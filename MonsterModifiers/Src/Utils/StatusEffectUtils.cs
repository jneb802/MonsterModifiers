using Jotunn.Entities;
using Jotunn.Managers;
using MonsterModifiers.StatusEffects;
using UnityEngine;

namespace MonsterModifiers;

public class StatusEffectUtils
{
    public static void CreateCustomStatusEffects()
    {
        StatusEffect bloodLossEffect = ScriptableObject.CreateInstance<BloodLoss_SE>();
        
        bloodLossEffect.name = "BloodLossStatusEffect";
        bloodLossEffect.m_name = "$se_bloodLoss";
        bloodLossEffect.m_icon =  ModifierAssetUtils.bloodIconRed;
        
        CustomStatusEffect bloodLoss = new CustomStatusEffect(bloodLossEffect, false);

        ItemManager.Instance.AddStatusEffect(bloodLoss);
    }
}