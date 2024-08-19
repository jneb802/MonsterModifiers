using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers;

public class CreatureSpawnerUtils
{
    public static List<CreatureSpawner> GetCreatureSpawnersInDungeon(DungeonGenerator dungeonGenerator)
    {
        List<CreatureSpawner> creatureSpawnerList = new List<CreatureSpawner>();
        GameObject parentPrefab = dungeonGenerator.gameObject;
        CreatureSpawner[] creatureSpawners = parentPrefab.GetComponentsInChildren<CreatureSpawner>();
        creatureSpawnerList.AddRange(creatureSpawners);
        return creatureSpawnerList;
    }

    public static GameObject GetCreaturePrefab(CreatureSpawner creatureSpawner)
    {
        return creatureSpawner.m_creaturePrefab;
    }
}