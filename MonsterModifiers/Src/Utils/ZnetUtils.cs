// using System.Collections.Generic;
// using HarmonyLib;
// using UnityEngine;
//
// namespace MonsterModifiers;
//
// using HarmonyLib;
// using UnityEngine;
// using System.Collections.Generic;
//
// [HarmonyPatch(typeof(ZNetScene))]
// [HarmonyPatch("RemoveObjects")]
// public class RemoveObjectsPatch
// {
//     static void Postfix(ZNetScene __instance, List<ZDO> currentNearObjects, List<ZDO> currentDistantObjects)
//     {
//         // Logging null issues in currentNearObjects
//         if (currentNearObjects == null)
//         {
//             Debug.LogError("RemoveObjects: currentNearObjects is null");
//         }
//         else
//         {
//             foreach (ZDO zdo in currentNearObjects)
//             {
//                 if (zdo == null)
//                 {
//                     Debug.LogError("RemoveObjects: A ZDO in currentNearObjects is null");
//                 }
//             }
//         }
//
//         // Logging null issues in currentDistantObjects
//         if (currentDistantObjects == null)
//         {
//             Debug.LogError("RemoveObjects: currentDistantObjects is null");
//         }
//         else
//         {
//             foreach (ZDO zdo in currentDistantObjects)
//             {
//                 if (zdo == null)
//                 {
//                     Debug.LogError("RemoveObjects: A ZDO in currentDistantObjects is null");
//                 }
//             }
//         }
//
//         // Logging null issues in m_tempRemoved and m_instances
//         if (__instance.m_tempRemoved == null)
//         {
//             Debug.LogError("RemoveObjects: m_tempRemoved is null");
//         }
//
//         if (__instance.m_instances == null)
//         {
//             Debug.LogError("RemoveObjects: m_instances is null");
//         }
//         else
//         {
//             int index = 0;
//             foreach (ZNetView znetView in __instance.m_instances.Values)
//             {
//                 if (znetView == null)
//                 {
//                     Debug.LogError($"RemoveObjects: ZNetView at index {index} in m_instances is null");
//                     index++;
//                     continue;
//                 }
//
//                 ZDO zdo = znetView.GetZDO();
//                 if (zdo == null)
//                 {
//                     Debug.LogError($"RemoveObjects: ZNetView '{znetView.name}' has a null ZDO at index {index}");
//                     index++;
//                     continue;
//                 }
//
//                 if (znetView.gameObject == null)
//                 {
//                     Debug.LogError($"RemoveObjects: ZNetView '{znetView.name}' has a null GameObject at index {index}");
//                 }
//
//                 index++;
//             }
//         }
//     }
// }
