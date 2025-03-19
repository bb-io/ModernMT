using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.ModernMT.Models.Translations.Requests
{
    public class TranslateXliffRequest : BaseTranslationRequest
    {
        public FileReference File { get; set; }
    }
}
