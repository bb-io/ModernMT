using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

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
