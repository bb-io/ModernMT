using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.ContextVector.Requests
{
    public class ContextVectorRequest
    {
        public string Text { get; set; }

        public string SourceLanguage { get; set; }

        public List<string> TargetLanguages { get; set; }
    }
}
