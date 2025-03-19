using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.DataSourceHandlers
{
    public class XliffStateDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            { "final", "final" },
            { "needs-adaptation", "needs-adaptation" },
            { "needs-l10n", "needs-l10n" },
            { "needs-review-adaptation", "needs-review-adaptation" },
            { "needs-review-l10n", "needs-review-l10n" },
            { "needs-review-translation", "needs-review-translation" },
            { "needs-translation", "needs-translation" },
            { "new", "new" },
            { "signed-off", "signed-off" },
            { "translated", "translated"}
        };

        IEnumerable<DataSourceItem> IStaticDataSourceItemHandler.GetData()
        {
            return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
