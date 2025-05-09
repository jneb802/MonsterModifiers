using UnityEngine;

namespace MonsterModifiers;

public class CompatibilityUtils
{
    public static bool isStarLevelsExpandedInstalled;
    
    public static void RunCompatibiltyChecks()
    {
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("CreatureLevelControl"))
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogWarning("CreatureLevelandLootControl plugin is installed. Please ensure special effects and infusions are disabled in CLLC configuration.");
        }  
        
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("MidnightsFX.StarLevelSystem"))
        {
            MonsterModifiersPlugin.MonsterModifiersLogger.LogWarning("StarLevelSystem plugin is installed. Please ensure max modifiers config is set.");

            isStarLevelsExpandedInstalled = true;
        }
    }
}