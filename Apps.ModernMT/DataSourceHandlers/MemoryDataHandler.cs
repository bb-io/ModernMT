using Apps.ModernMT.Api;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.ModernMT.DataSourceHandlers;

public class MemoryDataHandler(InvocationContext invocationContext) : BaseInvocable(invocationContext), IDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    IEnumerable<DataSourceItem> IDataSourceItemHandler.GetData(DataSourceContext context)
    {
        var client = new ModernMtClient(Creds);
        var memories = client.Memories.List();

        return memories
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
    }
}