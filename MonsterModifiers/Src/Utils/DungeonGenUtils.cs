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
}