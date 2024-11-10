using System;
using System.Collections.Generic;
using HarmonyLib;
using Jotunn.Managers;
using UnityEngine;

namespace MonsterModifiers.Custom_Components;

public class Orbs : MonoBehaviour
{
    public int m_orbAmount;
    public List<GameObject> m_orbs = new List<GameObject>();
    public Character m_character;
    public GameObject m_orbObject;
    public HitData orbDamage = new HitData();
    public float m_orbDamageValue = new int();
    private float orbSpawnTimer = 0f;
    private float orbSpawnInterval = 10f;
    
    public void Awake()
    {
        m_character = GetComponent<Character>();
        if (m_character == null)
        {
            return;
        }
        
        m_orbDamageValue = DamageUtils.CalculateDamage(m_character, 0.5f);
        
        MonsterModifier monsterModifier = m_character.gameObject.GetComponent<MonsterModifier>();
        if (monsterModifier == null)
        {
            return;
        }
        
        if (monsterModifier.Modifiers.Contains(MonsterModifierTypes.FireOrbs))
        {
            Debug.Log("Orb type is fire");
            m_orbObject = PrefabManager.Instance.GetPrefab("fireBallOrbCustomPrefab");
            orbDamage.m_damage = new HitData.DamageTypes
            {
                m_fire = m_orbDamageValue
            };
        }
        
        m_orbAmount = 3;
            
    }
    
    public void Start()
    {
        if (!Player.m_localPlayer.IsOwner())
        {
            return;
        }
        
        float radius = 5.0f;
        for (int i = 0; i < m_orbAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / 3;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 1, Mathf.Sin(angle) * radius);
            Vector3 orbPosition = m_character.transform.position + offset;
            Vector3 forward = (orbPosition - m_character.transform.position).normalized;
            Vector3 velocity = forward * 3.0f;
            
            GameObject orb = ZNetScene.Instantiate(m_orbObject, orbPosition, Quaternion.LookRotation(forward));
            Projectile orbProjectile = orb.GetComponent<Projectile>();
            orbProjectile.Setup(m_character,velocity,5f,orbDamage,null,null);
            orbProjectile.m_startPoint = orbPosition;
            
            m_orbs.Add(orb);
        }
    }

    public void Update()
    {
        if (!Player.m_localPlayer.IsOwner())
        {
            return;
        }
        
        for (int i = m_orbs.Count - 1; i >= 0; i--)
        {
            if (m_orbs[i] == null)
            {
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
                    Vector3 velocity = forward * 3.0f;

                    GameObject orb = ZNetScene.Instantiate(m_orbObject, orbPosition, Quaternion.LookRotation(forward));
                    Projectile orbProjectile = orb.GetComponent<Projectile>();
                    orbProjectile.Setup(m_character,velocity,5f,orbDamage,null,null);
                    orbProjectile.m_startPoint = orbPosition;
            
                    m_orbs.Add(orb);
                }
            }
        }
        else
        {
            foreach (var orb in m_orbs)
            {
                if (orb != null)
                {
                   Destroy(orb);
                }
            }
            m_orbs.Clear();

            Destroy(this);
        }
    }
    
    public void CircleAroundCharacter(GameObject orb, float dt, float radius, int index)
    {
        float angle = (Time.time * Mathf.Deg2Rad * 30) + (index * Mathf.PI * 2 / m_orbAmount);
        
        Vector3 characterPosition = m_character.transform.position;
        
        Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 1, Mathf.Sin(angle) * radius);
        Vector3 targetPosition = characterPosition + offset;

        Vector3 direction = (targetPosition - orb.transform.position).normalized;
        Projectile orbProjectile = orb.GetComponent<Projectile>();
        orbProjectile.m_vel = direction * 6.0f;
        orbProjectile.transform.position = targetPosition;
    }
    
    public static void AddOrbs(Character character)
    {
        character.gameObject.AddComponent<Orbs>();
    }

    // public class OrbData
    // {
    //     public GameObject orbObject;
    //     public Projectile orbProjectile;
    //     public ZNetView orbZNetView;
    //     public ZDO orbZDO;
    //     public ZSyncTransform orbZSyncTransform;
    // }
}