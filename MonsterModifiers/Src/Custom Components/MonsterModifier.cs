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
               // Debug.Log("Adding modifier: " + modifier.ToString() + " to monster with name: " + character.m_name);
            }
            
            if (character.m_nview.GetZDO().IsOwner())
            {
               string serializedModifiers = string.Join(",", Modifiers);
               character.m_nview.GetZDO().Set("modifiers", serializedModifiers);
               // Debug.Log("Adding modifiers to monster with name " + character.m_name);
            }
         }
         else
         {
            Modifiers = new List<MonsterModifierTypes>(Array.ConvertAll(modifiersString.Split(','), 
               str => (MonsterModifierTypes)Enum.Parse(typeof(MonsterModifierTypes), str)));
         }

         ApplyStartModifiers();

      }
   }

   public void ChangeModifiers(List<MonsterModifierTypes> modifierTypesList, int numModifiers)
   {
      if (level > 1)
      {
         foreach (var modifier in modifierTypesList)
         {
            Modifiers.Add(modifier);
            Debug.Log("Monster with name " + character.name + " has has changed modifiers. New modifier: " + modifier);
         }
         
         if (character.m_nview.GetZDO().IsOwner())
         {
            string serializedModifiers = string.Join(",", Modifiers);
            character.m_nview.GetZDO().Set("modifiers", serializedModifiers);
         }
      }
   }

   public void ApplyStartModifiers()
   {
      if (Modifiers.Contains(MonsterModifierTypes.PersonalShield))
      {
         PersonalShield.AddPersonalShield(character);
      }
         
      if (Modifiers.Contains(MonsterModifierTypes.ShieldDome))
      {
         var shieldDome = character.gameObject.AddComponent<ShieldDome>();
         shieldDome.AddShieldDome(character);
      }
         
      if (Modifiers.Contains(MonsterModifierTypes.StaggerImmune))
      {
         StaggerImmune.AddStaggerImmune(character);
      }
         
      if (Modifiers.Contains(MonsterModifierTypes.FastMovement))
      {
         FastMovement.AddFastMovement(character);
      }
         
      if (Modifiers.Contains(MonsterModifierTypes.DistantDetection))
      {
         DistantDetection.AddDistantDetection(character);
      }
   }
}