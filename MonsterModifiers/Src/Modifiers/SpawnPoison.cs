using Jotunn.Managers;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class SpawnPoison
{
    public static void AddSpawnPoison(GameObject prefab)
    {
        prefab.AddComponent<SpawnOnDamaged>();
        SpawnOnDamaged spawnOnDamaged = prefab.GetComponent<SpawnOnDamaged>();
        spawnOnDamaged.m_spawnOnDamage = PrefabManager.Instance.GetPrefab("blob_attack_aoe");
        
        MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Added resistance of SpawnPoison to creature with name: " + prefab);
        
    }
}