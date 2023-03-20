using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Translations.Responses
{
    public class TranslationOptionsResponse
    {
        public string TranslatedText { get; set; }

        public IEnumerable<string> AlternativeOptions { get; set; }
    }
}
