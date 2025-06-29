using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using MonsterModifiers.Visuals;

namespace MonsterModifiers.Custom_Components;

public class AddVisualsPatch
{
    [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.Start))]
    public static class Humanoid_Start_Patch
    {
        private static void Postfix(Humanoid __instance)
        {
            if (!__instance.IsPlayer() && !__instance.IsBoss())
            {
                ItemDrop.ItemData currentItem = __instance.GetCurrentWeapon();
                    
                if (currentItem.m_shared.m_name.Equals("skeleton_bow") || currentItem.m_shared.m_name.Equals("draugr_bow"))
                {
                    Debug.Log("Adding visuals to humanoid with name " + __instance.name);
                    MonsterModifier monsterModifier = __instance.GetComponent<MonsterModifier>();
                    if (monsterModifier != null)
                    {
                        Debug.Log("Adding bow visuals");
                        List<MonsterModifierTypes> modifiers = monsterModifier.Modifiers;
                        Debug.Log("monster has modifiers: " + modifiers.Count);
                        GameObject newItem = Visuals.BowVisuals.GetModifierVisual(currentItem, modifiers);
                        Debug.Log("Unequiping items");
                        __instance.UnequipAllItems();
                        Debug.Log("Adding new item with name: " + newItem);
                        __instance.GiveDefaultItem(newItem);
                    } 
                }
            }
        }
    }
}