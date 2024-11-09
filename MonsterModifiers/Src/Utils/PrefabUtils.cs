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
        
        PrefabManager.Instance.AddPrefab(healCustomPrefab);
        PrefabManager.Instance.AddPrefab(mistleCustomPrefab);
        PrefabManager.Instance.AddPrefab(staggerDeathNovaCustomPrefab);
        leechDeathVFX = PrefabManager.Instance.GetPrefab("vfx_leech_death");
        leechDeathSFX = PrefabManager.Instance.GetPrefab("sfx_leech_death");
        
        PrefabManager.OnVanillaPrefabsAvailable -= CreateCustomPrefabs;
    }
    
    
}