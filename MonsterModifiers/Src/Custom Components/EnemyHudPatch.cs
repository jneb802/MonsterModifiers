using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MonsterModifiers.Patches
{
    [HarmonyPatch(typeof(EnemyHud), "ShowHud")]
    public static class EnemyHud_ShowHud_Patch
    {
        private static void Postfix(EnemyHud __instance, Character c, bool isMount)
        {
            if (c.IsPlayer() || isMount)
            {
                return;
            }
            
            Custom_Components.MonsterModifiers monsterModifiers = c.GetComponent<Custom_Components.MonsterModifiers>();
            if (monsterModifiers == null)
            {
                return;
            }

            ChangeEnemyStarColor(c, Color.green);

        }
        
    private static void ChangeEnemyStarColor(Character character, Color color)
	{
		if (character.GetLevel() <= 1 || character.IsBoss() || !EnemyHud.instance.m_huds.TryGetValue(character, out var value))
		{
			return;
		}
        
        GameObject creatureGUI = value.m_gui;
        
		for (int i = 0; i < creatureGUI.transform.childCount; i++)
		{
			Transform child = creatureGUI.transform.GetChild(i);
			for (int j = 0; j < child.transform.childCount; j++)
			{
				Transform child2 = child.transform.GetChild(j);
				if (child2.name.StartsWith("star"))
				{
					child2.GetChild(0).GetComponent<Image>().color = color;
				}
			}
		}
	}
        
    private static Color GetModifierColor(MonsterModifierTypes modifier)
    {
        switch (modifier)
        {
            case MonsterModifierTypes.StaminaSiphon:
                return Color.green;
            default:
                return Color.white;
        }
    }
    }
}
