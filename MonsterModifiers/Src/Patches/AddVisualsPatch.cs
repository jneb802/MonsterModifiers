using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using MonsterModifiers.Visuals;

namespace MonsterModifiers.Custom_Components;

public class AddVisualsPatch
{
    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.Start))]
    public static class MonsterAI_Start_Patch
    {
        private static void Postfix(MonsterAI __instance)
        {
            MonsterModifier monsterModifier = __instance.GetComponent<MonsterModifier>();
            if (monsterModifier == null) return;
            
            List<MonsterModifierTypes> modifiers = monsterModifier.Modifiers;
            if (modifiers.Count == 0) return;

            bool hasSkeletonBow = false;
            bool hasDraugrBow = false;
            Humanoid humanoid = __instance.GetComponent<Humanoid>();

            foreach (var item in humanoid.m_inventory.GetAllItems())
            {
                if (item.m_dropPrefab.name == "skeleton_bow")
                {
                    hasSkeletonBow = true;
                    //Debug.Log("Humanoid has skeleton bow item");
                }

                if (item.m_dropPrefab.name == "draugr_bow")
                {
                    hasDraugrBow = true;
                    // Debug.Log("Humanoid has draugr bow item");
                }
            }

            if (hasSkeletonBow)
            {
                GameObject newItem = Visuals.BowVisuals.GetModifierVisual("skeleton_bow", modifiers);
                humanoid.m_inventory.RemoveAll();
                humanoid.m_inventory.AddItem(newItem, 1);
            }

            if (hasDraugrBow)
            {
                GameObject newItem = Visuals.BowVisuals.GetModifierVisual("draugr_bow", modifiers);
                humanoid.m_inventory.RemoveAll();
                humanoid.m_inventory.AddItem(newItem, 1);
            }
        }
    }
}