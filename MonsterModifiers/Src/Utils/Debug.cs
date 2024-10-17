// using System;
// using HarmonyLib;
// using UnityEngine;
//
// namespace MonsterModifiers;
//
// [HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Spawn))]
// public static class SpawnPatch
// {
//     [HarmonyPrefix]
//     public static bool SpawnPrefix(ref ZNetView __result, CreatureSpawner __instance)
//     {
//         try
//         {
//             Debug.Log("Spawn method started");
//
//             // Log the position of the spawner
//             Vector3 position = __instance.transform.position;
//             Debug.Log($"Position: {position}");
//
//             // Log the floor height check
//             float height;
//             if (ZoneSystem.instance.FindFloor(position, out height))
//             {
//                 position.y = height;
//                 Debug.Log($"Found floor at height: {height}");
//             }
//             else
//             {
//                 Debug.LogWarning("Failed to find floor for position.");
//             }
//
//             // Log rotation information
//             Quaternion rotation = Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360f), 0.0f);
//             Debug.Log($"Rotation: {rotation.eulerAngles}");
//
//             // Log object instantiation
//             GameObject spawnedObject = UnityEngine.Object.Instantiate(__instance.m_creaturePrefab, position, rotation);
//             Debug.Log($"Spawned object: {spawnedObject?.name}");
//
//             // Check if the spawned object or prefab is null
//             if (spawnedObject == null)
//             {
//                 Debug.LogError("Spawned object is null.");
//                 return true; // Exit if null to avoid further errors
//             }
//
//             // Log ZNetView component retrieval
//             ZNetView component1 = spawnedObject.GetComponent<ZNetView>();
//             if (component1 == null)
//             {
//                 Debug.LogError("ZNetView component is null.");
//                 return true; // Exit if null
//             }
//             Debug.Log($"ZNetView component: {component1}");
//
//             // Log wake-up animation
//             if (__instance.m_wakeUpAnimation)
//             {
//                 var zSyncAnim = spawnedObject.GetComponent<ZSyncAnimation>();
//                 Debug.Log($"ZSyncAnimation component: {zSyncAnim}");
//                 if (zSyncAnim != null)
//                 {
//                     zSyncAnim.SetBool("wakeup", true);
//                     Debug.Log("Wakeup animation set");
//                 }
//                 else
//                 {
//                     Debug.LogWarning("ZSyncAnimation component is null.");
//                 }
//             }
//
//             // Log BaseAI component retrieval
//             BaseAI component2 = spawnedObject.GetComponent<BaseAI>();
//             if (component2 == null)
//             {
//                 Debug.LogWarning("BaseAI component is null.");
//             }
//             else
//             {
//                 Debug.Log($"BaseAI component: {component2}");
//
//                 // Log patrol point setting
//                 if (__instance.m_setPatrolSpawnPoint)
//                 {
//                     component2.SetPatrolPoint();
//                     Debug.Log("Patrol point set");
//                 }
//             }
//
//             // Log level setup
//             int minLevel = __instance.m_minLevel;
//             int maxLevel = __instance.m_maxLevel;
//             float levelUpChanceOverride = __instance.m_levelupChance;
//
//             Debug.Log($"Min level: {minLevel}, Max level: {maxLevel}, Level up chance: {levelUpChanceOverride}");
//
//             if (__instance.m_location != null && !__instance.m_location.m_excludeEnemyLevelOverrideGroups.Contains(__instance.m_spawnGroupID))
//             {
//                 if (__instance.m_location.m_enemyMinLevelOverride >= 0)
//                 {
//                     minLevel = __instance.m_location.m_enemyMinLevelOverride;
//                     Debug.Log($"Min level overridden: {minLevel}");
//                 }
//
//                 if (__instance.m_location.m_enemyMaxLevelOverride >= 0)
//                 {
//                     maxLevel = __instance.m_location.m_enemyMaxLevelOverride;
//                     Debug.Log($"Max level overridden: {maxLevel}");
//                 }
//
//                 if (__instance.m_location.m_enemyLevelUpOverride >= 0.0f)
//                 {
//                     levelUpChanceOverride = __instance.m_location.m_enemyLevelUpOverride;
//                     Debug.Log($"Level up chance overridden: {levelUpChanceOverride}");
//                 }
//             }
//
//             // Log character level/quality adjustment
//             if (maxLevel > 1)
//             {
//                 Character component3 = spawnedObject.GetComponent<Character>();
//                 if (component3 != null)
//                 {
//                     int level = minLevel;
//                     Debug.Log($"Character component found. Starting level: {level}");
//                     while (level < maxLevel && UnityEngine.Random.Range(0.0f, 100f) <= SpawnSystem.GetLevelUpChance(levelUpChanceOverride))
//                     {
//                         level++;
//                         Debug.Log($"Level increased to: {level}");
//                     }
//                     if (level > 1)
//                     {
//                         component3.SetLevel(level);
//                         Debug.Log($"Set character level to: {level}");
//                     }
//                 }
//                 else
//                 {
//                     ItemDrop component4 = spawnedObject.GetComponent<ItemDrop>();
//                     if (component4 != null)
//                     {
//                         int quality = minLevel;
//                         Debug.Log($"ItemDrop component found. Starting quality: {quality}");
//                         while (quality < maxLevel && UnityEngine.Random.Range(0.0f, 100f) <= levelUpChanceOverride)
//                         {
//                             quality++;
//                             Debug.Log($"Quality increased to: {quality}");
//                         }
//                         if (quality > 1)
//                         {
//                             component4.SetQuality(quality);
//                             Debug.Log($"Set item quality to: {quality}");
//                         }
//                     }
//                 }
//             }
//
//             // Log ZDO checks and actions
//             if (__instance.m_nview == null)
//             {
//                 Debug.LogError("m_nview is null on the CreatureSpawner instance.");
//                 return true; // Exit if null
//             }
//
//             Debug.Log("m_nview is valid.");
//
//             var zdo = __instance.m_nview.GetZDO();
//             if (zdo == null)
//             {
//                 Debug.LogError("ZDO is null on m_nview.");
//                 return true; // Exit if null
//             }
//
//             zdo.SetConnection(ZDOExtraData.ConnectionType.Spawned, component1.GetZDO().m_uid);
//             Debug.Log("Set connection on ZDO");
//
//             zdo.Set(ZDOVars.s_aliveTime, ZNet.instance.GetTime().Ticks);
//             Debug.Log("Set alive time on ZDO");
//
//             // Log spawn effect
//             __instance.SpawnEffect(spawnedObject);
//             Debug.Log("Spawn effect triggered");
//
//             // Return the result
//             __result = component1;
//             Debug.Log("Spawn method completed successfully");
//
//             return false; // Skip original method
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error in Spawn method: {ex}");
//             return true; // Fallback to original method in case of error
//         }
//     }
// }
