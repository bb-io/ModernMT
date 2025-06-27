using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.ModernMT.Models.Translations.Requests;

public class TranslationRequest : BaseTranslationRequest, ITranslateTextInput
{
    public string Text { get; set; }
}