using System;
using System.Collections.Generic;
using System.Linq;
using MonsterModifiers.Modifiers;
using UnityEngine;
using MonsterModifiers.Visuals;

namespace MonsterModifiers.Custom_Components;

public class MonsterModifier : MonoBehaviour
{
   public List<MonsterModifierTypes> Modifiers = new List<MonsterModifierTypes>();

   public Character character = null!;

   public int level;

   // Set to true when attached to a boss character / 보스 캐릭터에 붙어 있을 때 true
   public bool IsBossCharacter = false;
   // Stars beyond the HUD icon slots; displayed as plain stars / HUD 아이콘 슬롯 초과 별 수
   public int OverflowStars = 0;

   private void Start()
   {
      character = GetComponent<Character>();
      level = character.GetLevel();

      // Skip Epic Loot bounty targets / Epic Loot 현상금 대상은 건너뜀
      if (character.m_nview != null && character.m_nview.GetZDO().GetString("BountyID") != string.Empty)
      {
         Destroy(this);
         return;
      }

      if (character.IsBoss())
      {
         if (MonsterModifiersPlugin.Configurations_Boss_Modifiers.Value == MonsterModifiersPlugin.Toggle.Off)
            return;

         IsBossCharacter = true;
         // Stars determine modifier count, capped at boss_Modifiers_Max; remainder shown as plain stars
         // 별 수 = 속성 수, boss_Modifiers_Max로 상한; 남은 별은 HUD에 별 아이콘으로 표시
         int starCount = Mathf.Max(0, level - 1);
         int actualCount = Mathf.Min(starCount, MonsterModifiersPlugin.Configurations_Boss_MaxModifiers.Value);
         OverflowStars = starCount - actualCount;

         string modifiersString = character.m_nview.GetZDO().GetString("modifiers", string.Empty);
         if (string.IsNullOrEmpty(modifiersString))
         {
            foreach (var modifier in ModifierUtils.RollRandomModifiers(actualCount, ModifierUtils.BossExcludedModifiers))
               Modifiers.Add(modifier);

            if (character.m_nview.GetZDO().IsOwner())
               character.m_nview.GetZDO().Set("modifiers", string.Join(",", Modifiers));
         }
         else
         {
            Modifiers = new List<MonsterModifierTypes>(Array.ConvertAll(modifiersString.Split(','),
               str => (MonsterModifierTypes)Enum.Parse(typeof(MonsterModifierTypes), str)));

            // Recalculate overflow in case MaxModifiers config changed
            // MaxModifiers 컨피그가 변경된 경우를 대비해 overflow 재계산
            int reloadedActual = Mathf.Min(starCount, MonsterModifiersPlugin.Configurations_Boss_MaxModifiers.Value);
            OverflowStars = starCount - reloadedActual;
         }

         ApplyStartModifiers();
         return;
      }

      if (level > 1)
      {
         string modifiersString = character.m_nview?.GetZDO()?.GetString("modifiers", string.Empty) ?? string.Empty;
         if (string.IsNullOrEmpty(modifiersString))
         {
            // level - 1 so a 2-star monster gets 1 modifier, 3-star gets 2, etc.
            // level - 1: ★2 몬스터 = 속성 1개, ★3 = 2개
            int numModifiers = Mathf.Min(level - 1, MonsterModifiersPlugin.Configurations_MaxModifiers.Value);

            foreach (var modifier in ModifierUtils.RollRandomModifiers(numModifiers))
               Modifiers.Add(modifier);

            if (character.m_nview.GetZDO().IsOwner())
            {
               string serializedModifiers = string.Join(",", Modifiers);
               character.m_nview.GetZDO().Set("modifiers", serializedModifiers);
            }
         }
         else
         {
            Modifiers = new List<MonsterModifierTypes>(Array.ConvertAll(modifiersString.Split(','),
               str => (MonsterModifierTypes)Enum.Parse(typeof(MonsterModifierTypes), str)));
         }

         ApplyStartModifiers();
      }
   }

   public void ChangeModifiers(List<MonsterModifierTypes> modifierTypesList, int numModifiers)
   {
      if (level > 1)
      {
         foreach (var modifier in modifierTypesList)
         {
            Modifiers.Add(modifier);
            Debug.Log("Monster with name " + character.name + " has has changed modifiers. New modifier: " + modifier);
         }

         if (character.m_nview.GetZDO().IsOwner())
         {
            string serializedModifiers = string.Join(",", Modifiers);
            character.m_nview.GetZDO().Set("modifiers", serializedModifiers);
         }
      }
   }

   public void ApplyStartModifiers()
   {
      if (Modifiers.Contains(MonsterModifierTypes.PersonalShield))
         PersonalShield.AddPersonalShield(character);

      if (Modifiers.Contains(MonsterModifierTypes.ShieldDome))
      {
         var shieldDome = character.gameObject.AddComponent<ShieldDome>();
         shieldDome.AddShieldDome(character);
      }

      if (Modifiers.Contains(MonsterModifierTypes.StaggerImmune))
         StaggerImmune.AddStaggerImmune(character);

      if (Modifiers.Contains(MonsterModifierTypes.FastMovement))
         FastMovement.AddFastMovement(character);

      if (Modifiers.Contains(MonsterModifierTypes.DistantDetection))
         DistantDetection.AddDistantDetection(character);

      if (Modifiers.Contains(MonsterModifierTypes.Quiet))
         Quiet.AddQuiet(character);

      // Synergy: PierceImmunity + SlashImmunity + BluntImmunity + FastMovement → Armor Charge
      // 시너지: 물리 3종 저항 + FastMovement → 철갑 돌격
      if (Modifiers.Contains(MonsterModifierTypes.PierceImmunity) &&
          Modifiers.Contains(MonsterModifierTypes.SlashImmunity) &&
          Modifiers.Contains(MonsterModifierTypes.BluntImmunity) &&
          Modifiers.Contains(MonsterModifierTypes.FastMovement))
      {
         Modifiers.Add(MonsterModifierTypes.ArmorCharge);
         ArmorCharge.Apply(character);
      }

      // Synergy: FireInfused + FastAttackSpeed → fire trail particles
      // 시너지: FireInfused + FastAttackSpeed → 화염 흔적 파티클
      if (Modifiers.Contains(MonsterModifierTypes.FireInfused) &&
          Modifiers.Contains(MonsterModifierTypes.FastAttackSpeed))
      {
         Modifiers.Add(MonsterModifierTypes.FireTrail);
         var ft = character.gameObject.AddComponent<FireTrail>();
         ft.Init(character);
      }
   }
}
