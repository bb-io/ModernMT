using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.ModernMT.DataSourceHandlers
{
    public class PriorityDataHandler : EnumDataHandler
    {
        protected override Dictionary<string, string> EnumValues => new()
        {
            { "normal", "Normal" },
            { "background", "Background" }
        };
    }
}
