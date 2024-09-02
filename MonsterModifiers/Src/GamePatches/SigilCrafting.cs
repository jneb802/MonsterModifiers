using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.GamePatches;

public class SigilCrafting
{
    public static bool IsCraftingSigil;
    
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.DoCrafting))]
    public static class InventoryGuiDoCraftingPatch
    {
        [UsedImplicitly]
        public static void Prefix(InventoryGui __instance)
        {
            IsCraftingSigil = true;
        }

        [UsedImplicitly]
        public static void Postfix(InventoryGui __instance, Player player)
        {
            
            IsCraftingSigil = false;
        }
    }
    
    [HarmonyPatch(typeof(Inventory))]
    [HarmonyPatch("AddItem", typeof(string), typeof(int), typeof(int), typeof(int), typeof(long), typeof(string), typeof(Vector2i), typeof(bool))]
    public static class InventoryAddItemPatch
    {
        [UsedImplicitly]
        public static void Postfix(Inventory __instance, ref ItemDrop.ItemData __result)
        {
            if (!IsCraftingSigil) return;
            
            Debug.Log("Attempting to craft sigil");
    
            ItemDrop.ItemData sigilItem = __result;
    
            ModifierRarity rarity = ModiferUtils.GetSigilRarityFromName(__result.m_dropPrefab.name);
            
            __result.m_dropPrefab.AddComponent<SigilComponent>();
            __result.m_dropPrefab.GetComponent<SigilComponent>().SetSigilModifiers(rarity);

            var modifiers = __result.m_dropPrefab.GetComponent<SigilComponent>().GetSigilModifiers();
            foreach (var modifier in modifiers)
            {
                Debug.Log($"Modifier with name: {modifier.ModifierType} added with value: {modifier.ModifierValue} to item with name {__result.m_dropPrefab.name}");
            }
        }
    }
}