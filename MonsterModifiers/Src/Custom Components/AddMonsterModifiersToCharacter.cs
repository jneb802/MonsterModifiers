using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class AddMonsterModifiersToCharacter
{
    [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
    public static class Character_Awake_Patch
    {
        private static void Postfix(Character __instance)
        {
            if (!__instance.IsPlayer())
            {
                __instance.gameObject.AddComponent<MonsterModifiers>();
                Debug.Log("Monster Modifier component was added to creature with name " + __instance.m_name);
            }
        }
    }
}