using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Jotunn.Managers;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.Visuals;

public class BowVisuals
{
    public static GameObject GetModifierVisual(ItemDrop.ItemData item, List<MonsterModifierTypes> modifiers)
    {
        switch (item.m_shared.m_name)
        {
            case "skeleton_bow":
                Debug.Log("Skeleton bow detected");
                return Visuals.BowVisuals.GetSkeletonBowVisual(modifiers);

            case "draugr_bow":
                Debug.Log("Draugr bow detected");
                return Visuals.BowVisuals.GetDraugrBowVisual(modifiers);

            default:
                Debug.Log("No matching visual found for: " + item.m_shared.m_name);
                return null;
        }
    }

    public static GameObject GetSkeletonBowVisual(List<MonsterModifierTypes> modifiers)
    {
        if (modifiers.Contains(MonsterModifierTypes.FireInfused))
        {
            return PrefabManager.Instance.GetPrefab("skeletonBowCustomPrefab_Fire");
        }
        else if (modifiers.Contains(MonsterModifierTypes.FrostInfused))
        {
            return PrefabManager.Instance.GetPrefab("skeletonBowCustomPrefab_Frost");
        }
        else if (modifiers.Contains(MonsterModifierTypes.PoisonInfused))
        {
            return PrefabManager.Instance.GetPrefab("skeletonBowCustomPrefab_Poison");
        }

        return null;
    }
    
    public static GameObject GetDraugrBowVisual(List<MonsterModifierTypes> modifiers)
    {
        if (modifiers.Contains(MonsterModifierTypes.FireInfused))
        {
            return PrefabManager.Instance.GetPrefab("draugrBowCustomPrefab_Fire");
        }
        else if (modifiers.Contains(MonsterModifierTypes.FrostInfused))
        {
            return PrefabManager.Instance.GetPrefab("draugrBowCustomPrefab_Frost");
        }
        else if (modifiers.Contains(MonsterModifierTypes.PoisonInfused))
        {
            return PrefabManager.Instance.GetPrefab("draugrBowCustomPrefab_Poison");
        }

        Debug.Log("No matching visual found for Draugr bow");
        return null;
    }
}