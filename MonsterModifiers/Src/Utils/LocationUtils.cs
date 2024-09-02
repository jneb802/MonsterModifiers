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

    public static void ModifyAllLocations()
    {
        ModifyLocation("Crypt2");
        
        ZoneManager.OnVanillaLocationsAvailable -= ModifyAllLocations;
    }
    
    public static void ModifyLocation(string locationName)
    {
        Debug.Log("Attempting to modify location: " + locationName);

        if (ZoneManager.Instance == null)
        {
            Debug.LogError("ZoneManager.Instance is null.");
            return;
        }

        ZoneSystem.ZoneLocation location = ZoneManager.Instance.GetZoneLocation(locationName);

        if (location != null)
        {
            Debug.Log("Location found: " + locationName);
            
            location.m_prefab.Load();

            if (location.m_prefab.Asset != null)
            {
                GameObject prefab = PrefabManager.Instance.GetPrefab("DungeonAltar");

                if (prefab != null)
                {
                    GameObject tempParent = new GameObject("TempParent");
                    tempParent.SetActive(false);
                    var altar = Object.Instantiate(prefab, tempParent.transform);
                    altar.transform.SetParent(location.m_prefab.Asset.transform);
                    altar.transform.localPosition = new Vector3(2.387963f, -0.1440315f, -4.879501f);
                    altar.transform.rotation = Quaternion.Euler(0, 180, 0);
                    Object.Destroy(tempParent);
                }
                else
                {
                    Debug.LogError("Prefab 'DungeonAltar' not found.");
                }
            }
            else
            {
                Debug.LogError("location.m_prefab.Asset is null after loading.");
            }
        }
        else
        {
            Debug.LogError("Location with name: " + locationName + " not found.");
        }
    }

}