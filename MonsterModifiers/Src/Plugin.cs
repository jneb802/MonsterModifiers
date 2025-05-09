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
using UnityEngine;
using Paths = BepInEx.Paths;

namespace MonsterModifiers
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class MonsterModifiersPlugin : BaseUnityPlugin
    {
        internal const string ModName = "MonsterModifiers";
        internal const string ModVersion = "1.2.2";
        internal const string Author = "warpalicious";
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
            
            Configurations_MaxModifiers = ConfigFileExtensions.BindConfig(Config, "Balance", "Max Modifiers",5,"The maximum amount of modifiers a creature can have.", true);
            
            // ShieldDome.LoadShieldDome();
            
            CompatibilityUtils.RunCompatibiltyChecks();
            
            StatusEffectUtils.CreateCustomStatusEffects();
            
            PrefabManager.OnVanillaPrefabsAvailable += PrefabUtils.CreateCustomPrefabs;
        }
        
        public static ConfigEntry<int> Configurations_MaxModifiers;
        

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