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

        DungeonGenerator dungeonGenerator = DungeonGenUtils.GetDungeonInterior(this.gameObject);
        if (dungeonGenerator == null)
        {
            Debug.Log("Dungeon Generator is null");
            return false;
        }
        
        List<MonsterModifierTypes> modifierList = new List<MonsterModifierTypes>();
        if (sigilObject.m_customData.TryGetValue(Sigil.CustomDataKey, out string serializedModifiers))
        {
            modifierList = new List<MonsterModifierTypes>(
                Array.ConvertAll(serializedModifiers.Split(','), 
                    str => (MonsterModifierTypes)Enum.Parse(typeof(MonsterModifierTypes), str))
            );
        }
        
        List<Character> interiorCharactersList = WorldUtils.GetAllCharacter(dungeonGenerator.transform.position, 64f);
        if (interiorCharactersList == null)
        {
            Debug.Log("interiorCharacterList is null");
            return false;
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
                    monsterModifier.ChangeModifiers(modifierList, modifierList.Count);
                }
                
            }
        }
        
        inventory.RemoveAll();
        return true;
    }
    
    public string GetLeverHoverText()
    {
        return !PrivateArea.CheckAccess(this.transform.position) ? Localization.instance.Localize("$piece_incinerator\n$piece_noaccess") : Localization.instance.Localize("[<color=yellow><b>$KEY_Use</b></color>] $piece_pulllever");
    }
    

}