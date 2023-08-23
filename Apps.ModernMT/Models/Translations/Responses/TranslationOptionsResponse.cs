using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT.Models.Translations.Responses;

public class TranslationOptionsResponse : TranslationResponse
{
    [Display("Alternative options")] public IEnumerable<string> AlternativeOptions { get; set; }
}