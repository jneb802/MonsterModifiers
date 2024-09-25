using System;
using System.Collections.Generic;
using MonsterModifiers.Modifiers;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class MonsterModifiers : MonoBehaviour
{
   public List<MonsterModifierTypes> Modifiers = new List<MonsterModifierTypes>();

   public Character character;

   public int level;
   
   private void Start()
   {
      character = GetComponent<Character>();
      level = character.m_level;
      RollModifiers(level);
   }

   public void RollModifiers(int level)
   {
      foreach (i in levels)
      {
         
      }
      Modifiers.Add(MonsterModifierTypes.StaminaSiphon);
   }

   public void GetModifierWeight(MonsterModifierTypes modifier)
   {
      
   }
   
   public void GetAllModifiers(MonsterModifierTypes modifier)
   {
      
   }
   
}