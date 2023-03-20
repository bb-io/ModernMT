using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Requests
{
    public class DetectMultipleLanguagesRequest
    {
        public List<string> Texts { get; set; }
    }
}
