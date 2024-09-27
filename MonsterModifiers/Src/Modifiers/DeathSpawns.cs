using System.Collections.Generic;
using HarmonyLib;
using Jotunn;
using Jotunn.Managers;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class DeathSpawns
{
    [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
    public class PoisonDeath_Character_OnDeath_Patch
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

                    List<Player> nearbyPlayers = new List<Player>();
                    Player.GetPlayersInRange(__instance.transform.position, 5f, nearbyPlayers);
                    foreach (var player in nearbyPlayers)
                    {
                        player.Damage(poisonHit);
                    }
                }
            }

            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FireDeath))
            {
                GameObject fireNovaAOE = ZNetScene.instance.GetPrefab("fx_fireskeleton_nova");
                TimedDestruction timedDestruction = fireNovaAOE.GetComponent<TimedDestruction>(); 
                timedDestruction.m_timeout = 1f;

                if (fireNovaAOE != null)
                {
                    Object.Instantiate(fireNovaAOE, 
                        new Vector3(
                            __instance.transform.position.x,
                            __instance.transform.position.y + 2f,
                            __instance.transform.position.z
                        ),
                        __instance.transform.rotation);

                    HitData fireHit = new HitData
                    {
                        m_damage = { m_fire = 20f }
                    };

                    List<Player> nearbyPlayers = new List<Player>();
                    Player.GetPlayersInRange(__instance.transform.position, 5f, nearbyPlayers);
                    foreach (var player in nearbyPlayers)
                    {
                        player.Damage(fireHit);
                    }
                }
            }
            
            if (modiferComponent.Modifiers.Contains(MonsterModifierTypes.FrostDeath))
            {
                GameObject frostNovaAOE = ZNetScene.instance.GetPrefab("fx_fenring_icenova");
                TimedDestruction timedDestruction = frostNovaAOE.GetComponent<TimedDestruction>();
                timedDestruction.m_timeout = 0f;
                GameObject sfx2 = frostNovaAOE.FindDeepChild("sfx").gameObject;
                sfx2.SetActive(false);
                GameObject sfx3 = frostNovaAOE.FindDeepChild("sfx").gameObject;
                sfx3.SetActive(false);
                    
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

                    List<Player> nearbyPlayers = new List<Player>();
                    Player.GetPlayersInRange(__instance.transform.position, 5f, nearbyPlayers);
                    foreach (var player in nearbyPlayers)
                    {
                        player.Damage(frostHit);
                    }
                }
            }
        }
    }
}