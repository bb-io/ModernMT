using Apps.ModernMT.Models.Translations.Requests;

namespace Apps.ModernMT.Models.ContextVector.Requests;

public class ContextVectorRequest : BaseTranslationRequest
{
    public string Text { get; set; }
}