using Apps.ModernMT.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.ModernMT.Models.Quality
{
    public class EstimateXliffRequest
    {
        public FileReference File { get; set; }

        [Display("Source Language")]
        [StaticDataSource(typeof(LanguageDataHandler))]
        public string SourceLanguage { get; set; }

        [Display("Target Language")]
        [StaticDataSource(typeof(LanguageDataHandler))]
        public string TargetLanguage { get; set; }

        public float? Threshold { get; set; }

        [StaticDataSource(typeof(ConditionDataHandler))]
        public string? Condition { get; set; }

        [Display("New Target State")]
        [StaticDataSource(typeof(XliffStateDataHandler))]
        public string? State { get; set; }

    }
}
