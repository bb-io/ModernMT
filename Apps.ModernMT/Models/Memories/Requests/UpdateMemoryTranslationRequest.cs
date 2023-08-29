using Apps.ModernMT.DataSourceHandlers;
using Apps.ModernMT.Models.Translations.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.ModernMT.Models.Memories.Requests;

public class UpdateMemoryTranslationRequest
{
    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguage { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLanguage { get; set; }

    [Display("Memory")]
    [DataSource(typeof(MemoryDataHandler))]
    public string MemoryId { get; set; }

    [Display("Original sentence")]
    public string OriginalSentence { get; set; }

    public string Translation { get; set; }

    [Display("Translation UID")]
    public string TranslationUId { get; set; }

    [Display("Session")]
    public string Session { get; set; }
}