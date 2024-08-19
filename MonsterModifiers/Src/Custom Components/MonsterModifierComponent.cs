using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public enum ModifierRarity
{
   Magic,
   Rare,
   Epic,
   Legendary,
   Mythic
}

public class MonsterModifierComponent
{
   public List<Modifier> Modifiers = new List<Modifier>();
   
}