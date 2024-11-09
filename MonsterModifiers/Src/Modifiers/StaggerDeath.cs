using System.Collections.Generic;
using HarmonyLib;
using Jotunn.Managers;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class StaggerDeath
{
    [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
    public class StaggerDeath_Character_OnDeath_Patch
    {
        public static void Prefix(Character __instance)
        {
            if (__instance == null)
            {
                return;
            }
            
            if (__instance.IsPlayer())
            {
                return;
            }
            
            if (__instance.m_nview == null)
            {
                return;
            }

            if (__instance.gameObject.name == "mistleCustomPrefab(Clone)")
            {
                Vector3 characterPosition = __instance.transform.position;
                GameObject mistileNova = PrefabManager.Instance.GetPrefab("staggerDeathNovaCustomPrefab");

                if (mistileNova != null)
                {
                    GameObject.Instantiate(mistileNova,
                        new Vector3(
                            __instance.transform.position.x,
                            __instance.transform.position.y - 1f,
                            __instance.transform.position.z
                        ),
                        __instance.transform.rotation);

                    List<Character> characters = WorldUtils.GetAllCharacter(__instance.transform.position, 5f);
                    foreach (var character in characters)
                    {
                        if (character == __instance || character == null)
                        {
                            continue;
                        }

                        if (character.m_nview == null || character.IsPlayer())
                        {
                            continue;
                        }

                        character.Stagger(characterPosition);

                    }

                    List<Player> nearbyPlayers = new List<Player>();
                    Player.GetPlayersInRange(__instance.transform.position, 5f, nearbyPlayers);
                    foreach (Character character in nearbyPlayers)
                    {
                        if (character == null || character.m_nview == null)
                        {
                            continue;
                        }

                        character.Stagger(characterPosition);
                    }
                }
            }
        }
    }
}