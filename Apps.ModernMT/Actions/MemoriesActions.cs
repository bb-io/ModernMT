using Apps.ModernMT.Api;
using Apps.ModernMT.Api.Http;
using Apps.ModernMT.Dtos;
using Apps.ModernMT.Models.Memories.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using Apps.ModernMT.Models.Memories.Requests;
using ModernMT.Model;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;

namespace Apps.ModernMT.Actions;

[ActionList("Memories")]
public class MemoriesActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;
    
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public MemoriesActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Get memory", Description = "Get memory metadata")]
    public MemoryDto GetMemory([ActionParameter] MemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        var memory = client.Memories.Get(input.MemoryId);

        return new(memory);
    }

    [Action("Create memory", Description = "Create a new memory")]
    public MemoryDto CreateMemory([ActionParameter] CreateMemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        var memory = client.Memories.Create(input.Name, input.Description);

        return new(memory);
    }

    [Action("Update memory", Description = "Update memory metadata")]
    public void UpdateMemory([ActionParameter] UpdateMemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        client.Memories.Edit(input.MemoryId, input.Name, input.Description);
    }

    [Action("Delete memory", Description = "Permanently delete a memory and its content. It cannot be undone")]
    public void DeleteMemory([ActionParameter] MemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        client.Memories.Delete(input.MemoryId);
    }

    [Action("Add translation to memory", Description = "Add a sentence-translation pair to the given memory")]
    public void AddTranslationToMemory([ActionParameter] TranslationToMemoryRequest input)
    {
        var client = new ModernMtClient(Creds);

        client.Memories.Add(
            input.MemoryId,
            input.SourceLanguage,
            input.TargetLanguage,
            input.OriginalSentence,
            input.Translation,
            input.TranslationUId,
            input.Session);
    }

    [Action("Update memory translation pair", Description = "Update a sentence-translation pair in the given memory")]
    public void UpdateMemoryTranslationPair([ActionParameter] UpdateMemoryTranslationRequest input)
    {
        var client = new ModernMtClient(Creds);

        client.Memories.Replace(
            input.MemoryId,
            input.TranslationUId,
            input.SourceLanguage,
            input.TargetLanguage,
            input.OriginalSentence,
            input.Translation,
            input.Session);
    }

    [Action("Import memory from tmx", Description = "Import memory from tmx file")]
    public async Task ImportMemory([ActionParameter] ImportMemoryRequest input)
    {
        using var client = new ModernMtRestClient();
        var mtClient = new ModernMtClient(Creds);

        using var request = new ModernMtRestRequest($"/memories/{input.MemoryId}/content", HttpMethod.Post, Creds);
        var fileStream = await _fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileStream.GetByteData();
        request.AddFile(fileBytes, "tmx", "tmx");

        var response = await client.ExecuteWithHandling<ImportTmxResponse>(request);

        ImportJob job = response.Data;

        while (job.Progress < 1)
        {
            await Task.Delay(5000);
            job = mtClient.Memories.GetImportStatus(job.Id);
        }
    }
}