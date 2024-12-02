using System;
using System.Collections.Generic;
using HarmonyLib;
using Jotunn.Managers;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class Orbs : MonoBehaviour, IMonoUpdater
{
    public int m_orbAmount;
    public List<GameObject> m_orbs = new List<GameObject>();
    public Character m_character;
    public GameObject m_orbObject;
    public HitData orbDamage = new HitData();
    public float m_orbDamageValue = new int();
    private float orbSpawnTimer = 0f;
    private float orbSpawnInterval = 10f;
    public ZNetView m_view;
    public static readonly List<IMonoUpdater> s_instances = new List<IMonoUpdater>();

    public void OnEnable()
    {
        s_instances.Add(this);
    }
    
    public void OnDisable()
    {
        s_instances.Remove(this);
    }

    public void Awake()
    {
        m_character = GetComponent<Character>();
        if (m_character == null)
        {
            return;
        }
        
        MonsterModifier monsterModifier = m_character.gameObject.GetComponent<MonsterModifier>();
        if (monsterModifier == null)
        {
            return;
        }

        if (m_character.m_nview.GetZDO().IsOwner())
        {
            Debug.Log("Player is owner, setting orb data in ZDO");
            m_view = m_character.m_nview;
            ZDO orbZDO = m_view.GetZDO();
            
            m_orbDamageValue = DamageUtils.CalculateDamage(m_character, 0.5f);
            orbZDO.Set("orbDamageValue",m_orbDamageValue);
            
            if (monsterModifier.Modifiers.Contains(MonsterModifierTypes.FireOrbs))
            {
                Debug.Log("Orb type is fire");
                string orbType = "fireBallOrbCustomPrefab";
                orbZDO.Set("orbType",orbType);
                
                m_orbAmount = 4;
                orbZDO.Set("orbAmount",m_orbAmount);
                
                m_orbObject = GetOrbPrefab("fireBallOrbCustomPrefab");
                orbDamage.m_damage = new HitData.DamageTypes
                {
                    m_fire = m_orbDamageValue
                };
            }
            
            if (monsterModifier.Modifiers.Contains(MonsterModifierTypes.FrostOrbs))
            {
                Debug.Log("Orb type is frost");
                string orbType = "iceShardOrbCustomPrefab";
                orbZDO.Set("orbType",orbType);
                
                m_orbAmount = 4;
                orbZDO.Set("orbAmount",m_orbAmount);
                
                m_orbObject = GetOrbPrefab("iceShardOrbCustomPrefab");
                orbDamage.m_damage = new HitData.DamageTypes
                {
                    m_frost = m_orbDamageValue
                };
            }
        }
        else
        {
            Debug.Log("Player is not owner, reading orb data from ZDO");
            m_view = m_character.m_nview;
            ZDO orbZDO = m_view.GetZDO();
            
            m_orbDamageValue = orbZDO.GetFloat("orbDamageValue");

            m_orbAmount = orbZDO.GetInt("orbAmount");
            
            if (monsterModifier.Modifiers.Contains(MonsterModifierTypes.FireOrbs))
            {
                Debug.Log("Orb type is fire");
                string orbType = orbZDO.GetString("orbType");
                
                m_orbObject = GetOrbPrefab(orbType);
                orbDamage.m_damage = new HitData.DamageTypes
                {
                    m_fire = m_orbDamageValue
                };
            }
            
            if (monsterModifier.Modifiers.Contains(MonsterModifierTypes.FrostOrbs))
            {
                Debug.Log("Orb type is frost");
                string orbType = orbZDO.GetString("orbType");
                
                m_orbObject = GetOrbPrefab(orbType);
                orbDamage.m_damage = new HitData.DamageTypes
                {
                    m_frost = m_orbDamageValue
                };
            }
        }
    }
    
    public void Start()
    {
        if (m_character.m_nview.GetZDO().IsOwner())
        {
            float radius = 5.0f;
            for (int i = 0; i < m_orbAmount; i++)
            {
                
                float angle = i * Mathf.PI * 2 / 3;
                Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 1, Mathf.Sin(angle) * radius);
                Vector3 orbPosition = m_character.transform.position + offset;
                Vector3 forward = (orbPosition - m_character.transform.position).normalized;
                Vector3 velocity = forward * 1.0f;
            
                GameObject orb = ZNetScene.Instantiate(m_orbObject, orbPosition, Quaternion.LookRotation(forward));
                ZNetView orbZnetView = orb.GetComponent<ZNetView>();
                ZSyncTransform orbZSyncTransform = orb.GetComponent<ZSyncTransform>();
                // Destroy(orbZSyncTransform);
                
                Projectile orbProjectile = orb.GetComponent<Projectile>();
                orbProjectile.m_stayTTL = 20f;
                orbProjectile.Setup(m_character,velocity,5f,orbDamage,null,null);
                orbProjectile.m_startPoint = orbPosition;
                
                // orbZnetView.GetZDO().SetPosition(orbPosition);
            
                // Debug.Log("Created orb object with prefab hash: " + orbZnetView.GetZDO().m_prefab);
                // Debug.Log("Created orb object with zdo uid: " + orbZnetView.GetZDO().m_uid);
            
                m_orbs.Add(orb);
            }
        }
    }

    public GameObject GetOrbPrefab(string orbPrefabName)
    {
        switch (orbPrefabName)
        {
            case "fireBallOrbCustomPrefab":
                return PrefabManager.Instance.GetPrefab("fireBallOrbCustomPrefab");
            case "iceShardOrbCustomPrefab":
                return PrefabManager.Instance.GetPrefab("iceShardOrbCustomPrefab");
            default:
                return null;
        }
    }
    
    public void CustomFixedUpdate(float deltaTime)
    {
        for (int i = m_orbs.Count - 1; i >= 0; i--)
        {
            if (m_orbs[i] == null)
            {
                Debug.Log("Removing orb at index: " + i);
                m_orbs.RemoveAt(i);
            }
        }
        
        if (m_character != null)
        {
            float dt = Time.deltaTime;
            float circleRadius = 5.0f;

            if (m_orbs.Count > 0)
            {
                for (int i = 0; i < m_orbs.Count; i++)
                {
                    CircleAroundCharacter(m_orbs[i], dt, circleRadius, i);
                }
            }

            if (m_character.m_nview.IsOwner())
            {
                // Check if m_orbs is less than m_orbCount and start the timer
                if (m_orbs.Count < m_orbAmount)
                {
                    orbSpawnTimer += dt;
            
                    // Check if 10 seconds have passed
                    if (orbSpawnTimer >= orbSpawnInterval)
                    {
                        orbSpawnTimer = 0f; // Reset timer after spawning
            
                        // Instantiate a new orb
                        float angle = m_orbs.Count * Mathf.PI * 2 / m_orbAmount;
                        Vector3 offset = new Vector3(Mathf.Cos(angle) * circleRadius, 1, Mathf.Sin(angle) * circleRadius);
                        Vector3 orbPosition = m_character.transform.position + offset;
                        Vector3 forward = (orbPosition - m_character.transform.position).normalized;
                        Vector3 velocity = forward * 1.0f;
            
                        GameObject orb = ZNetScene.Instantiate(m_orbObject, orbPosition, Quaternion.LookRotation(forward));
                        Projectile orbProjectile = orb.GetComponent<Projectile>();
                        orbProjectile.m_stayTTL = 20f;
                        orbProjectile.Setup(m_character,velocity,5f,orbDamage,null,null);
                        orbProjectile.m_startPoint = orbPosition;
                    
                        m_orbs.Add(orb);
                    }
                }
            }
        }
        else
        {
            foreach (var orb in m_orbs)
            {
                if (orb != null)
                {
                    ZNetScene.instance.Destroy(orb);
                }
            }
            
            m_orbs.Clear();
        }
    }
    
    public void CircleAroundCharacter(GameObject orb, float dt, float radius, int index)
    {
        float rotationSpeed = 60;
        float angle = (Time.time * Mathf.Deg2Rad * rotationSpeed) + (index * Mathf.PI * 2 / m_orbAmount);
        Vector3 characterPosition = m_character.transform.position;
        Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 1, Mathf.Sin(angle) * radius);
        Vector3 targetPosition = characterPosition + offset;
        Vector3 direction = (targetPosition - orb.transform.position).normalized;
        Projectile orbProjectile = orb.GetComponent<Projectile>();
        orbProjectile.m_vel = direction * 1.0f;
        orb.transform.position = targetPosition;
    }
    
    public static void AddOrbs(Character character)
    {
        character.gameObject.AddComponent<Orbs>();
    }
    
    [HarmonyPatch(typeof(MonoUpdaters), nameof(MonoUpdaters.FixedUpdate))]
    private static class MonoUpdaters_FixedUpdate_Patch
    {
        private static void Postfix(MonoUpdaters __instance)
        {
            float fixedTimeDelta = Time.fixedDeltaTime;
            __instance.m_update.CustomFixedUpdate(s_instances, "MonoUpdaters.FixedUpdate.Orbs", fixedTimeDelta);
        }
    }

    public void CustomUpdate(float deltaTime, float time)
    {
    }

    public void CustomLateUpdate(float deltaTime)
    {
    }
}