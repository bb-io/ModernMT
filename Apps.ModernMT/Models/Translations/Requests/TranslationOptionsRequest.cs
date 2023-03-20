using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Translations.Requests
{
    public class TranslationOptionsRequest : BaseTranslationRequest
    {
        public string Text { get; set; }

        public int NumberOfOptions { get; set; }

    }
}
