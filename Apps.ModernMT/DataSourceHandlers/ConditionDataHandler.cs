using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.DataSourceHandlers
{
    public class ConditionDataHandler : IStaticDataSourceItemHandler
    {
        protected Dictionary<string, string> EnumValues => new()
        {
            { ">", "Score is above threshold" },
            { ">=", "Score is above or equal threshold" },
            { "=", "Score is same as threshold" },
            { "<", "Score is below threshold" },
            { "<=", "Score is below or equal threshold" }
        };

        IEnumerable<DataSourceItem> IStaticDataSourceItemHandler.GetData()
        {
            return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
