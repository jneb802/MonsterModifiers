using System;
using System.Reflection;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class ShieldDome: MonoBehaviour
{
    public static CustomPrefab ShieldGenereatorBubbleCustomPrefab = null!;
    public static EffectList.EffectData shieldDomeBubbleSFX = null!;
    public GameObject shieldGenereatorBubble = null!;
    public Character m_character = null!;
    public ShieldGenerator m_shieldGenerator = null!;
    public ShieldDomeImageEffect m_shieldDomeImageEffect = null!;
    public ZNetView m_nview = null!;
    
    public static void LoadShieldDome()
    {
        ShieldGenereatorBubbleCustomPrefab = new CustomPrefab(ModifierAssetUtils.ashlandsAssetBundle, "ShieldDome_Bubble", true);
        PrefabManager.Instance.AddPrefab(ShieldGenereatorBubbleCustomPrefab);
    }
    
    public void AddShieldDome(Character character)
    {
        // Debug.Log("Monster with name " + character.m_name + " has modifier ShieldDome");
        character.gameObject.AddComponent<ShieldDome>();
        shieldGenereatorBubble = ZNetScene.Instantiate(ShieldGenereatorBubbleCustomPrefab.Prefab, character.transform.position,
            character.transform.rotation);
        
        m_character = character;
        
        m_shieldGenerator = shieldGenereatorBubble.GetComponent<ShieldGenerator>();
        m_nview = shieldGenereatorBubble.GetComponent<ZNetView>();

        m_shieldGenerator.m_nview = m_nview;
        m_shieldGenerator.m_defaultFuel = 999;
        m_shieldGenerator.m_radius = 10;
        m_shieldGenerator.m_maxShieldRadius = 10;
        m_shieldGenerator.m_minShieldRadius = 10;
        
        m_shieldDomeImageEffect = UnityEngine.Object.FindFirstObjectByType<ShieldDomeImageEffect>();
        
        // Register the custom RPC for destroying the ShieldDome
        if (m_nview != null)
        {
            m_nview.Register("RPC_DestroyShieldDome", new Action<long>(RPC_DestroyShieldDome));
        }
    }

    public void Update()
    {
        if (m_character != null && shieldGenereatorBubble != null)
        {
                m_shieldGenerator.m_shieldDome.transform.position = m_character.transform.position;
                m_shieldGenerator.m_shieldDome.transform.rotation = m_character.transform.rotation;
         
                m_shieldDomeImageEffect.SetShieldData(m_shieldGenerator,m_character.transform.position,10,m_shieldGenerator.m_lastFuel,m_shieldGenerator.m_lastHitTime);
        }
    }
    
    public void OnDestroy()
    {
        if (m_nview != null && m_nview.IsValid() && m_nview.IsOwner())
        {
            m_nview.InvokeRPC(ZNetView.Everybody, "RPC_DestroyShieldDome");
        }
    }
    
    private void RPC_DestroyShieldDome(long sender)
    {
        DestroyShieldDome();
    }
    
    private void DestroyShieldDome()
    {
        if (m_shieldGenerator != null)
        {
            m_shieldDomeImageEffect.RemoveShield(m_shieldGenerator);
        }

        if (shieldGenereatorBubble != null)
        {
            ZNetScene.instance.Destroy(shieldGenereatorBubble);
        }

        if (m_shieldGenerator.m_shieldDome != null)
        {
            ZNetScene.instance.Destroy(m_shieldGenerator.m_shieldDome);
        }
    }
}