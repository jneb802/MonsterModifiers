using Mono.Security.X509;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.StatusEffects;

public class BloodLoss_SE : StatusEffect
{
    public int bloodLossAmount = 0;
    public int bloodLossCap;
    private float reductionTimer = 0f;
    
    public override void Setup(Character character)
    {
        base.Setup(character);
        bloodLossCap = Mathf.FloorToInt(character.GetMaxHealth());
    }
    
    public override void UpdateStatusEffect(float dt)
    {
        reductionTimer += dt;
        
        if (reductionTimer >= 60f)
        {
            bloodLossAmount = Mathf.Max(0, bloodLossAmount - 10);
            this.m_character.GetSEMan().RemoveStatusEffect("BloodLossStatusEffect".GetStableHashCode());
            
            reductionTimer = 0f;
        }

        if (bloodLossAmount > bloodLossCap)
        {
            bloodLossAmount = 0;
            this.m_character.GetSEMan().RemoveStatusEffect("BloodLossStatusEffect".GetStableHashCode());
            
            HitData bloodLossHit = new HitData
            {
                m_damage = { m_slash = m_character.GetMaxHealth() * 0.30f }
            };
            
            m_character.Damage(bloodLossHit);
            Object.Instantiate(PrefabUtils.leechDeathSFX,m_character.transform.position,m_character.transform.rotation);
            Object.Instantiate(PrefabUtils.leechDeathVFX,m_character.transform.position,m_character.transform.rotation);
        }
    }
    
    public override string GetIconText()
    {
        return bloodLossAmount.ToString();
    }
    
    public override void OnDamaged(HitData hit, Character attacker)
    {
        Debug.Log("Character with name: " + m_character.name + " was attacked by a monster with bloodloss");
        if (!ModifierUtils.RunRPCDamageChecks(attacker,hit))
        {
            return;
        }
            
        if (!ModifierUtils.RunHitChecks(hit, true))
        {
            return;
        }

        MonsterModifier modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
        if (modiferComponent == null)
        {
            return;
        }
        
        if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.BloodLoss))
        {
            return;
        }

        bloodLossAmount += Mathf.FloorToInt(hit.GetTotalDamage());
        
    }
}