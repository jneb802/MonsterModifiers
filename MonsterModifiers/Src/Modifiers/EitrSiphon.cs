using HarmonyLib;
using JetBrains.Annotations;

namespace MonsterModifiers.Modifiers;

public class EitrSiphon
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class EitrSiphon_Character_RPC_Damage_Patch
    {
        public static void Postfix(Character __instance, HitData hit)
        {
            if (!ModifierUtils.RunRPCDamageChecks(__instance,hit))
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

            if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.EitrSiphon))
            {
                return;
            }
            
            __instance.UseEitr(hit.GetTotalDamage());
        }
    }
}