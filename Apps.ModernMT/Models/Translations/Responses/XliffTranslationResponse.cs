using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.Translate;

namespace Apps.ModernMT.Models.Translations.Responses
{
    public class XliffTranslationResponse : ITranslateFileOutput
    {
        [Display("Translated File")]
        public FileReference File { get; set; }

        [Display("Billed Characters")]
        public int BilledCharacters { get; set; }
    }
}
