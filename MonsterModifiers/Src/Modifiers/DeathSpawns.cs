using System.Collections.Generic;
using HarmonyLib;
using Jotunn;
using Jotunn.Managers;
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
                GameObject blobAoe = ZNetScene.instance.GetPrefab("blob_aoe");
                if (blobAoe != null)
                {
                    Object.Instantiate(blobAoe, __instance.transform.position, __instance.transform.rotation);

                    HitData poisonHit = new HitData
                    {
                        m_damage = { m_poison = 20f }
                    };

                    ApplyDamageToNearbyPlayers(__instance.transform.position, poisonHit);
                }
            }
                
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FireDeath))
            {
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
                        m_damage = { m_fire = 20f }
                    };

                    ApplyDamageToNearbyPlayers(__instance.transform.position, fireHit);
                }
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FrostDeath))
            {
                GameObject frostNovaAOE = ZNetScene.instance.GetPrefab("fx_fenring_icenova");
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
                        m_damage = { m_frost = 30f }
                    };

                    ApplyDamageToNearbyPlayers(__instance.transform.position, frostHit);
                }
            }
        }
    }
}