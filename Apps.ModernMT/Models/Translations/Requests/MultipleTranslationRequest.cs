namespace Apps.ModernMT.Models.Translations.Requests;

public class MultipleTranslationRequest : BaseTranslationRequest
{
    public IEnumerable<string> Texts { get; set; }

}