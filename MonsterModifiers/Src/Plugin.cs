using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Extensions;
using Jotunn.Managers;
using Jotunn.Utils;
using LocalizationManager;
using MonsterModifiers.Modifiers;
using MonsterModifiers.Patches;
using UnityEngine;
using Paths = BepInEx.Paths;

namespace MonsterModifiers
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class MonsterModifiersPlugin : BaseUnityPlugin
    {
        internal const string ModName = "MonsterModifiers";
        internal const string ModVersion = "1.3.1";
        internal const string Author = "KorCaptain";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource MonsterModifiersLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);

        // Location Manager variables
        public Texture2D tex = null!;

        // Use only if you need them
        //private Sprite mySprite = null!;
        //private SpriteRenderer sr = null!;

        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            // Uncomment the line below to use the LocalizationManager for localizing your mod.
            //Localizer.Load(); // Use this to initialize the LocalizationManager (for more information on LocalizationManager, see the LocalizationManager documentation https://github.com/blaxxun-boop/LocalizationManager#example-project).
            bool saveOnSet = Config.SaveOnConfigSet;
            Config.SaveOnConfigSet =
                false; // This and the variable above are used to prevent the config from saving on startup for each config entry. This is speeds up the startup process.

            ShaderLogFilter.Apply();

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();

            if (saveOnSet)
            {
                Config.SaveOnConfigSet = saveOnSet;
                Config.Save();
            }

            YamlUtils.ParseDefaultYamls();
            TranslationUtils.AddLocalizations();
            ModifierAssetUtils.Setup();
            ModifierAssetUtils.LoadAllIcons();

            Configurations_MaxModifiers = Config.Bind("Balance", "Max Modifiers", 5,
                new ConfigDescription(L("몬스터 1마리당 최대 속성 수 (0=속성 없음, 5=최대 5개)", "Max modifier count per monster (0=none, 5=max 5)"), new AcceptableValueRange<int>(0, 5)));

            // === Modifier_Offense ===
            Cfg_StaminaSiphon_DrainPercent = Config.Bind("Modifier_Offense", "StaminaSiphon Drain %", 100,
                new ConfigDescription(L("데미지의 N%만큼 스태미나 흡수 (0=없음, 100=데미지와 동일)", "Drain N% of damage dealt as stamina (0=none, 100=equal to damage)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_EitrSiphon_DrainPercent = Config.Bind("Modifier_Offense", "EitrSiphon Drain %", 100,
                new ConfigDescription(L("데미지의 N%만큼 에이트르 흡수 (0=없음, 100=데미지와 동일)", "Drain N% of damage dealt as Eitr (0=none, 100=equal to damage)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_ShieldBreaker_DurabilityReduction = Config.Bind("Modifier_Offense", "ShieldBreaker Durability Reduction %", 50,
                new ConfigDescription(L("막을 때 방패 내구도를 N% 감소 (50=절반으로 감소)", "Reduce shield durability by N% on block (50=half)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_IgnoreArmor_ArmorReduction = Config.Bind("Modifier_Offense", "IgnoreArmor Armor Reduction %", 50,
                new ConfigDescription(L("공격 시 플레이어 방어력을 N% 무시 (50=절반 무시)", "Ignore N% of player armor on hit (50=half)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_FoodDrain_FoodReduction = Config.Bind("Modifier_Offense", "FoodDrain Food Time Reduction %", 50,
                new ConfigDescription(L("랜덤 음식 하나의 지속시간을 N% 감소 (50=절반으로)", "Reduce a random food item's remaining time by N% (50=half)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_FireInfused_DamagePercent = Config.Bind("Modifier_Offense", "FireInfused Bonus Damage %", 50,
                new ConfigDescription(L("총 데미지의 N%를 화염 피해로 추가 (50=절반 추가)", "Add N% of total damage as bonus fire damage (50=half)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_FrostInfused_DamagePercent = Config.Bind("Modifier_Offense", "FrostInfused Bonus Damage %", 50,
                new ConfigDescription(L("총 데미지의 N%를 냉기 피해로 추가", "Add N% of total damage as bonus frost damage"), new AcceptableValueRange<int>(0, 100)));
            Cfg_PoisonInfused_DamagePercent = Config.Bind("Modifier_Offense", "PoisonInfused Bonus Damage %", 50,
                new ConfigDescription(L("총 데미지의 N%를 독 피해로 추가", "Add N% of total damage as bonus poison damage"), new AcceptableValueRange<int>(0, 100)));
            Cfg_LightningInfused_DamagePercent = Config.Bind("Modifier_Offense", "LightningInfused Bonus Damage %", 50,
                new ConfigDescription(L("총 데미지의 N%를 번개 피해로 추가", "Add N% of total damage as bonus lightning damage"), new AcceptableValueRange<int>(0, 100)));
            Cfg_FastAttackSpeed_SpeedPercent = Config.Bind("Modifier_Offense", "FastAttackSpeed Speed %", 50,
                new ConfigDescription(L("공격 애니메이션 속도 N% 증가 (50=1.5배)", "Increase attack animation speed by N% (50=1.5x)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_Knockback_StaggerForce = Config.Bind("Modifier_Offense", "Knockback Stagger Force", 500,
                new ConfigDescription(L("넉백 시 경직 강도 플랫값 (기본 500)", "Flat stagger force applied on knockback hit (default 500)"), new AcceptableValueRange<int>(0, 2000)));
            Cfg_Knockback_PushForce = Config.Bind("Modifier_Offense", "Knockback Push Force", 45,
                new ConfigDescription(L("넉백 시 밀침 거리 플랫값 (기본 45)", "Flat push force magnitude for knockback (default 45)"), new AcceptableValueRange<int>(0, 200)));
            Cfg_Forceful_PushMultiplier = Config.Bind("Modifier_Offense", "Forceful Push Multiplier", 5.0f,
                new ConfigDescription(L("기본 밀침력에 N배 적용 (기본 5.0)", "Multiply base push force by N (default 5.0)"), new AcceptableValueRange<float>(0f, 20f)));
            Cfg_Wet_ApplyChance = Config.Bind("Modifier_Offense", "Wet Apply Chance %", 100,
                new ConfigDescription(L("공격 시 물에 젖음 상태 적용 확률 (0=없음, 100=항상)", "Chance to apply Wet status on hit (0=never, 100=always)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_RemoveStatusEffect_Chance = Config.Bind("Modifier_Offense", "RemoveStatusEffect Chance %", 100,
                new ConfigDescription(L("공격 시 랜덤 상태효과 제거 발동 확률 (0=없음, 100=항상)", "Chance to remove a random status effect on hit (0=never, 100=always)"), new AcceptableValueRange<int>(0, 100)));

            // === Modifier_Defense ===
            Cfg_PierceImmunity_DamageReduction = Config.Bind("Modifier_Defense", "PierceImmunity Damage Reduction %", 70,
                new ConfigDescription(L("관통 피해 N% 감소 (70=30%만 받음)", "Reduce pierce damage by N% (70=take only 30%)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_SlashImmunity_DamageReduction = Config.Bind("Modifier_Defense", "SlashImmunity Damage Reduction %", 70,
                new ConfigDescription(L("베기 피해 N% 감소", "Reduce slash damage by N%"), new AcceptableValueRange<int>(0, 100)));
            Cfg_BluntImmunity_DamageReduction = Config.Bind("Modifier_Defense", "BluntImmunity Damage Reduction %", 70,
                new ConfigDescription(L("둔기 피해 N% 감소", "Reduce blunt damage by N%"), new AcceptableValueRange<int>(0, 100)));
            Cfg_ElementalImmunity_DamageReduction = Config.Bind("Modifier_Defense", "ElementalImmunity Damage Reduction %", 70,
                new ConfigDescription(L("화염/냉기/번개/독/정신 피해 N% 감소", "Reduce fire/frost/lightning/poison/spirit damage by N%"), new AcceptableValueRange<int>(0, 100)));
            Cfg_FastMovement_SpeedPercent = Config.Bind("Modifier_Defense", "FastMovement Speed %", 50,
                new ConfigDescription(L("이동/달리기/걷기 속도 N% 증가 (50=1.5배)", "Increase move/run/walk speed by N% (50=1.5x)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_DistantDetection_RangeMultiplier = Config.Bind("Modifier_Defense", "DistantDetection Range Multiplier", 2.0f,
                new ConfigDescription(L("청각/시각 감지 범위 N배 (기본 2.0)", "Multiply hear/view detection range by N (default 2.0)"), new AcceptableValueRange<float>(1f, 5f)));

            // === Modifier_Lifesteal ===
            Cfg_Vampiric_HealPercent = Config.Bind("Modifier_Lifesteal", "Vampiric Heal %", 50,
                new ConfigDescription(L("딜의 N%만큼 공격자 회복 (50=딜의 절반)", "Heal attacker for N% of damage dealt (50=half of damage)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_Absorption_HealPercent = Config.Bind("Modifier_Lifesteal", "Absorption Heal %", 70,
                new ConfigDescription(L("저항으로 막은 피해의 N%만큼 회복 (면역 속성 보유 시)", "Heal for N% of damage absorbed by immunity modifiers"), new AcceptableValueRange<int>(0, 100)));
            Cfg_SoulEater_GrowthPerStack = Config.Bind("Modifier_Lifesteal", "SoulEater Growth Per Stack %", 10,
                new ConfigDescription(L("주변 몬스터 사망 시 스택당 N% 성장 (크기/체력/데미지, 최대 3스택)", "Grow by N% per stack when nearby monsters die (size/health/damage, max 3 stacks)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_BloodLoss_BurstDamagePercent = Config.Bind("Modifier_Lifesteal", "BloodLoss Burst Damage %", 30,
                new ConfigDescription(L("혈손 폭발 시 최대체력의 N% 베기 피해 (기본 30)", "Deal N% of max health as slash damage when BloodLoss bursts (default 30)"), new AcceptableValueRange<int>(0, 100)));

            // === Modifier_Death ===
            Cfg_PoisonDeath_DamagePercent = Config.Bind("Modifier_Death", "PoisonDeath Damage %", 50,
                new ConfigDescription(L("사망 시 독 폭발 피해 (몬스터 공격력의 N%)", "Poison AOE damage on death (N% of monster attack power)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_FireDeath_DamagePercent = Config.Bind("Modifier_Death", "FireDeath Damage %", 50,
                new ConfigDescription(L("사망 시 화염 폭발 피해 (몬스터 공격력의 N%)", "Fire nova damage on death (N% of monster attack power)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_FrostDeath_DamagePercent = Config.Bind("Modifier_Death", "FrostDeath Damage %", 50,
                new ConfigDescription(L("사망 시 냉기 폭발 피해 (몬스터 공격력의 N%)", "Frost nova damage on death (N% of monster attack power)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_HealDeath_HealPercent = Config.Bind("Modifier_Death", "HealDeath Heal %", 75,
                new ConfigDescription(L("사망 시 주변 몬스터에게 최대체력의 N% 치유 (기본 75)", "Heal nearby monsters for N% of max health on death (default 75)"), new AcceptableValueRange<int>(0, 100)));
            Cfg_SummonDeath_ScalePercent = Config.Bind("Modifier_Death", "SummonDeath Scale %", 70,
                new ConfigDescription(L("소환체 크기 (원본의 N%, 기본 70)", "Spawned minion scale as N% of parent (default 70)"), new AcceptableValueRange<int>(1, 100)));
            Cfg_SummonDeath_HealthPercent = Config.Bind("Modifier_Death", "SummonDeath Health %", 70,
                new ConfigDescription(L("소환체 체력 (원본의 N%, 기본 70)", "Spawned minion health as N% of parent (default 70)"), new AcceptableValueRange<int>(1, 100)));

            // ShieldDome.LoadShieldDome();
            
            CompatibilityUtils.RunCompatibiltyChecks();
            
            StatusEffectUtils.CreateCustomStatusEffects();
            
            PrefabManager.OnVanillaPrefabsAvailable += PrefabUtils.CreateCustomPrefabs;
        }
        
        public static ConfigEntry<int> Configurations_MaxModifiers = null!;

        // Modifier_Offense
        public static ConfigEntry<int> Cfg_StaminaSiphon_DrainPercent = null!;
        public static ConfigEntry<int> Cfg_EitrSiphon_DrainPercent = null!;
        public static ConfigEntry<int> Cfg_ShieldBreaker_DurabilityReduction = null!;
        public static ConfigEntry<int> Cfg_IgnoreArmor_ArmorReduction = null!;
        public static ConfigEntry<int> Cfg_FoodDrain_FoodReduction = null!;
        public static ConfigEntry<int> Cfg_FireInfused_DamagePercent = null!;
        public static ConfigEntry<int> Cfg_FrostInfused_DamagePercent = null!;
        public static ConfigEntry<int> Cfg_PoisonInfused_DamagePercent = null!;
        public static ConfigEntry<int> Cfg_LightningInfused_DamagePercent = null!;
        public static ConfigEntry<int> Cfg_FastAttackSpeed_SpeedPercent = null!;
        public static ConfigEntry<int> Cfg_Knockback_StaggerForce = null!;
        public static ConfigEntry<int> Cfg_Knockback_PushForce = null!;
        public static ConfigEntry<float> Cfg_Forceful_PushMultiplier = null!;
        public static ConfigEntry<int> Cfg_Wet_ApplyChance = null!;
        public static ConfigEntry<int> Cfg_RemoveStatusEffect_Chance = null!;

        // Modifier_Defense
        public static ConfigEntry<int> Cfg_PierceImmunity_DamageReduction = null!;
        public static ConfigEntry<int> Cfg_SlashImmunity_DamageReduction = null!;
        public static ConfigEntry<int> Cfg_BluntImmunity_DamageReduction = null!;
        public static ConfigEntry<int> Cfg_ElementalImmunity_DamageReduction = null!;
        public static ConfigEntry<int> Cfg_FastMovement_SpeedPercent = null!;
        public static ConfigEntry<float> Cfg_DistantDetection_RangeMultiplier = null!;

        // Modifier_Lifesteal
        public static ConfigEntry<int> Cfg_Vampiric_HealPercent = null!;
        public static ConfigEntry<int> Cfg_Absorption_HealPercent = null!;
        public static ConfigEntry<int> Cfg_SoulEater_GrowthPerStack = null!;
        public static ConfigEntry<int> Cfg_BloodLoss_BurstDamagePercent = null!;

        // Modifier_Death
        public static ConfigEntry<int> Cfg_PoisonDeath_DamagePercent = null!;
        public static ConfigEntry<int> Cfg_FireDeath_DamagePercent = null!;
        public static ConfigEntry<int> Cfg_FrostDeath_DamagePercent = null!;
        public static ConfigEntry<int> Cfg_HealDeath_HealPercent = null!;
        public static ConfigEntry<int> Cfg_SummonDeath_ScalePercent = null!;
        public static ConfigEntry<int> Cfg_SummonDeath_HealthPercent = null!;
        

        private static string L(string ko, string en)
        {
            return System.Globalization.CultureInfo.CurrentUICulture.Name.StartsWith("ko") ? ko : en;
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                MonsterModifiersLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                MonsterModifiersLogger.LogError($"There was an issue loading your {ConfigFileName}");
                MonsterModifiersLogger.LogError("Please check your config entries for spelling and format!");
            }
        }
    }

    public static class KeyboardExtensions
    {
        public static bool IsKeyDown(this KeyboardShortcut shortcut)
        {
            return shortcut.MainKey != KeyCode.None && Input.GetKeyDown(shortcut.MainKey) &&
                   shortcut.Modifiers.All(Input.GetKey);
        }

        public static bool IsKeyHeld(this KeyboardShortcut shortcut)
        {
            return shortcut.MainKey != KeyCode.None && Input.GetKey(shortcut.MainKey) &&
                   shortcut.Modifiers.All(Input.GetKey);
        }
    }
}