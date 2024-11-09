using System.Reflection;
using Jotunn.Utils;
using UnityEngine;
using UnityEngine.U2D;

namespace MonsterModifiers;

public class ModifierAssetUtils
{
    public static AssetBundle ashlandsAssetBundle;
    public static AssetBundle statusEffectBundle;
    public static AssetBundle modiferIconsBundle;

    public static Sprite swordIcon;
    public static Sprite shieldIcon;
    public static Sprite plusSquareIcon;
    public static Sprite circleIcon;
    public static Sprite soulIcon;
    public static Sprite skullIcon;
    public static Sprite appleIcon;
    public static Sprite shieldBrokenIcon;
    public static Sprite potionIcon;
    public static Sprite bloodIcon;
    public static Sprite bloodIconRed;
    public static Sprite heartIcon;

    public static void Setup()
    {
        ashlandsAssetBundle = AssetUtils.LoadAssetBundleFromResources("monster_modifiers_ashlands", Assembly.GetExecutingAssembly());
        statusEffectBundle = AssetUtils.LoadAssetBundleFromResources("statusicon", Assembly.GetExecutingAssembly());
        modiferIconsBundle = AssetUtils.LoadAssetBundleFromResources("modifiericons", Assembly.GetExecutingAssembly());
    }

    public static void LoadAllIcons()
    {
        swordIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/Sword.png");
        shieldIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/Shield.png");
        plusSquareIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/PlusBox.png");
        circleIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/Circle.png");
        soulIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/SoulEater.png");
        skullIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/Skull.png");
        appleIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/Apple.png");
        shieldBrokenIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/ShieldBroken.png");
        potionIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/Potion.png");
        heartIcon = modiferIconsBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/heartIcon.png");
        
        bloodIcon = statusEffectBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/bloodIcon.png");
        bloodIconRed = statusEffectBundle.LoadAsset<Sprite>("Assets/WarpProjects/Modifiers/NewModifierIcons/bloodIconRed.png");
    }


    
}