using System.Collections.Generic;
using Jotunn.Entities;

namespace MonsterModifiers;

public class TranslationUtils
{
    public static CustomLocalization Localization;
    
    public static void AddLocalizations()
    {
        Localization = Jotunn.Managers.LocalizationManager.Instance.GetLocalization();
        
        // Add translations for the custom item in AddClonedItems
        Localization.AddTranslation("English", new Dictionary<string, string>
        {
            {"item_sigil", "Sigil"},
            {"item_sigil_description", "A magical Sigil"},
            {"piece_altar", "Altar"},
        });
    }
}