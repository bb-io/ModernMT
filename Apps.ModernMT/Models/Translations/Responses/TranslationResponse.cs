using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT.Models.Translations.Responses;

public class TranslationResponse
{
    [Display("Translated text")]
    public string TranslatedText { get; set; }
}