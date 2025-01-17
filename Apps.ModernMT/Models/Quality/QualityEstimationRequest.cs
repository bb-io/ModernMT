using Apps.ModernMT.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.ModernMT.Models.Quality
{
    public class QualityEstimationRequest
    {
        [Display("Source language")]
        [StaticDataSource(typeof(LanguageDataHandler))]
        public string SourceLanguage { get; set; }

        [Display("Target language")]
        [StaticDataSource(typeof(LanguageDataHandler))]
        public string TargetLanguage { get; set; }

        [Display("Sentence")]
        public string Sentence { get; set; }

        [Display("Translation")]
        public string Translation { get; set; }
    }
}
