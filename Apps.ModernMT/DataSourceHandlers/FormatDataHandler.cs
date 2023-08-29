using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.DataSourceHandlers
{
    public class FormatDataHandler : EnumDataHandler
    {
        protected override Dictionary<string, string> EnumValues => new()
        {
            { "text/plain", "Plain text" },
            { "text/xml", "XML" },
            { "text/html", "HTML" },
            { "application/xliff+xml", "XLIFF" }
        };
    }
}
