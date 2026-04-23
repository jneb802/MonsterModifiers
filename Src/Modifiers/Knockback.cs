using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class Knockback
{
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public class Knockback_Character_RPC_Damage_Patch
    {
        // Prefix: set m_pushForce before RPC_Damage processes it via AddPushbackForce in CustomFixedUpdate
        public static void Prefix(Character __instance, HitData hit)
        {
            if (!ModifierUtils.RunRPCDamageChecks(__instance, hit))
                return;

            if (!ModifierUtils.RunHitChecks(hit, true))
                return;

            if (__instance.IsBlocking())
                return;

            Character attacker = hit.GetAttacker();
            if (attacker == null)
                return;

            var modifierComponent = attacker.GetComponent<Custom_Components.MonsterModifier>();
            if (modifierComponent == null)
                return;

            if (!modifierComponent.Modifiers.Contains(MonsterModifierTypes.Knockback))
                return;

            if (!__instance.IsPlayer())
                return;

            float staggerForce = MonsterModifiersPlugin.Cfg_Knockback_StaggerForce.Value;
            float pushForce = MonsterModifiersPlugin.Cfg_Knockback_PushForce.Value;

            // Trigger stagger animation via hit data
            hit.m_pushForce = staggerForce;

            // Set m_pushForce directly — Valheim's CustomFixedUpdate applies this via AddPushbackForce
            Vector3 knockDir = hit.m_dir == Vector3.zero ? attacker.GetLookDir() : hit.m_dir;
            knockDir.y = 0.3f;
            knockDir.Normalize();

            if (__instance.m_pushForce.magnitude < pushForce)
                __instance.m_pushForce = knockDir * pushForce;
        }
    }
}
