using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MonsterModifiers.Modifiers;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class MonsterModifier : MonoBehaviour
{
   public List<MonsterModifierTypes> Modifiers = new List<MonsterModifierTypes>();

   public Character character;

   public int level;
   
   private void Start()
   {
      character = GetComponent<Character>();
      level = character.GetLevel();
      
      if (level > 1)
      {
         string modifiersString = character.m_nview.GetZDO().GetString("modifiers", string.Empty);
         if (string.IsNullOrEmpty(modifiersString))
         {
            foreach (var modifier in ModifierUtils.RollRandomModifiers(level - 1))
            {
               Modifiers.Add(modifier);
               Debug.Log("Adding modifier: " + modifier.ToString() + " to monster with name: " + character.m_name);
            }
            
            if (character.m_nview.GetZDO().IsOwner())
            {
               string serializedModifiers = string.Join(",", Modifiers);
               character.m_nview.GetZDO().Set("modifiers", serializedModifiers);
               Debug.Log("Adding modifiers to monster with name " + character.m_name);
            }
         }
         else
         {
            Modifiers = new List<MonsterModifierTypes>(Array.ConvertAll(modifiersString.Split(','), 
               str => (MonsterModifierTypes)Enum.Parse(typeof(MonsterModifierTypes), str)));
         }

         if (Modifiers.Contains(MonsterModifierTypes.PersonalShield))
         {
            PersonalShield.AddPersonalShield(character);
         }
         
         if (Modifiers.Contains(MonsterModifierTypes.ShieldDome))
         {
            var shieldDome = character.gameObject.AddComponent<ShieldDome>();
            shieldDome.AddShieldDome(character);
         }
      }
   }
}