using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Xliff.Utils;
using Blackbird.Xliff.Utils.Extensions;

namespace Apps.ModernMT.Actions.Base;

public class BaseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : BaseInvocable(invocationContext)
{
    protected IEnumerable<AuthenticationCredentialsProvider> Credentials =>
        InvocationContext.AuthenticationCredentialsProviders;
        
    protected async Task<XliffDocument> GetXliffDocumentFromFile(FileReference fileReference)
    {
        var fileStream = await fileManagementClient.DownloadAsync(fileReference);
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return memoryStream.ToXliffDocument();
    }
}
