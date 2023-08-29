using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT.Models.Translations.Responses;

public class MultipleTranslationResponse
{
    [Display("Translations")]
    public IEnumerable<TranslationResponse> Translations { get; set; }
}