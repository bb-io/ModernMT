using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Translations.Requests
{
    public class MultipleTranslationRequest : BaseTranslationRequest
    {
        public List<string> Texts { get; set; }

    }
}
