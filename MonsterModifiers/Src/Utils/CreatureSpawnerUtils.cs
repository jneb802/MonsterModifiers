using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers;

public class CreatureSpawnerUtils
{
    public static List<CreatureSpawner> GetCreatureSpawnersInDungeon(DungeonGenerator dungeonGenerator)
    {
        MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("GetCreatureSpawnersInDungeon is called");
        List<CreatureSpawner> creatureSpawnerList = new List<CreatureSpawner>();
        GameObject parentPrefab = dungeonGenerator.gameObject;
        MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("DungeonGenerator Parent has name: " + parentPrefab);
        CreatureSpawner[] creatureSpawners = parentPrefab.GetComponentsInChildren<CreatureSpawner>(true);
        creatureSpawnerList.AddRange(creatureSpawners);
        if (creatureSpawners.Length == 0)
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("No creature spawners were found in search");
        }
        foreach (var creatureSpawner in creatureSpawners)
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Found creatureSpawner with name: " + creatureSpawner.gameObject + " in dungeon with name: " + dungeonGenerator);
        }
        return creatureSpawnerList;
    }

    public static GameObject GetCreaturePrefab(CreatureSpawner creatureSpawner)
    {
        return creatureSpawner.m_creaturePrefab;
    }
}