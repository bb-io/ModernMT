using Apps.ModernMT.Api;
using Apps.ModernMT.Extensions;
using Apps.ModernMT.Models.Memories.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;

namespace Apps.ModernMT.Actions;

[ActionList]
public class GlossaryActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private readonly IFileManagementClient _fileManagementClient;

    public GlossaryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
        invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Import glossary", Description = "Import glossary from a file")]
    public async Task ImportGlossary([ActionParameter] ImportMemoryRequest input)
    {
        var client = new ModernMtClient(Creds);

        await using var glossaryStream = await _fileManagementClient.DownloadAsync(input.File);
        var blackbirdGlossary = await glossaryStream.ConvertFromTBX();
        var csv = blackbirdGlossary.ToModernMtCsv();

        var filePath = $"{input.MemoryId}.csv";
        try
        {
            await File.WriteAllTextAsync(filePath, csv);
            client.Memories.ImportGlossary(input.MemoryId, File.OpenRead(filePath), "equivalent");
        }
        finally
        {
            File.Delete(filePath);
        }
    }
}