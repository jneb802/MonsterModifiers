using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using MonsterModifiers.Custom_Components;

namespace MonsterModifiers.GamePatches;

public static class ItemDropPatch
{
    [HarmonyPatch(typeof(ItemDrop.ItemData))]
    [HarmonyPatch("GetTooltip", new[] {typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float) })]
    public static class ItemDrop_GetTooltip_Patch
    {
        public static void Postfix(ref string __result, ItemDrop.ItemData item, bool crafting)
        {
            if (!crafting)
            {
                var text = new StringBuilder(__result);
            
                if (item != null)
                {
                    if (item.m_dropPrefab.TryGetComponent<SigilComponent>(out SigilComponent sigilComponent))
                    {
        
                        var modifierTextList = new List<string>();
        
                        if (sigilComponent.m_sigilModifiers.Count > 0)
                        {
                            foreach (var modifier in sigilComponent.m_sigilModifiers)
                            {
                                string modifierName = ModiferUtils.GetModifierDisplayName(modifier.ModifierType);
                                float modifierValue = modifier.ModifierValue;
        
                                string modifierTextString = $"\n<color=#00ffffff>{modifierName}: {modifierValue}</color>";
                                modifierTextList.Add(modifierTextString);
                            }
        
                            foreach (var modifierText in modifierTextList)
                            {
                                text.Append(modifierText);
                            }
                            
                            text.Append(modifierTextList);
                        }
                    }
                }
            
                __result = text.ToString(); 
            }
        }
    }
}