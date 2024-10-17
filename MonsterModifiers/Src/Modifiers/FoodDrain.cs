using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class FoodDrain
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class FoodDrain_Character_RPC_Damage_Patch
    {
        public static void Postfix(Character __instance, HitData hit)
        {
            if (!ModifierUtils.RunRPCDamageChecks(__instance,hit))
            {
                return;
            }
            
            if (!ModifierUtils.RunHitChecks(hit, true))
            {
                return;
            }

            if (__instance.IsBlocking())
            {
                return;
            }

            var attacker = hit.GetAttacker();
            var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.FoodDrain))
            {
                return;
            }

            Player player = __instance as Player;
            if (player != null)
            {
                List<Player.Food> playerFoods = player.GetFoods();
                int foodCount = playerFoods.Count;
                if (foodCount > 0)
                {
                    int randomNumber = Random.Range(0, foodCount);
                    playerFoods[randomNumber].m_time *= 0.5f;
                }
            }
        }
    }
}