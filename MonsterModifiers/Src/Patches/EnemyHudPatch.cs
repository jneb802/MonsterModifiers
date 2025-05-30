using System.Collections.Generic;
using System.Linq.Expressions;
using HarmonyLib;
using MonsterModifiers.Modifiers;
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

			Custom_Components.MonsterModifier monsterModifier = c.GetComponent<Custom_Components.MonsterModifier>();
			if (monsterModifier == null)
			{
				return;
			}

			ChangeEnemyStars(c, monsterModifier.Modifiers);

		}


		public static void ChangeEnemyStars(Character character, List<MonsterModifierTypes> modifiers)
		{
			if (character.GetLevel() <= 1 || character.IsBoss() ||
			    !EnemyHud.instance.m_huds.TryGetValue(character, out var value))
			{
				return;
			}
			
			// Debug.Log("Count of modifiers is " + modifiers.Count);

			GameObject creatureGUI = value.m_gui;
			// Debug.Log("Name of creatureGUI is " + creatureGUI.name);

			int startPosition = 0;
			
			// The maximum stars StarLevelsExpanded will allow on a creature HUD before truncated to a new star display format.
			int starLevelsExpandedMaxStarOnHud = 7;
			
			if (CompatibilityUtils.isStarLevelsExpandedInstalled && character.GetLevel() > starLevelsExpandedMaxStarOnHud)
			{
				startPosition = 2;
			}
			
			for (int i = startPosition; i < creatureGUI.transform.childCount; i++)
			{
				Transform child = creatureGUI.transform.GetChild(i);
				if (child.name.StartsWith("level_"+(modifiers.Count+1)) || child.name.StartsWith("level_n") && child.gameObject.activeSelf)
				{
					// Debug.Log("Name of child at high index " + i + " is " + child.name);
					for (int j = 0; j < child.transform.childCount; j++) {
						Transform child2 = child.transform.GetChild(j);
						// Only modify stars, dont care about other componets in here
						if (child2.name.StartsWith("star") && child2.gameObject.activeSelf) {
                            child2.GetComponent<Image>().sprite =
                            ModifierUtils.GetModifierIcon(modifiers[Mathf.Min(j, character.GetLevel() - 2)]);
                            child2.GetComponent<Image>().color =
                                ModifierUtils.GetModifierColor(modifiers[Mathf.Min(j, character.GetLevel() - 2)]);
                            child2.GetChild(0).gameObject.SetActive(false);
                        }
					}
				}
			}
			
		}
	}
}
