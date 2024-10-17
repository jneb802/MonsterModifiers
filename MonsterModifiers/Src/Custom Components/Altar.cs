using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class Altar : MonoBehaviour
{
    public Switch m_incinerateSwitch;
    public Container m_container;
    public ZNetView m_nview;

    public void Awake()
    {
        this.m_incinerateSwitch.m_onUse += new Switch.Callback(this.OnOffering);
        this.m_incinerateSwitch.m_onHover += new Switch.TooltipCallback(this.GetLeverHoverText);
        this.m_nview = this.GetComponent<ZNetView>();
        if ((UnityEngine.Object)this.m_nview == (UnityEngine.Object)null || this.m_nview.GetZDO() == null)
            return;
    }

    public bool OnOffering(Switch sw, Humanoid user, ItemDrop.ItemData item)
    {
        if (!this.m_nview.IsValid() || !this.m_nview.HasOwner() || !PrivateArea.CheckAccess(this.transform.position))
            return false;
        
        Altar altar = this;
        Inventory inventory = altar.m_container.GetInventory();
        ItemDrop.ItemData sigilObject = inventory.GetItem(0);
        if (sigilObject ==  null)
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Player and invoked custom altar but inventory is empty");
            return false;
        }
        if (!sigilObject.m_shared.m_name.Contains("item_sigil"))
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Offered item is not a sigil");
            return false;
        }
        
        DungeonGenerator dungeonGenerator = DungeonGenUtils.GetDungeonInterior(this.gameObject);
        if (dungeonGenerator == null)
        {
            Debug.Log("Dungeon Generator is null");
            return false;
        }
        
        // DungeonGenUtils.DeleteDungeon(dungeonGenerator);
        // int randomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        // dungeonGenerator.Clear();
        // dungeonGenerator.Generate(randomSeed, ZoneSystem.SpawnMode.Full);  // Adjust SpawnMode as needed
        
        List<MonsterModifierTypes> modifierList = new List<MonsterModifierTypes>();
        if (sigilObject.m_customData.TryGetValue(Sigil.CustomDataKey, out string serializedModifiers))
        {
            modifierList = new List<MonsterModifierTypes>(
                Array.ConvertAll(serializedModifiers.Split(','), 
                    str => (MonsterModifierTypes)Enum.Parse(typeof(MonsterModifierTypes), str))
            );
            
            // Debug or log each modifier's name
            foreach (var modifier in modifierList)
            {
                // Log the modifier name
                Debug.Log($"Modifier: {modifier}");
            }
        }
        else
        {
            Debug.LogWarning("No modifiers found in m_customData.");
        }
        
        List<Character> interiorCharactersList = WorldUtils.GetAllCharacter(dungeonGenerator.transform.position, 64f);

        if (interiorCharactersList.Count == 0)
        {
            List<CreatureSpawner> interiorCreatureSpawnerList = WorldUtils.GetCreatureSpawnersInDungeon(dungeonGenerator);
            Debug.Log("There are " + interiorCreatureSpawnerList.Count + " creature spawners in the dungeon");

            foreach (var creatureSpawner in interiorCreatureSpawnerList)
            {
                Debug.Log("Attempting to do manual spawn using creature spawner");
                creatureSpawner.gameObject.SetActive(true);
                if (creatureSpawner != null && creatureSpawner.gameObject.activeSelf)
                {
                    creatureSpawner.Awake();
                    if (creatureSpawner.m_nview != null)
                    {
                        if (creatureSpawner.m_nview.GetZDO() != null)
                        {
                            if (creatureSpawner.CheckGlobalKeys())
                            {
                                creatureSpawner.m_levelupChance = 100f;
                                creatureSpawner.m_minLevel = modifierList.Count + 1;
                                creatureSpawner.m_maxLevel = modifierList.Count + 1;
                                creatureSpawner.Spawn();
                            }
                        }
                    }
                    Debug.Log("Successfully manually spawned using creature spawner"); 
                }
            }
            interiorCharactersList = WorldUtils.GetAllCharacter(dungeonGenerator.transform.position, 64f);
        }
        
        if (interiorCharactersList.Count > 0)
        {
            Debug.Log("Dungeon has " + interiorCharactersList.Count + "characters");
            foreach (var character in interiorCharactersList)
            {
                character.SetLevel(modifierList.Count + 1);
                MonsterModifier monsterModifier = character.GetComponentInParent<MonsterModifier>();
                if (monsterModifier != null)
                {
                    Debug.Log("Monster has modifier component");
                    if (character.m_level == modifierList.Count + 1)
                    {
                        Debug.Log("Monster is the correct level modifier component");
                        monsterModifier.ForceStart();
                        monsterModifier.ChangeModifiers(modifierList, modifierList.Count);
                    }
                }
                else
                {
                    Debug.Log("Monster does not have modifier component");
                    character.gameObject.AddComponent<MonsterModifier>().ChangeModifiers(modifierList, modifierList.Count);
                }
            }
        }
        
        inventory.RemoveAll();
        return true;
    }
    
    public string GetLeverHoverText()
    {
        return !PrivateArea.CheckAccess(this.transform.position) ? Localization.instance.Localize("$piece_atlar\n$piece_noaccess") : Localization.instance.Localize("[<color=yellow><b>$KEY_Use</b></color>] $piece_pulllever");
    }
    

}