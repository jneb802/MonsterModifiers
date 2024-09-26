using HarmonyLib;
using JetBrains.Annotations;

namespace MonsterModifiers.Modifiers;

public class StaminaSiphon
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class StaminaSiphon_Character_RPC_Damage_Patch
    {
        public static void Postfix(Character __instance, HitData hit)
        {
            if (hit == null || __instance == null)
            {
                return;  
            }
            
            if (hit.m_damage.GetTotalDamage() == 0)
            {
                return;
            }

            var attacker = hit.GetAttacker();

            if (attacker.IsPlayer() || attacker == null)
            {
                return;
            }

            var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.StaminaSiphon))
            {
                return;
            }
            
            __instance.UseStamina(hit.GetTotalDamage());
        }
    }
}