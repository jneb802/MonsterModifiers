using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Jotunn;
using Jotunn.Managers;
using MonsterModifiers.StatusEffects;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class DeathSpawns
{
    public static void ApplyDamageToNearbyPlayers(Vector3 position, HitData hit)
    {
        List<Player> nearbyPlayers = new List<Player>();
        Player.GetPlayersInRange(position, 5f, nearbyPlayers);
        foreach (var player in nearbyPlayers)
        {
            player.Damage(hit);
        }
    }
    
    [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
    public class DeathSpawns_Character_OnDeath_Patch
    {
        public static void Prefix(Character __instance)
        {
            if (__instance == null || __instance.IsPlayer())
            {
                return;
            }

            var modiferComponent = __instance.GetComponent<Custom_Components.MonsterModifier>();
            if (modiferComponent == null)
            {
                return;
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.PoisonDeath))
            {
                float deathSpawnDamage = DamageUtils.CalculateDamage(__instance, MonsterModifiersPlugin.Cfg_PoisonDeath_DamagePercent.Value / 100f);
                GameObject blobAoe = ZNetScene.instance.GetPrefab("blob_aoe");
                if (blobAoe != null)
                {
                    Object.Instantiate(blobAoe, __instance.transform.position, __instance.transform.rotation);

                    HitData poisonHit = new HitData
                    {
                        m_damage = { m_poison = deathSpawnDamage }
                    };

                    ApplyDamageToNearbyPlayers(__instance.transform.position, poisonHit);
                }
            }
                
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FireDeath))
            {
                float deathSpawnDamage = DamageUtils.CalculateDamage(__instance, MonsterModifiersPlugin.Cfg_FireDeath_DamagePercent.Value / 100f);
                // TO-DO: I'm overwriting the vanilla values here. Make a copy somehow.
                GameObject fireNovaAOE = ZNetScene.instance.GetPrefab("fx_fireskeleton_nova");

                ParticleSystem[] listParticleSystem = fireNovaAOE.GetComponentsInChildren<ParticleSystem>();
                foreach (var particleSystem in listParticleSystem)
                {
                    var main = particleSystem.main;
                    main.startDelay = 0f;
                }

                ZSFX zsfx = fireNovaAOE.GetComponentInChildren<ZSFX>();
                zsfx.m_delay = 0f;
                zsfx.m_minDelay = 0f;
                zsfx.m_maxDelay = 0f;

                if (fireNovaAOE != null)
                {
                    
                    Object.Instantiate(fireNovaAOE, 
                        new Vector3(
                            __instance.transform.position.x, __instance.transform.position.y + 1.25f, __instance.transform.position.z),
                            __instance.transform.rotation);

                    HitData fireHit = new HitData
                    {
                        m_damage = { m_fire = deathSpawnDamage }
                    };

                    ApplyDamageToNearbyPlayers(__instance.transform.position, fireHit);
                }
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FrostDeath))
            {
                float deathSpawnDamage = DamageUtils.CalculateDamage(__instance, MonsterModifiersPlugin.Cfg_FrostDeath_DamagePercent.Value / 100f);
                GameObject frostNovaAOE = ZNetScene.instance.GetPrefab("fx_DvergerMage_Nova_ring");
                // TimedDestruction timedDestruction = frostNovaAOE.GetComponent<TimedDestruction>();
                // timedDestruction.m_timeout = 2.5f;
                
                ParticleSystem[] listParticleSystem = frostNovaAOE.GetComponentsInChildren<ParticleSystem>();
                foreach (var particleSystem in listParticleSystem)
                {
                    var main = particleSystem.main;
                    main.startDelay = 0f;
                }

                ZSFX zsfx = frostNovaAOE.GetComponentInChildren<ZSFX>();
                zsfx.m_delay = 0f;
                zsfx.m_minDelay = 0f;
                zsfx.m_maxDelay = 0f;
                
                    
                if (frostNovaAOE != null)
                {
                    GameObject.Instantiate(frostNovaAOE, 
                        new Vector3(
                            __instance.transform.position.x,
                            __instance.transform.position.y + 1f,
                            __instance.transform.position.z
                            ),
                            __instance.transform.rotation);

                    HitData frostHit = new HitData
                    {
                        m_damage = { m_frost = deathSpawnDamage }
                    };

                    ApplyDamageToNearbyPlayers(__instance.transform.position, frostHit);
                }
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.StaggerDeath))
            {
                GameObject customMistile = PrefabManager.Instance.GetPrefab("mistleCustomPrefab");

                if (customMistile != null)
                {
                    GameObject.Instantiate(customMistile,
                        new Vector3(
                            __instance.transform.position.x,
                            __instance.transform.position.y + 1f,
                            __instance.transform.position.z
                        ),
                        __instance.transform.rotation);
                }
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.HealDeath))
            {
                GameObject healNova = PrefabManager.Instance.GetPrefab("healCustomPrefab");
                
                float healAmount = __instance.GetMaxHealth() * (MonsterModifiersPlugin.Cfg_HealDeath_HealPercent.Value / 100f);
                
                if (healNova != null)
                {
                    GameObject.Instantiate(healNova,
                        new Vector3(
                            __instance.transform.position.x,
                            __instance.transform.position.y + 1f,
                            __instance.transform.position.z
                        ),
                        __instance.transform.rotation);
                }

                if (Player.m_localPlayer.IsOwner())
                {
                    List<Character> characters = WorldUtils.GetAllCharacter(__instance.transform.position,15f);
                    foreach (var character in characters)
                    {
                        if (character == __instance || character == null)
                        {
                            continue;
                        }
                        
                        if (character.m_nview == null || !character.m_nview.IsValid() || character.IsPlayer() || character.IsDead())
                        {
                            continue;
                        }

                        character.GetSEMan().AddStatusEffect("HealDeathStatusEffect".GetStableHashCode(),false,0,healAmount);
                        // Debug.Log("Character with name: " + character.name + " was given HealDeath status effect");
                    }
            
                    List<Player> nearbyPlayers = new List<Player>();
                    Player.GetPlayersInRange(__instance.transform.position, 15f, nearbyPlayers);
                    foreach (Character character in nearbyPlayers)
                    {
                        if (character == null || character.m_nview == null || !character.m_nview.IsValid())
                        {
                            continue;
                        }

                        character.GetSEMan().AddStatusEffect("HealDeathStatusEffect".GetStableHashCode(),true,0,healAmount);
                        // Debug.Log("Player with name: " + character.name + " was given HealDeath status effect");
                    }
                }
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.TarDeath))
            {
                GameObject tarNova = ZNetScene.instance.GetPrefab("blobtar_projectile_tarball");

                if (tarNova != null)
                {
                    GameObject.Instantiate(tarNova,
                        new Vector3(
                            __instance.transform.position.x,
                            __instance.transform.position.y + 1f,
                            __instance.transform.position.z
                        ),
                        __instance.transform.rotation);
                }

                if (Player.m_localPlayer.IsOwner())
                {
                    List<Character> characters = WorldUtils.GetAllCharacter(__instance.transform.position,5f);
                    foreach (var character in characters)
                    {
                        if (character == __instance || character == null)
                        {
                            continue;
                        }

                        if (character.m_nview == null || !character.m_nview.IsValid() || character.IsPlayer())
                        {
                            continue;
                        }

                        character.GetSEMan().AddStatusEffect("Tared".GetStableHashCode());
                    }

                    List<Player> nearbyPlayers = new List<Player>();
                    Player.GetPlayersInRange(__instance.transform.position, 5f, nearbyPlayers);
                    foreach (Character character in nearbyPlayers)
                    {
                        if (character == null || character.m_nview == null || !character.m_nview.IsValid())
                        {
                            continue;
                        }

                        character.GetSEMan().AddStatusEffect("Tared".GetStableHashCode());
                    }
                }
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.SummonDeath))
            {
                if (__instance.m_nview != null && __instance.m_nview.IsOwner())
                {
                    string prefabName = Utils.GetPrefabName(__instance.gameObject);
                    GameObject summonPrefab = ZNetScene.instance.GetPrefab(prefabName);
                    if (summonPrefab == null) return;

                    int spawnCount = Mathf.Min(modiferComponent.Modifiers.Count + 1, 5);
                    float parentHealth = __instance.GetMaxHealth();
                    // 부모 속성 수 - 1 = 자식 속성 수. 0이 되면 SummonDeath 불가 → 자연 종료
                    int childModCount = Mathf.Max(modiferComponent.Modifiers.Count - 1, 0);

                    for (int i = 0; i < spawnCount; i++)
                    {
                        // 자식마다 개별 랜덤 롤
                        List<MonsterModifierTypes> childMods = childModCount > 0
                            ? ModifierUtils.RollRandomModifiers(childModCount)
                            : new List<MonsterModifierTypes>();

                        Vector3 offset = new Vector3(
                            UnityEngine.Random.Range(-2f, 2f),
                            0f,
                            UnityEngine.Random.Range(-2f, 2f)
                        );
                        GameObject spawned = Object.Instantiate(summonPrefab,
                            __instance.transform.position + offset,
                            Quaternion.identity);

                        spawned.transform.localScale *= MonsterModifiersPlugin.Cfg_SummonDeath_ScalePercent.Value / 100f;

                        Character spawnedChar = spawned.GetComponent<Character>();
                        if (spawnedChar == null) continue;

                        int spawnLevel = childMods.Count > 0 ? childMods.Count + 1 : 1;
                        spawnedChar.SetLevel(spawnLevel);

                        ZDO? zdo = spawnedChar.m_nview?.GetZDO();
                        if (zdo != null && zdo.IsOwner())
                        {
                            if (childMods.Count > 0)
                                zdo.Set("modifiers", string.Join(",", childMods));
                            float healthPct = MonsterModifiersPlugin.Cfg_SummonDeath_HealthPercent.Value / 100f;
                            spawnedChar.SetMaxHealth(parentHealth * healthPct);
                            spawnedChar.SetHealth(parentHealth * healthPct);
                        }
                    }
                }
            }
        }
    }
}