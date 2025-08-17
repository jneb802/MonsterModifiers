using System;
using System.Collections.Generic;
using System.IO;
using Jotunn.Utils;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MonsterModifiers;

public class YamlUtils
{
    public static string defaultModifierValues;
    
    public static Dictionary<MonsterModifierTypes, ModifierData> ParseDefaultYamls()
    { 
        defaultModifierValues = AssetUtils.LoadTextFromResources("modifierValues.yml");
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        
        return deserializer.Deserialize<Dictionary<MonsterModifierTypes, ModifierData>>(new StringReader(defaultModifierValues));
    }
    
}