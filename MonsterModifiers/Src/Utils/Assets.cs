using System.Reflection;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers;

public class Assets
{
    // public static void CreateSigil()
    // {
    //     ItemConfig sigilConfig = new ItemConfig();
    //     sigilConfig.Amount = 1;
    //     sigilConfig.AddRequirement(new RequirementConfig("Stone", 1));
    //     sigilConfig.CraftingStation = "piece_sigiltable_warp";
    //     
    //     var sigilItem = new CustomItem(MonsterModifiersPlugin.assetBundle, "Sigil", fixReference: true, sigilConfig);
    //     sigilItem.ItemPrefab.AddComponent<SigilComponent>();
    //     
    //     ItemManager.Instance.AddItem(sigilItem);
    //     
    //     PrefabManager.OnVanillaPrefabsAvailable -= CreateSigil;
    // }
    
    public static void CreateSigil()
    {
        // Create and add a custom item
        ItemConfig sigilConfig = new ItemConfig();
        sigilConfig.Amount = 1;
        sigilConfig.AddRequirement(new RequirementConfig("Stone", 1));

        // Prefab did not use mocked refs so no need to fix them
        var runeItem = new CustomItem(MonsterModifiersPlugin.assetBundle, "Sigil_magic_warp", fixReference: false, sigilConfig);
        ItemManager.Instance.AddItem(runeItem);
    }

    public static void AddAltar()
    {
        var altarPrefab = new CustomPrefab(MonsterModifiersPlugin.assetBundle, "DungeonAltar", true);

        if (altarPrefab == null)
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogError("Failed to load gameObject with name: DungeonAltar");
        }
        PrefabManager.Instance.AddPrefab(altarPrefab);
    }
    
    public static void AddSigilTable()
    {
        var sigilTablePrefab = new CustomPrefab(MonsterModifiersPlugin.assetBundle, "piece_sigiltable_warp", true);

        if (sigilTablePrefab == null)
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogError("Failed to load gameObject with name: piece_sigiltable_warp");
        }
        PrefabManager.Instance.AddPrefab(sigilTablePrefab);
    }
    
    
}