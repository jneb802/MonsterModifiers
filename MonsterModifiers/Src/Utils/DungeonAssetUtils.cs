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
        AddAltar();
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

    public static void AddAltar()
    {
        CustomPrefab altarPrefab = new CustomPrefab(dungeonAssetBundle, "DungeonAltar", true);
        
        var altar = altarPrefab.Prefab.AddComponent<Altar>();
        altar.m_incinerateSwitch = altarPrefab.Prefab.GetComponentInChildren<Switch>();
        altar.m_nview = altarPrefab.Prefab.GetComponent<ZNetView>();
        altar.m_container = altarPrefab.Prefab.GetComponentInChildren<Container>();
        
        PrefabManager.Instance.AddPrefab(altarPrefab);
    }
    
    public static void AddSigilTable()
    {
        var sigilTablePrefab = new CustomPrefab(dungeonAssetBundle, "piece_sigiltable_warp", true);

        if (sigilTablePrefab == null)
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogError("Failed to load gameObject with name: piece_sigiltable_warp");
        }
        PrefabManager.Instance.AddPrefab(sigilTablePrefab);
    }
}