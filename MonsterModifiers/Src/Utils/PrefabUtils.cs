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
        
        PrefabManager.Instance.AddPrefab(healCustomPrefab);
        
        leechDeathVFX = PrefabManager.Instance.GetPrefab("vfx_leech_death");
        leechDeathSFX = PrefabManager.Instance.GetPrefab("sfx_leech_death");
        
        PrefabManager.OnVanillaPrefabsAvailable -= CreateCustomPrefabs;
    }
    
    
}