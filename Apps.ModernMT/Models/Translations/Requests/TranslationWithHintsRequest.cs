namespace Apps.ModernMT.Models.Translations.Requests;

public class TranslationWithHintsRequest : TranslationRequest
{
    public IEnumerable<long>? Hints { get; set; }
}