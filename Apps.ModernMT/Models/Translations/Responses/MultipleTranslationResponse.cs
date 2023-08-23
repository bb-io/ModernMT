using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT.Models.Translations.Responses;

public class MultipleTranslationResponse
{
    [Display("Translated texts")]
    public IEnumerable<string> TranslatedTexts { get; set; }
}