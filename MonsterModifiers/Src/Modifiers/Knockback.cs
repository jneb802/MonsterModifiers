using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class Knockback
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class Knockback_Character_RPC_Damage_Patch
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (hit == null || __instance == null) return;
            if (hit.m_damage.GetTotalDamage() == 0) return;
            if (!__instance.IsPlayer()) return;

            var attacker = hit.GetAttacker();
            if (attacker == null || attacker.IsPlayer()) return;
            if (__instance.IsBlocking()) return;

            var modiferComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null || !modiferComponent.Modifiers.Contains(MonsterModifierTypes.Knockback))
                return;

            hit.m_pushForce = MonsterModifiersPlugin.Cfg_Knockback_StaggerForce.Value;

            Vector3 knockDir = hit.m_dir;
            if (knockDir.sqrMagnitude < 0.01f)
                knockDir = (__instance.transform.position - attacker.transform.position).normalized;
            knockDir.y = 0.3f;
            knockDir.Normalize();

            float forceMagnitude = MonsterModifiersPlugin.Cfg_Knockback_PushForce.Value;
            if (__instance.m_pushForce.magnitude < forceMagnitude)
                __instance.m_pushForce = knockDir * forceMagnitude;
        }
    }
}
