using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.ModernMT.DataSourceHandlers
{
    public class FormatDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            { "text/plain", "Plain text" },
            { "text/xml", "XML" },
            { "text/html", "HTML" },
            { "application/xliff+xml", "XLIFF" }
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
