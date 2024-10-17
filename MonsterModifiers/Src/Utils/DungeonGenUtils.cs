using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterModifiers;

public class DungeonGenUtils
{

    public static DungeonGenerator GetDungeonInterior(GameObject prefab)
    {
        Debug.Log("GetDungeonInterior called.");
    
        if (prefab == null)
        {
            Debug.LogError("prefab is null.");
            return null;
        }
        
        Debug.Log("Prefab position: " + prefab.transform.position);

        var GroundPosition = prefab.transform.position;
        Debug.Log("GroundPosition: " + GroundPosition);

        var SkyPosition = prefab.transform.position + new Vector3(0, 5000, 0);
        Debug.Log("SkyPosition: " + SkyPosition);
        
        float maxDistance = 64f;

        var GeneratorPosition = Vector3.zero;
        Debug.Log("GeneratorPosition: " + GeneratorPosition);

        DungeonGenerator dungeonGenerator = GetDungeonGeneratorInBounds(GroundPosition + GeneratorPosition, maxDistance);
        Debug.Log("DungeonGenerator found: " + (dungeonGenerator != null ? dungeonGenerator.name : "null"));

        return dungeonGenerator;
    }
    
    /// <summary>
    /// Returns the first DungeonGenerator in bounds if exists.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static DungeonGenerator GetDungeonGeneratorInBounds(Vector3 center, float distance)
    {
        var list = SceneManager.GetActiveScene().GetRootGameObjects();

        for (int lcv = 0; lcv < list.Length; lcv++)
        {
            var obj = list[lcv];
            var dg = obj.GetComponent<DungeonGenerator>();

            if (dg != null && InBounds(center, obj.transform.position, distance))
            {
                return dg;
            }
        }

        return null;
    }
    
    /// <summary>
    /// Determines if two positions are in range of each other.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static bool InBounds(Vector3 center, Vector3 position, float distance)
    {
        var delta = center - position;
        var mag = GetMaximumDistance(delta.x, delta.z);
        return mag <= distance;
    }
    
    /// <summary>
    /// Calculates the maximum distance a point in space can be from another: The hypotenuse of a triangle represented by x, z.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static float GetMaximumDistance(float x, float z)
    {
        return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2));
    }
    
    /// <summary>
    /// Calculates the generous radius of a dungeon.
    /// </summary>
    /// <param name="dg"></param>
    /// <returns></returns>
    public static float GetDungeonRadius(DungeonGenerator dg)
    {
        return GetMaximumDistance(dg.m_zoneSize.x / 2, dg.m_zoneSize.z / 2);
    }
    
    /// <summary>
    /// Returns whether an object can be destroyed and respawned.
    /// Used for ground locations.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private bool QualifyingObject(GameObject obj)
    {
        if (obj.GetComponent<RandomFlyingBird>() != null)
        {
            return false;
        }

        return obj.GetComponent<Destructible>() ||
               obj.GetComponent<MineRock>() ||
               obj.GetComponent<MineRock5>() ||
               obj.GetComponent<ItemDrop>() ||
               obj.GetComponent<Piece>() ||
               obj.GetComponent<Pickable>() ||
               obj.GetComponent<Character>() ||
               obj.GetComponent<CreatureSpawner>() ||
               obj.GetComponent<WearNTear>() ||
               obj.GetComponent<SpawnArea>() ||
               obj.GetComponent<RandomSpawn>();
    }
    
    /// <summary>
    /// Returns whether an object can be destroyed and respawned.
    /// Used for sky locations.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static bool QualifyingSkyObject(GameObject obj)
    {
        return !(obj.GetComponent<DungeonGenerator>() ||
                 obj.GetComponent<LocationProxy>() ||
                 obj.GetComponent<Player>()) &&
               obj.transform.position.y > 4000;
    }
    
    /// <summary>
    /// Deletes an object from the ZNetScene.
    /// </summary>
    /// <param name="obj"></param>
    private static void DeleteObject(ref GameObject obj)
    {
        var nview = obj.GetComponent<ZNetView>();
        if (nview != null && nview.GetZDO() != null)
        {
            nview.GetZDO().SetOwner(ZDOMan.GetSessionID());
        }

        ZNetScene.instance.Destroy(obj);
    }
    
    /// <summary>
    /// If the object is a door, closes it if it has key requirements.
    /// </summary>
    /// <param name="obj"></param>
    public void TryResetDoor(GameObject obj)
    {
        var door = obj.GetComponent<Door>();

        if (door != null && door.m_keyItem != null)
        {
            if (door.m_nview != null && door.m_nview.GetZDO() != null)
            {
                door.m_nview.GetZDO().Set(ZDOVars.s_state, 0);
                door.UpdateState();
            }
        }
    }
    
    /// <summary>
    /// Deletes all objects in the given dungeon.
    /// </summary>
    /// <param name="dungeonGenerator"></param>
    public static void DeleteDungeon(DungeonGenerator dungeonGenerator)
    {
        var list = SceneManager.GetActiveScene().GetRootGameObjects();
        
        var skyDistance = GetDungeonRadius(dungeonGenerator);

        for (int lcv = 0; lcv < list.Length; lcv++)
        {
            var obj = list[lcv];
            
            // sky object
            if (QualifyingSkyObject(obj) && 
                InBounds(dungeonGenerator.transform.position, obj.transform.position, skyDistance))
            {
                DeleteObject(ref obj);
            }
        }
    }
}
