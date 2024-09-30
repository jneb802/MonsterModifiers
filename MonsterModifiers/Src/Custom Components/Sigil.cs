using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace MonsterModifiers.Custom_Components
{
    public class Sigil : MonoBehaviour
    {
        public List<MonsterModifierTypes> m_sigilModifiers = new List<MonsterModifierTypes>();
        
        public ItemDrop m_itemDrop;
        
        public const string CustomDataKey = "sigilModifiers";

        public void Awake()
        {
            m_itemDrop = GetComponent<ItemDrop>();
            LoadModifiersFromCustomData();
            if (m_sigilModifiers.Count == 0)
            {
                RollSigilModifiers();
            }
            Debug.Log("Sigil called awake");
        }

        public List<MonsterModifierTypes> GetSigilModifiers()
        {
            return m_sigilModifiers;
        }
        
        public void RollSigilModifiers()
        {
            m_sigilModifiers = ModifierUtils.RollRandomModifiers(2);
            SaveModifiersToCustomData(); // Save the rolled modifiers to m_customData
            foreach (var modifier in m_sigilModifiers)
            {
                string modifierName = modifier.ToString();
                Debug.Log("Added modifier with name " + modifierName + " to sigil");
            }
        }
        
        private void SaveModifiersToCustomData()
        {
            // Serialize the modifiers list into a comma-separated string
            string serializedModifiers = string.Join(",", m_sigilModifiers);
            m_itemDrop.m_itemData.m_customData[CustomDataKey] = serializedModifiers;
        }

        private void LoadModifiersFromCustomData()
        {
            if (m_itemDrop.m_itemData.m_customData.TryGetValue(CustomDataKey, out string serializedModifiers))
            {
                // Deserialize the string back into a list of modifiers
                string[] modifiers = serializedModifiers.Split(',');
                m_sigilModifiers = new List<MonsterModifierTypes>();
                foreach (var modifierName in modifiers)
                {
                    if (Enum.TryParse(modifierName, out MonsterModifierTypes modifier))
                    {
                        m_sigilModifiers.Add(modifier);
                    }
                }
                // Debug.Log("Loaded modifiers from m_customData: " + serializedModifiers);
            }
        }

        public void AddModifier(MonsterModifierTypes modifier)
        {
            m_sigilModifiers.Add(modifier);
            MonsterModifiersPlugin.MonsterModifiersLogger.LogDebug("Added modifier with name: " + modifier + " to Sigil Component");
        }
        
    }
}