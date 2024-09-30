using System.Collections.Generic;
using UnityEngine;

namespace MonsterModifiers;

public class WorldUtils
{
    public static List<Character> GetAllCharacter(Vector3 position, float range)
    {
        Collider[] hits = Physics.OverlapBox(position, Vector3.one * range, Quaternion.identity);
        List<Character> characters = new List<Character>();

        foreach (var hit in hits)
        {
            var npc = hit.transform.root.gameObject.GetComponentInChildren<Character>();
            if (npc != null)
            {
                characters.Add(npc);
            }
        }

        return characters;
    }
}