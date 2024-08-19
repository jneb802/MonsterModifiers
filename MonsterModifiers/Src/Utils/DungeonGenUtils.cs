using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterModifiers;

public class DungeonGenUtils
{

    public static DungeonGenerator GetDungeonInterior(LocationProxy locationProxy, Location location)
    {
        var locationDungeonGenerator = location.m_generator;
        var GroundPosition = locationProxy.transform.position;
        var SkyPosition = locationProxy.transform.position;
        var GroundDistance = location.m_exteriorRadius;
        var SkyDistance = location.m_interiorRadius;
        float maxDistance = Mathf.Max(GroundDistance, SkyDistance);
        var GeneratorPosition = location.m_generator != null ? location.m_generator.transform.localPosition : Vector3.zero;
        DungeonGenerator dungeonGenerator = GetDungeonGeneratorInBounds(GroundPosition + GeneratorPosition, maxDistance);

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