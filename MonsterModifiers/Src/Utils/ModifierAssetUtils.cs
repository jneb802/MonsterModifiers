using System.Reflection;
using Jotunn.Utils;
using UnityEngine;

namespace MonsterModifiers;

public class ModifierAssetUtils
{
    public static AssetBundle ashlandsAssetBundle;
    public static AssetBundle statusEffectBundle;

    public static void Setup()
    {
        ashlandsAssetBundle = AssetUtils.LoadAssetBundleFromResources("monster_modifiers_ashlands", Assembly.GetExecutingAssembly());
        statusEffectBundle = AssetUtils.LoadAssetBundleFromResources("statusicon", Assembly.GetExecutingAssembly());
    }
    
}