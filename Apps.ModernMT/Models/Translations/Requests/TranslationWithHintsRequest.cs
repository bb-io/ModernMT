﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Translations.Requests
{
    public class TranslationWithHintsRequest : BaseTranslationRequest
    {
        public string Text { get; set; }

        public List<long> Hints { get; set; }
    }
}
