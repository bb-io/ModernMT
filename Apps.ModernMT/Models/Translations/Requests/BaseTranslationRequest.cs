using Apps.ModernMT.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using ModernMT.Model;

namespace Apps.ModernMT.Models.Translations.Requests;

public class BaseTranslationRequest
{
    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguage { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLanguage { get; set; }


    [Display("Context vector")]
    public string? Context { get; set; }

    // Todo, change into multiple strings when we support dynamic array input
    [Display("Hint")]
    [DataSource(typeof(MemoryDataHandler))]
    public string? Hints { get; set; }

    [Display("Format")]
    [DataSource(typeof(FormatDataHandler))]
    public string? Format { get; set; }

    // Todo, change into multiple strings whenwe support dynamic array input
    [Display("Glossary")]
    [DataSource(typeof(MemoryDataHandler))]
    public string? Glossaries { get; set; }

    [Display("Ignore glossary case")]
    public bool? IgnoreGlossaryCase { get; set; }

    [Display("Priority")]
    [DataSource(typeof(PriorityDataHandler))]
    public string? Priority { get; set; }

    [Display("Multiline")]
    public bool? Multiline { get; set; }

    [Display("Timeout")]
    public int? Timeout { get; set; }

    [Display("Alternative translations")]
    public int? AltTranslations { get; set; }

    [Display("Mask profanities")]
    public bool? MaskProfanities { get; set; }

    [Display("Session")]
    public string? Session { get; set; }

    public TranslateOptions CreateOptions()
    {
        var options = new TranslateOptions
        {
            Priority = Priority ?? string.Empty,
            Multiline = Multiline ?? true,
            Timeout = Timeout ?? 10000,
            Format = Format ?? string.Empty,
            AltTranslations = AltTranslations ?? 0,
            Session = Session ?? string.Empty,
            IgnoreGlossaryCase = IgnoreGlossaryCase ?? false,
            MaskProfanities = MaskProfanities ?? false,
        };

        options.SetGlossaries(new List<string> { Glossaries });

        return options;
    }
}