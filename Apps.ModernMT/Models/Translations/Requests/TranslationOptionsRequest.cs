using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT.Models.Translations.Requests;

public class TranslationOptionsRequest : TranslationRequest
{
    [Display("Number of options")]
    public int? NumberOfOptions { get; set; }
    
    [Display("Context vector")]
    public string? ContextVector { get; set; }
    
    public IEnumerable<long>? Hints { get; set; }

}