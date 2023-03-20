using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Responses
{
    public class DetectMultipleLanguagesResponse
    {
        public IEnumerable<string> Languages { get; set; }
    }
}
