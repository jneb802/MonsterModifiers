using System.Reflection;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Utils;
using MonsterModifiers.Custom_Components;
using UnityEngine;

namespace MonsterModifiers.Modifiers;

public class ShieldDome: MonoBehaviour
{
    public static CustomPrefab ShieldGenereatorBubbleCustomPrefab;
    public GameObject shieldGenereatorBubble;
    public Character m_character;
    public ShieldGenerator m_shieldGenerator;
    public ShieldDomeImageEffect m_shieldDomeImageEffect;
    
    public static void LoadShieldDome()
    {
        AssetBundle assetBundle = AssetUtils.LoadAssetBundleFromResources("monster_modifiers_ashlands", Assembly.GetExecutingAssembly());
        ShieldGenereatorBubbleCustomPrefab = new CustomPrefab(assetBundle, "ShieldDome_Bubble", true);
    }
    
    public void AddShieldDome(Character character)
    {
        Debug.Log("Monster with name " + character.m_name + " has modifier ShieldDome");
        character.gameObject.AddComponent<ShieldDome>();
        shieldGenereatorBubble = Instantiate(ShieldGenereatorBubbleCustomPrefab.Prefab, character.transform.position,
            character.transform.rotation);
        
        m_character = character;
        
        m_shieldGenerator = shieldGenereatorBubble.GetComponent<ShieldGenerator>();
        m_shieldGenerator.m_nview = m_shieldGenerator.GetComponentInParent<ZNetView>();
        m_shieldGenerator.m_defaultFuel = 200;
        m_shieldGenerator.m_radius = 10;
        m_shieldGenerator.m_maxShieldRadius = 10;
        m_shieldGenerator.m_minShieldRadius = 10;
        
        m_shieldDomeImageEffect = UnityEngine.Object.FindFirstObjectByType<ShieldDomeImageEffect>();
    }

    public void Update()
    {
        if (m_character != null && shieldGenereatorBubble != null)
        {
            
            shieldGenereatorBubble.transform.position = m_character.transform.position;
            shieldGenereatorBubble.transform.rotation = m_character.transform.rotation;
            
            m_shieldGenerator.m_shieldDome.transform.position = m_character.transform.position;
            m_shieldGenerator.m_shieldDome.transform.rotation = m_character.transform.rotation;
            
            m_shieldDomeImageEffect.transform.position = m_character.transform.position;
            m_shieldDomeImageEffect.transform.rotation = m_character.transform.rotation;
         
            m_shieldDomeImageEffect.SetShieldData(m_shieldGenerator,m_character.transform.position,10,m_shieldGenerator.m_lastFuel,m_shieldGenerator.m_lastHitTime);
        }
    }

    public void Destroy()
    {
        m_shieldDomeImageEffect.RemoveShield(m_shieldGenerator);
        Destroy(shieldGenereatorBubble);
    }
    
    [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
    public class ShieldDome_Character_OnDeath_Patch
    {
        public static void Prefix(Character __instance)
        {
            if (__instance.TryGetComponent(out ShieldDome shieldDome))
            {
                shieldDome.Destroy();
                Debug.Log("Shield dome destroyed when character died");
            }
        }
    }
}