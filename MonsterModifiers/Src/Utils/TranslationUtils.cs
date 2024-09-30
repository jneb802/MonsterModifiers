namespace MonsterModifiers;

public class TranslationUtils
{
    public void AddLocalizations()
    {
        var localization = Jotunn.Managers.LocalizationManager.Instance.GetLocalization();
        localization.AddTranslation("$item_sigil", "Sigil");
        localization.AddTranslation("$item_sigil_description", "A magical Sigil");
    }
}