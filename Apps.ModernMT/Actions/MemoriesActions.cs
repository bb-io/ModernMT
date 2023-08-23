using Apps.ModernMT.Api;
using Apps.ModernMT.Api.Http;
using Apps.ModernMT.Constants;
using Apps.ModernMT.Dtos;
using Apps.ModernMT.Models;
using Apps.ModernMT.Models.Error.Response;
using Apps.ModernMT.Models.Memories.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using Apps.ModernMT.Models.Memories.Requests;
using ModernMT.Model;
using Newtonsoft.Json.Linq;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Newtonsoft.Json;

namespace Apps.ModernMT.Actions;

[ActionList]
public class MemoriesActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public MemoriesActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get all memories", Description = "Get all memories")]
    public AllMemoriesResponse GetAllMemories()
    {
        var client = new ModernMtClient(Creds);
        var memories = client.Memories.List();

        return new()
        {
            Memories = memories.Select(m => new MemoryDto(m)).ToArray()
        };
    }

    [Action("Get memory", Description = "Get memory by id")]
    public MemoryDto GetMemory([ActionParameter] MemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        var memory = client.Memories.Get(input.MemoryId);

        return new(memory);
    }

    [Action("Create memory", Description = "Create memory")]
    public MemoryDto CreateMemory([ActionParameter] CreateMemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        var memory = client.Memories.Create(input.Name, input.Description);

        return new(memory);
    }

    [Action("Update memory", Description = "Update memory by id")]
    public void UpdateMemory([ActionParameter] UpdateMemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        client.Memories.Edit(input.MemoryId, input.Name, input.Description);
    }

    [Action("Delete memory", Description = "Delete memory by id")]
    public void DeleteMemory([ActionParameter] MemoryRequest input)
    {
        var client = new ModernMtClient(Creds);
        client.Memories.Delete(input.MemoryId);
    }

    [Action("Add translation to memory", Description = "Add translation pair to memory")]
    public void AddTranslationToMemory([ActionParameter] TranslationToMemoryRequest input)
    {
        var client = new ModernMtClient(Creds);

        client.Memories.Add(
            input.MemoryId,
            input.SourceLanguage,
            input.TargetLanguage,
            input.OriginalSentence,
            input.Translation,
            input.TranslationUId);
    }

    [Action("Update memory translation pair", Description = "Update memory translation pair")]
    public void UpdateMemoryTranslationPair([ActionParameter] UpdateMemoryTranslationRequest input)
    {
        var client = new ModernMtClient(Creds);

        client.Memories.Replace(
            input.MemoryId,
            input.TranslationUId,
            input.SourceLanguage,
            input.TargetLanguage,
            input.OriginalSentence,
            input.Translation);
    }

    [Action("Import memory from tmx", Description = "Import memory from tmx file")]
    public async Task<ImportMemoryResponse> ImportMemory([ActionParameter] ImportMemoryRequest input)
    {
        using var client = new ModernMtRestClient();
    
        using var request = new ModernMtRestRequest($"/memories/{input.MemoryId}/content", HttpMethod.Post, Creds);
        request.AddFile(input.File, "tmx", "tmx");

        var response = await client.ExecuteWithHandling<ImportTmxResponse>(request);
        return new(response.Data);
    }

    [Action("Get import status", Description = "Get import status by Id")]
    public ImportMemoryResponse GetImportStatus([ActionParameter] GetImportStatusRequest input)
    {
        var client = new ModernMtClient(Creds);
        var importJob = client.Memories.GetImportStatus(input.ImportId);

        return new(importJob);
    }
}