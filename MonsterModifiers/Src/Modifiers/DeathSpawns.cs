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
                float deathSpawnDamage = DamageUtils.CalculateDamage(__instance, 0.5f);
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
                float deathSpawnDamage = DamageUtils.CalculateDamage(__instance, 0.5f);
                // TO-DO: I'm overwriting the vanilla values here. Make a copy somehow.
                GameObject fireNovaAOE = ZNetScene.instance.GetPrefab("fx_fireskeleton_nova");

                ParticleSystem[] listParticleSystem = fireNovaAOE.GetComponentsInChildren<ParticleSystem>();
                foreach (var particleSystem in listParticleSystem)
                {
                    particleSystem.startDelay = 0f;
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
                float deathSpawnDamage = DamageUtils.CalculateDamage(__instance, 0.5f);
                GameObject frostNovaAOE = ZNetScene.instance.GetPrefab("fx_DvergerMage_Nova_ring");
                // TimedDestruction timedDestruction = frostNovaAOE.GetComponent<TimedDestruction>();
                // timedDestruction.m_timeout = 2.5f;
                
                ParticleSystem[] listParticleSystem = frostNovaAOE.GetComponentsInChildren<ParticleSystem>();
                foreach (var particleSystem in listParticleSystem)
                {
                    particleSystem.startDelay = 0f;
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
                
                float healAmount = (__instance.GetMaxHealth() * 0.75f);
                
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
                        
                        if (character.m_nview == null || character.IsPlayer())
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
                        if (character == null || character.m_nview == null)
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
                        
                        if (character.m_nview == null || character.IsPlayer())
                        {
                            continue;
                        }
                        
                        character.GetSEMan().AddStatusEffect("Tared".GetStableHashCode());
                    }
            
                    List<Player> nearbyPlayers = new List<Player>();
                    Player.GetPlayersInRange(__instance.transform.position, 5f, nearbyPlayers);
                    foreach (Character character in nearbyPlayers)
                    {
                        if (character == null || character.m_nview == null)
                        {
                            continue;
                        }
                        
                        character.GetSEMan().AddStatusEffect("Tared".GetStableHashCode());
                    }
                }
            }
        }
    }
}