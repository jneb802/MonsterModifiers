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
    
    
    public static void CreateSigil()
    {
        ItemConfig sigilConfig = new ItemConfig();
        sigilConfig.Amount = 1;
        sigilConfig.AddRequirement(new RequirementConfig("Stone", 1));
        
        var sigilItem = new CustomItem(MonsterModifiersPlugin.assetBundle, "Sigil", fixReference: true, sigilConfig);
        sigilItem.ItemPrefab.AddComponent<SigilComponent>();
        
        ItemManager.Instance.AddItem(sigilItem);
        
        PrefabManager.OnVanillaPrefabsAvailable -= CreateSigil;
    }

    public static void AddAltar()
    {
        var altarPrefab = new CustomPrefab(MonsterModifiersPlugin.assetBundle, "DungeonAltar", true);
        altarPrefab.Prefab.AddComponent<AltarComponent>();

        if (altarPrefab == null)
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogError("Failed to load gameObject with name: DungeonAltar");
        }
        PrefabManager.Instance.AddPrefab(altarPrefab);
    }
    
    
}