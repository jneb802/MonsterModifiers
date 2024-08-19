using Jotunn.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterModifiers;

public class LocationUtils
{

    public static LocationProxy GetLocationProxy(GameObject gameObject)
    {
        LocationProxy locationProxy = gameObject.gameObject.GetComponentInParent<LocationProxy>();
        return locationProxy;
    }
    
    public static Location GetLocation(GameObject gameObject)
    {
        Location location = gameObject.gameObject.GetComponent<Location>();
        return location;
    }
    
    public void ModifyLocation(string locationName)
    {
        ZoneSystem.ZoneLocation location = ZoneManager.Instance.GetZoneLocation(locationName);
        
        if (location != null)
        {
            GameObject prefab = PrefabManager.Instance.GetPrefab("DungeonAltar");
            if (prefab != null)
            {
                var altar = Object.Instantiate(prefab, location.m_prefab.Asset.transform);
                altar.transform.localPosition = new Vector3(-8.52f, 5.37f, -0.92f);
            }
            else
            {
                Debug.LogError("Prefab 'piece_lul' not found.");
            }
        }
        else
        {
            Debug.LogError($"Location '{locationName}' not found.");
        }
    }
}