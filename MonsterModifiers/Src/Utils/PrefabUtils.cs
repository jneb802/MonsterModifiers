using Jotunn;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace MonsterModifiers;

public class PrefabUtils
{
    public static GameObject leechDeathVFX;
    public static GameObject leechDeathSFX;
    
    public static void CreateCustomPrefabs()
    {
        GameObject shaman_heal_aoe = PrefabManager.Instance.GetPrefab("shaman_heal_aoe");
        GameObject modifier_shaman_heal_aoe = PrefabManager.Instance.CreateClonedPrefab("healCustomPrefab",shaman_heal_aoe);
        CustomPrefab healCustomPrefab = new CustomPrefab(modifier_shaman_heal_aoe, false);
        Aoe aoe = healCustomPrefab.Prefab.GetComponent<Aoe>();
        aoe.m_statusEffect = "";
        
        // This is the mistle prefab. I modiify it to remove monsterAI.
        GameObject mistile = PrefabManager.Instance.GetPrefab("Mistile");
        GameObject modifier_mistile = PrefabManager.Instance.CreateClonedPrefab("mistleCustomPrefab",mistile);
        CustomPrefab mistleCustomPrefab = new CustomPrefab(modifier_mistile, false);
        Character mistileCharacter = mistleCustomPrefab.Prefab.GetComponent<Humanoid>();
        mistileCharacter.m_speed = 0f;
        mistileCharacter.m_flyFastSpeed = 0f;
        mistileCharacter.m_flySlowSpeed = 0f;
        mistileCharacter.m_name = "$modifier_mistile";
        mistleCustomPrefab.Prefab.GetComponent<CharacterTimedDestruction>().m_timeoutMin = 2;
        mistleCustomPrefab.Prefab.GetComponent<CharacterTimedDestruction>().m_timeoutMax = 4;
        
        // This is the mistile Nova. I had to change the rotation to ensure if would explore on the horizontal plane.
        GameObject fx_DvergerMage_Mistile_attack = PrefabManager.Instance.GetPrefab("fx_DvergerMage_Mistile_attack");
        GameObject modifier_fx_DvergerMage_Mistile_attack = PrefabManager.Instance.CreateClonedPrefab("staggerDeathNovaCustomPrefab", fx_DvergerMage_Mistile_attack);
        CustomPrefab staggerDeathNovaCustomPrefab = new CustomPrefab(modifier_fx_DvergerMage_Mistile_attack, false);
        GameObject shockwave1 = staggerDeathNovaCustomPrefab.Prefab.FindDeepChild("shockwave (1)").gameObject;
        ParticleSystem particleSystem = shockwave1.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startRotationX = new ParticleSystem.MinMaxCurve(Mathf.Deg2Rad * 90f);
        mainModule.startRotationY = new ParticleSystem.MinMaxCurve(Mathf.Deg2Rad * 0f);
        mainModule.startRotationZ = new ParticleSystem.MinMaxCurve(Mathf.Deg2Rad * 0f);
        
        // This is the skeleton_bow prefab. I modiify it to change the projectile to an elemental infused type.
        GameObject skeletonBow = PrefabManager.Instance.GetPrefab("skeleton_bow");
        
        // FIRE
        GameObject modifier_skeletonBow_Fire = PrefabManager.Instance.CreateClonedPrefab("skeletonBowCustomPrefab_Fire", skeletonBow);
        CustomPrefab skeletonBowCustomPrefab_Fire = new CustomPrefab(modifier_skeletonBow_Fire, false);
        ItemDrop.ItemData skeletonBowFireItemData = skeletonBowCustomPrefab_Fire.Prefab.GetComponent<ItemDrop>().m_itemData;
        skeletonBowFireItemData.m_shared.m_attack.m_attackProjectile = PrefabManager.Instance.GetPrefab("bow_projectile_fire");
        
        // FROST
        GameObject modifier_skeletonBow_Frost = PrefabManager.Instance.CreateClonedPrefab("skeletonBowCustomPrefab_Frost", skeletonBow);
        CustomPrefab skeletonBowCustomPrefab_Frost = new CustomPrefab(modifier_skeletonBow_Frost, false);
        ItemDrop.ItemData skeletonBowFrostItemData = skeletonBowCustomPrefab_Frost.Prefab.GetComponent<ItemDrop>().m_itemData;
        skeletonBowFrostItemData.m_shared.m_attack.m_attackProjectile = PrefabManager.Instance.GetPrefab("bow_projectile_frost");

        // POISON
        GameObject modifier_skeletonBow_Poison = PrefabManager.Instance.CreateClonedPrefab("skeletonBowCustomPrefab_Poison", skeletonBow);
        CustomPrefab skeletonBowCustomPrefab_Poison = new CustomPrefab(modifier_skeletonBow_Poison, false);
        ItemDrop.ItemData skeletonBowPoisonItemData = skeletonBowCustomPrefab_Poison.Prefab.GetComponent<ItemDrop>().m_itemData;
        skeletonBowPoisonItemData.m_shared.m_attack.m_attackProjectile = PrefabManager.Instance.GetPrefab("bow_projectile_poison");
        
        // This is the draugr bow prefab. I modiify it to change the projectile to an elemental infused type.
        GameObject draugrBow = PrefabManager.Instance.GetPrefab("draugr_bow");
        
        // FIRE
        GameObject modifier_draugrBow_Fire = PrefabManager.Instance.CreateClonedPrefab("draugrBowCustomPrefab_Fire", draugrBow);
        CustomPrefab draugrBowCustomPrefab_Fire = new CustomPrefab(modifier_draugrBow_Fire, false);
        ItemDrop.ItemData draugrBowFireItemData = draugrBowCustomPrefab_Fire.Prefab.GetComponent<ItemDrop>().m_itemData;
        draugrBowFireItemData.m_shared.m_attack.m_attackProjectile = PrefabManager.Instance.GetPrefab("bow_projectile_fire");
        
        // FROST
        GameObject modifier_draugrBow_Frost = PrefabManager.Instance.CreateClonedPrefab("draugrBowCustomPrefab_Frost", draugrBow);
        CustomPrefab draugrBowCustomPrefab_Frost = new CustomPrefab(modifier_draugrBow_Frost, false);
        ItemDrop.ItemData draugrBowFrostItemData = draugrBowCustomPrefab_Frost.Prefab.GetComponent<ItemDrop>().m_itemData;
        draugrBowFrostItemData.m_shared.m_attack.m_attackProjectile = PrefabManager.Instance.GetPrefab("bow_projectile_frost");

        // POISON
        GameObject modifier_draugrBow_Poison = PrefabManager.Instance.CreateClonedPrefab("draugrBowCustomPrefab_Poison", draugrBow);
        CustomPrefab draugrBowCustomPrefab_Poison = new CustomPrefab(modifier_draugrBow_Poison, false);
        ItemDrop.ItemData draugrBowPoisonItemData = draugrBowCustomPrefab_Poison.Prefab.GetComponent<ItemDrop>().m_itemData;
        draugrBowPoisonItemData.m_shared.m_attack.m_attackProjectile = PrefabManager.Instance.GetPrefab("bow_projectile_poison");
        
        PrefabManager.Instance.AddPrefab(healCustomPrefab);
        PrefabManager.Instance.AddPrefab(mistleCustomPrefab);
        PrefabManager.Instance.AddPrefab(staggerDeathNovaCustomPrefab);
        PrefabManager.Instance.AddPrefab(skeletonBowCustomPrefab_Fire);
        PrefabManager.Instance.AddPrefab(skeletonBowCustomPrefab_Frost);
        PrefabManager.Instance.AddPrefab(skeletonBowCustomPrefab_Poison);
        PrefabManager.Instance.AddPrefab(draugrBowCustomPrefab_Fire);
        PrefabManager.Instance.AddPrefab(draugrBowCustomPrefab_Frost);
        PrefabManager.Instance.AddPrefab(draugrBowCustomPrefab_Poison);
        
        leechDeathVFX = PrefabManager.Instance.GetPrefab("vfx_leech_death");
        leechDeathSFX = PrefabManager.Instance.GetPrefab("sfx_leech_death");
        
        PrefabManager.OnVanillaPrefabsAvailable -= CreateCustomPrefabs;
    }
    
    
}