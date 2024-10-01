using Jotunn.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterModifiers;

public class LocationUtils
{
    private static readonly Vector3 blackForest_crypt2_position = new Vector3(2.387963f, -0.1440315f, -4.879501f);
    private static readonly Quaternion blackForest_crypt2_rotation = Quaternion.Euler(0, 180, 0);
    
    private static readonly Vector3 blackForest_crypt4_position = new Vector3(-3.318436f, -0.06190443f, 11.17026f);
    private static readonly Quaternion blackForest_crypt4_rotation = Quaternion.Euler(0, 0, 0);
    
    private static readonly Vector3 swamp_sunkenCrypt4_position = new Vector3(-3.808441f, 0.01061201f, -1.311096f);
    private static readonly Quaternion swamp_sunkenCrypt4_rotation = Quaternion.Euler(0, -90, 0);
    
    private static readonly Vector3 mountain_frostCave_position = new Vector3(-5.006397f, -3.530296f, 2.135468f);
    private static readonly Quaternion mountain_frostCave_rotation = Quaternion.Euler(0, 0, 0);
    
    private static readonly Vector3 mistlands_entrance1_position = new Vector3(-0.01721701f, -1.47747f, 1.25589f);
    private static readonly Quaternion mistlands_entrance1_rotation = Quaternion.Euler(0, 0, 0);
    
    private static readonly Vector3 mistlands_entrance2_position = new Vector3(4.753941f, 8.999512f, 6.184937f);
    private static readonly Quaternion mistlands_entrance2_rotation = Quaternion.Euler(0, -59.971f, 0);

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
        ModifyLocation("Crypt2", blackForest_crypt2_position,blackForest_crypt2_rotation);
        ModifyLocation("Crypt4", blackForest_crypt4_position,blackForest_crypt4_rotation);
        ModifyLocation("SunkenCrypt4", swamp_sunkenCrypt4_position,swamp_sunkenCrypt4_rotation);
        ModifyLocation("MountainCave02", mountain_frostCave_position,mountain_frostCave_rotation);
        ModifyLocation("Mistlands_DvergrTownEntrance1", mistlands_entrance1_position,mistlands_entrance1_rotation);
        ModifyLocation("Mistlands_DvergrTownEntrance2", mistlands_entrance2_position,mistlands_entrance2_rotation);
        
        ZoneManager.OnVanillaLocationsAvailable -= ModifyAllLocations;
    }
    
    public static void ModifyLocation(string locationName, Vector3 position, Quaternion rotation)
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
                string altarPrefabName = string.Empty;
                
                switch (locationName)
                {
                    case "Crypt2":
                        altarPrefabName = "DungeonAltar_BlackForest";
                        break;
                    case "Crypt4":
                        altarPrefabName = "DungeonAltar_BlackForest";
                        break;
                    case "SunkenCrypt4":
                        altarPrefabName = "DungeonAltar_Swamp";
                        break;
                    case "MountainCave02":
                        altarPrefabName = "DungeonAltar_Mountain";
                        break;
                    case "Mistlands_DvergrTownEntrance1":
                        altarPrefabName = "DungeonAltar_Mistlands1";
                        break;
                    case "Mistlands_DvergrTownEntrance2":
                        altarPrefabName = "DungeonAltar_Mistlands2";
                        break;
                }
                
                GameObject altarPrefab = PrefabManager.Instance.GetPrefab(altarPrefabName);

                if (altarPrefab != null)
                {
                    GameObject tempParent = new GameObject("TempParent");
                    tempParent.SetActive(false);
                    var altar = Object.Instantiate(altarPrefab, tempParent.transform);
                    altar.transform.SetParent(location.m_prefab.Asset.transform);
                    altar.transform.localPosition = position;
                    altar.transform.rotation = rotation;
                    Object.Destroy(tempParent);
                }
                else
                {
                    Debug.LogError("Prefab " + altarPrefabName + " not found.");
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