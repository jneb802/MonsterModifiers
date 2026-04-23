using HarmonyLib;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class AddMonsterModifiersToCharacter
{
    [HarmonyPriority(Priority.First)]
    [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
    public static class Character_Awake_Patch
    {
        private static void Postfix(Character __instance)
        {
            if (!__instance.IsPlayer() && !__instance.IsBoss())
            {
                __instance.gameObject.AddComponent<MonsterModifier>();
            }
        }
    }

    // 소환 직후 Animator 미초기화 상태에서 ExtraStats 등 외부 모드가 접근할 때
    // NullReferenceException 방지용 가드
    [HarmonyPatch(typeof(Character), "UpdateCachedAnimHashes")]
    public static class Character_UpdateCachedAnimHashes_Guard
    {
        private static bool Prefix(Character __instance)
        {
            return __instance.m_animator != null && __instance.m_animator.isInitialized;
        }
    }
}