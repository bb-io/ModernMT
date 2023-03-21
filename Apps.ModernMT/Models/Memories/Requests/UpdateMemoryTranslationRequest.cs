using Apps.ModernMT.Models.Translations.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Memories.Requests
{
    public class UpdateMemoryTranslationRequest : BaseTranslationRequest
    {
        public long Id { get; set; }

        public string OriginalSentence { get; set; }

        public string Translation { get; set; }

        public string TranslationUId { get; set; }
    }
}
