using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers;

public class WorldUtils
{
    public static List<Character> GetAllCharacter(Vector3 position, float range)
    {
        Collider[] hits = Physics.OverlapBox(position, Vector3.one * range, Quaternion.identity);
        List<Character> characters = new List<Character>();

        foreach (var hit in hits)
        {
            Character character = hit.transform.root.gameObject.GetComponentInChildren<Character>();
            if (character != null)
            {
                characters.Add(character);
            }
        }

        return characters;
    }
    
    public static List<CreatureSpawner> GetCreatureSpawnersInDungeon(DungeonGenerator dungeonGenerator)
    {
        MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("GetCreatureSpawnersInDungeon is called");
        List<CreatureSpawner> creatureSpawnerList = new List<CreatureSpawner>();
        GameObject parentPrefab = dungeonGenerator.gameObject;
        MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("DungeonGenerator Parent has name: " + parentPrefab);
        
        CreatureSpawner[] creatureSpawners = parentPrefab.GetComponentsInChildren<CreatureSpawner>(true);
        foreach (var spawner in creatureSpawners)
        {
            creatureSpawnerList.Add(spawner);
        }
        
        foreach (var creatureSpawner in creatureSpawners)
        {
            string spawnerPath = GetFullPath(creatureSpawner.gameObject);
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Found creatureSpawner at path: " + spawnerPath);
        }

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
    
    private static string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform;
        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }
        return path;
    }
    
    public static List<CreatureSpawner> GetAllCreatureSpawner(Vector3 position, float range)
    {
        Collider[] hits = Physics.OverlapBox(position, Vector3.one * range, Quaternion.identity);
        List<CreatureSpawner> creatureSpawners = new List<CreatureSpawner>();

        foreach (var hit in hits)
        {
            CreatureSpawner creatureSpawner = hit.transform.root.gameObject.GetComponentInChildren<CreatureSpawner>();
            if (creatureSpawner != null)
            {
                creatureSpawners.Add(creatureSpawner);
            }
        }

        return creatureSpawners;
    }
}