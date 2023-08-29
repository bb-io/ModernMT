﻿using Apps.ModernMT.DataSourceHandlers;
using Apps.ModernMT.Models.Translations.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.ModernMT.Models.ContextVector.Requests;

public class ContextVectorRequest
{
    [Display("Text")]
    public string Text { get; set; }

    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguage { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLanguage { get; set; }

    // Todo, change into multiple strings when we support dynamic array input
    [Display("Hints")]
    [DataSource(typeof(MemoryDataHandler))]
    public string? Hints { get; set; }

    [Display("Limit")]
    public int? Limit { get; set; }


}