using System;
using Jotunn.Utils;

namespace MonsterModifiers;

public class YamlUtils
{
    public string defaultModiferValues;
    
    public void ParseDefaultYamls()
    { 
        defaultModiferValues = AssetUtils.LoadTextFromResources("modifierValues.yml");
    }
}