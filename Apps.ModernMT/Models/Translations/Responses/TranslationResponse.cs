﻿using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.ModernMT.Models.Translations.Responses;

public class TranslationResponse : ITranslateTextOutput
{
    [Display("Translation")]
    public string TranslatedText { get; set; }

    [Display("Context vector")]
    public string? ContextVector { get; set; }

    [Display("Number of characters")]
    public int Characters { get; set; }

    [Display("Number of characters billed")]
    public int BilledCharacters { get; set; }

    [Display("Detected language")]
    public string DetectedLanguage { get; set; }

    [Display("Alternative translations")]
    public List<string>? Alternatives { get; set; }

}