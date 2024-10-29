using System.Collections.Generic;
using System.Linq;
using MonsterModifiers.Modifiers;
using Unity.Collections;
using UnityEngine;

namespace MonsterModifiers;

public class DamageUtils
{
    public static float TryCurrentWeapon(Humanoid humanoid)
    {
        ItemDrop.ItemData currentWeapon = humanoid.GetCurrentWeapon();
        if (currentWeapon == null)
        {
            // Debug.Log("currentWeapon is null");
            return 10f;
        }

        HitData.DamageTypes damageTypes = currentWeapon.GetDamage();
        if (damageTypes.HaveDamage())
        {
            // Debug.Log("damageTypes has no damage");
            return 10f;
        }
        
        float totalDamage = damageTypes.GetTotalDamage();
        if (totalDamage == 0)
        {
            // Debug.Log("totalDamage is 0");
            return 10f;
        }

        float currentWeaponDamage = humanoid.GetCurrentWeapon().GetDamage().GetTotalDamage();

        return currentWeaponDamage;
    }

    public static float TryWeaponList(GameObject[] weaponArray)
    {
        float weaponDamage = 10f;
        
        List<GameObject> weapons = weaponArray.ToList();
        
        if (weapons.Count > 0)
        {
            foreach (GameObject weapon in weapons)
            {
                ItemDrop.ItemData itemData = weapon.GetComponent<ItemDrop>().m_itemData;
                if (itemData != null)
                {
                    float totalDamage = itemData.GetDamage().GetTotalDamage();
                    if (totalDamage > weaponDamage)
                    {
                        weaponDamage = totalDamage;
                    }
                }
                else
                {
                    // Debug.Log("Item data is null for item: " + weapon.name);
                }
            }
        }

        if (weaponDamage > 0)
        {
            // Debug.Log("Found weapon with damage: " + weaponDamage);
        }
        else
        {
            // Debug.Log("Failed to find non-zero damage weapon");
        }
        
        return weaponDamage;
    }

    public static float CalculateDamage(Character character)
    {
        Humanoid humanoid = character as Humanoid;
        if (humanoid == null)
        {
            // Debug.Log("Humanoid is null");
            return 10f;
        }

        float characterDamage = 10f;

        float currentWeaponDamage = TryCurrentWeapon(humanoid);
        if (currentWeaponDamage > 0 && currentWeaponDamage > characterDamage)
        {
            characterDamage = currentWeaponDamage;
        }
        
        float defaultWeaponDamage = TryWeaponList(humanoid.m_defaultItems);
        if (defaultWeaponDamage > 0 && defaultWeaponDamage > characterDamage)
        {
            characterDamage = defaultWeaponDamage;
        }
        
        float randomWeaponDamage = TryWeaponList(humanoid.m_randomWeapon);
        if (randomWeaponDamage > 0 && randomWeaponDamage > characterDamage)
        {
            characterDamage = randomWeaponDamage;
        }
    
        float finalDamage = characterDamage * 0.5f;
        
        // Debug.Log("Creature death spawn damage is: " + finalDamage);
        return finalDamage;
    }
}