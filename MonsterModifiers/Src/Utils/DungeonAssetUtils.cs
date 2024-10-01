using System.Reflection;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers;

public class DungeonAssetUtils
{
    public static AssetBundle dungeonAssetBundle;
    
    public static void Setup()
    {
        dungeonAssetBundle = AssetUtils.LoadAssetBundleFromResources("dungeonmodifiers", Assembly.GetExecutingAssembly());
    }

    public static void SetupDungeonAssets()
    {
        CreateSigil();
        AddAllAltars();
    }
    
    public static void CreateSigil()
    {
        ItemConfig sigilConfig = new ItemConfig();
        sigilConfig.Amount = 1;
        sigilConfig.AddRequirement(new RequirementConfig("Stone", 1));
        
        var runeItem = new CustomItem(dungeonAssetBundle, "Sigil_magic_warp", fixReference: false, sigilConfig);
        runeItem.ItemPrefab.AddComponent<Sigil>();
        ItemManager.Instance.AddItem(runeItem);
    }
    
    public static void AddAllAltars()
    {
        AddAltar("DungeonAltar_BlackForest");  // Corresponds to Crypt2// Corresponds to Crypt4
        AddAltar("DungeonAltar_Swamp");        // Corresponds to SunkenCrypt4
        AddAltar("DungeonAltar_Mountain");     // Corresponds to MountainCave02
        AddAltar("DungeonAltar_Mistlands1");   // Corresponds to Mistlands_DvergrTownEntrance1
        AddAltar("DungeonAltar_Mistlands2");   // Corresponds to Mistlands_DvergrTownEntrance2
    }

    public static void AddAltar(string altarName)
    {
        CustomPrefab altarPrefab = new CustomPrefab(dungeonAssetBundle, altarName, true);
        
        var altar = altarPrefab.Prefab.AddComponent<Altar>();
        altar.m_incinerateSwitch = altarPrefab.Prefab.GetComponentInChildren<Switch>();
        altar.m_nview = altarPrefab.Prefab.GetComponent<ZNetView>();
        altar.m_container = altarPrefab.Prefab.GetComponentInChildren<Container>();
        
        PrefabManager.Instance.AddPrefab(altarPrefab);
    }
}