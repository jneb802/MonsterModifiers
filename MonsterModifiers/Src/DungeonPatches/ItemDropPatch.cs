using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.GamePatches;

public static class ItemDropPatch
{
    [HarmonyPatch(typeof(ItemDrop.ItemData))]
    [HarmonyPatch("GetTooltip", new[] { typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float) })]
    public static class ItemDrop_GetTooltip_Patch
    {
        public static void Postfix(ref string __result, ItemDrop.ItemData item, bool crafting)
        {
            if (!crafting && item != null)
            {
                var text = new StringBuilder(__result);

                // Check if the item has Sigil modifiers stored in m_customData
                if (item.m_customData.TryGetValue(Sigil.CustomDataKey, out string serializedModifiers))
                {
                    // Deserialize the modifiers from the stored string
                    string[] modifiers = serializedModifiers.Split(',');
                    var modifierTextList = new List<string>();

                    foreach (var modifierName in modifiers)
                    {
                        if (Enum.TryParse(modifierName, out MonsterModifierTypes modifier))
                        {
                            string modifierTextString = $"\n<color=#00ffffff>{modifierName}</color>";
                            modifierTextList.Add(modifierTextString);
                        }
                    }

                    // Append the modifiers to the tooltip text
                    foreach (var modifierText in modifierTextList)
                    {
                        text.Append(modifierText);
                    }

                    // Debug.Log("Added Sigil modifiers to tooltip: " + string.Join(", ", modifiers));
                }
                else
                {
                    // Debug.Log("This itemData does not have Sigil modifiers in m_customData");
                }

                __result = text.ToString(); // Update the result with the new tooltip
            }
        }
    }
}