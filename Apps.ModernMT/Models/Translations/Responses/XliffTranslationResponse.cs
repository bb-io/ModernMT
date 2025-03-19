using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.ModernMT.Models.Translations.Responses
{
    public class XliffTranslationResponse
    {
        [Display("Translated File")]
        public FileReference TranslatedFile { get; set; }

        [Display("Billed Characters")]
        public int BilledCharacters { get; set; }
    }
}
