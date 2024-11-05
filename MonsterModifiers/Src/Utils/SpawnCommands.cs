using HarmonyLib;
using System;
using System.Collections.Generic;
using MonsterModifiers.Custom_Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterModifiers
{
    public static class SpawnCommands
    {
        /// <summary>
        /// Attempts to reset all locations in range.
        /// </summary>
        /// <param name="range"></param>
        public static void SpawnModifier(string creatureName, string modifierName)
        {
            GameObject prefab = ZNetScene.instance.GetPrefab(creatureName);
            if (prefab == null)
            {
                Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Missing object " + creatureName, 0, (Sprite)null);
                return;
            }
            
            if (Enum.TryParse(modifierName, true, out MonsterModifierTypes modifierType))
            {
                Vector3 vector3 = UnityEngine.Random.insideUnitSphere;
                GameObject spawnedGameObject = ZNetScene.Instantiate<GameObject>(
                    prefab, 
                    Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up + vector3, 
                    Quaternion.identity
                );

                Character character = spawnedGameObject.GetComponent<Character>();
                
                if (character.m_nview.GetZDO().IsOwner())
                {
                    character.SetLevel(2);
                    string serializedModifiers = string.Join(",", modifierName);
                    character.m_nview.GetZDO().Set("modifiers", serializedModifiers);
                }
            }
            else
            {
                Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Invalid modifier name: " + modifierName, 0, (Sprite)null);
            }
        }


        [HarmonyPatch(typeof(Terminal), nameof(Terminal.InitTerminal))]
        private static class Patch_Terminal_InitTerminal
        {
            [HarmonyPriority(Priority.First)]
            private static void Prefix(out bool __state)
            {
                __state = Terminal.m_terminalInitialized;
            }

            private static void Postfix(bool __state)
            {
                if (__state)
                {
                    return;
                }

                MonsterModifiersPlugin.MonsterModifiersLogger.LogInfo("Adding Terminal Commands for monster modifier spawning.");

                new Terminal.ConsoleCommand("modifier", "[creature] [modifier]", delegate (Terminal.ConsoleEventArgs args)
                {
                    if (args.Length > 2)
                    {
                        string creatureName = args[1];
                        string modifierName = args[2];
                        SpawnModifier(creatureName, modifierName);
                    }
                    else
                    {
                        args.Context.AddString("Usage: modifier [creature] [modifier]");
                    }
                }, isCheat: true, isNetwork: false, onlyServer: false);
            }
        }
    }
}