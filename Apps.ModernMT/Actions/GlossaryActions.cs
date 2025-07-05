using System.Text;
using Apps.ModernMT.Api.Http;
using Apps.ModernMT.Extensions;
using Apps.ModernMT.Models.Memories.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;

namespace Apps.ModernMT.Actions;

[ActionList("Glossaries")]
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
        using var client = new ModernMtRestClient();

        await using var glossaryStream = await _fileManagementClient.DownloadAsync(input.File);
        var blackbirdGlossary = await glossaryStream.ConvertFromTBX();
        var csv = blackbirdGlossary.ToModernMtCsv();

        using var request = new ModernMtRestRequest($"/memories/{input.MemoryId}/glossary", HttpMethod.Post, Creds);

        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(new StreamContent(fileStream), "csv", $"{input.MemoryId}.csv");
        multipartFormDataContent.Add(new StringContent("equivalent"), "type");
        request.Content = multipartFormDataContent;

        await client.ExecuteWithHandling(request);
    }
}