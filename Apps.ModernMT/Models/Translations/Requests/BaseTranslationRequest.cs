using Apps.ModernMT.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using ModernMT.Model;

namespace Apps.ModernMT.Models.Translations.Requests;

public class BaseTranslationRequest
{
    [Display("Source language")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string SourceLanguage { get; set; }

    [Display("Target language")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string TargetLanguage { get; set; }


    [Display("Context vector")] public string? Context { get; set; }

    [Display("Hint")]
    [DataSource(typeof(MemoryDataHandler))]
    public IEnumerable<string>? Hints { get; set; }

    [Display("Format")]
    [StaticDataSource(typeof(FormatDataHandler))]
    public string? Format { get; set; }

    [Display("Glossaries")]
    [DataSource(typeof(MemoryDataHandler))]
    public IEnumerable<string>? Glossaries { get; set; }

    [Display("Ignore glossary case")] public bool? IgnoreGlossaryCase { get; set; }

    [Display("Priority")]
    [StaticDataSource(typeof(PriorityDataHandler))]
    public string? Priority { get; set; }

    [Display("Multiline")] public bool? Multiline { get; set; }

    [Display("Timeout")] public int? Timeout { get; set; }

    [Display("Alternative translations")] public int? AltTranslations { get; set; }

    [Display("Mask profanities")] public bool? MaskProfanities { get; set; }

    [Display("Session")] public string? Session { get; set; }

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

        if (Glossaries is not null)
            options.SetGlossaries(Glossaries.ToList());

        return options;
    }
}