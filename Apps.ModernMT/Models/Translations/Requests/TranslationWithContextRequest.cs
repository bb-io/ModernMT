namespace Apps.ModernMT.Models.Translations.Requests;

public class TranslationWithContextRequest : TranslationRequest
{
    public string Context { get; set; }
}