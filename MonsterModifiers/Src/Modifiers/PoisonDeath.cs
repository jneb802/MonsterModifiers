// using HarmonyLib;
// using Jotunn.Managers;
// using UnityEngine;
//
// namespace MonsterModifiers.Modifiers;
//
// public class PoisonDeath
// {
//     [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
//     public class PoisonDeath_Character_OnDeath_Patch
//     {
//         public static void Postfix(Character __instance)
//         {
//             if (__instance == null || __instance.IsPlayer())
//             {
//                 return;  
//             }
//
//             var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
//             if (modiferComponent == null)
//             {
//                 return;
//             }
//
//             if (!modiferComponent.Modifiers.Contains(MonsterModifierTypes.PoisonDeath))
//             {
//                 return;
//             }
//
//             GameObject blobAttackAoe = ZNetScene.instance.GetPrefab("blob_attack_aoe");
//             GameObject blobAoe = ZNetScene.instance.GetPrefab("blob_aoe");
//             if (blobAoe != null && blobAttackAoe != null)
//             {
//                 int hashName = "$se_poison_name".GetStableHashCode();
//                 
//                 blobAoe.GetComponent<Aoe>().m_hitCharacters = true;
//                 blobAoe.GetComponent<Aoe>().m_hitEnemy = true;
//                 blobAoe.GetComponent<Aoe>().m_damage.m_poison = 90;
//                 
//                 blobAttackAoe.GetComponent<ItemDrop.ItemData>().m_shared.m_attackStatusEffect = ObjectDB.instance.GetStatusEffect(hashName);
//                 
//                 GameObject.Instantiate(blobAoe,__instance.transform.position, __instance.transform.rotation);
//                 GameObject.Instantiate(blobAttackAoe,__instance.transform.position, __instance.transform.rotation);
//             }
//
//         }
//     }
// }