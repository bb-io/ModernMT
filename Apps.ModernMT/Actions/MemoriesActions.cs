using Apps.ModernMT.Dtos;
using Apps.ModernMT.Models.Memories.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using ModernMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.ModernMT.Models.Memories.Requests;
using System.IO;
using RestSharp;
using Newtonsoft.Json;
using ModernMT.Model;
using Newtonsoft.Json.Linq;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.ModernMT.Actions
{
    [ActionList]
    public class MemoriesActions
    {
        [Action("Get all memories", Description = "Get all memories")]
        public AllMemoriesResponse GetAllMemories(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            var memories = mmt.Memories.List();
            return new AllMemoriesResponse()
            {
                Memories = memories.Select(m => new MemoryDto()
                {
                    Name = m.Name,
                    CreatedOn = m.CreationDate,
                    Id = m.Id
                })
            };
        }

        [Action("Get memory", Description = "Get memory by id")]
        public MemoryResponse GetMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] MemoryRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            var memory = mmt.Memories.Get(input.Id);
            return new MemoryResponse()
            {
                Id = memory.Id,
                Name = memory.Name, 
                CreatedOn = memory.CreationDate,
            };
        }

        [Action("Create memory", Description = "Create memory")]
        public MemoryResponse CreateMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateMemoryRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            var memory = mmt.Memories.Create(input.Name, input.Description);
            return new MemoryResponse()
            {
                Id = memory.Id,
                CreatedOn = memory.CreationDate,
                Name = memory.Name,
            };
        }

        [Action("Update memory", Description = "Update memory by id")]
        public void UpdateMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] UpdateMemoryRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            mmt.Memories.Edit(input.Id, input.Name, input.Description);
        }

        [Action("Delete memory", Description = "Delete memory by id")]
        public void DeleteMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteMemoryRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            mmt.Memories.Delete(input.Id);
        }

        [Action("Add translation to memory", Description = "Add translation pair to memory")]
        public void AddTranslationToMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslationToMemoryRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            mmt.Memories.Add(input.Id, input.SourceLanguage, input.TargetLanguage, input.OriginalSentence, input.Translation, input.TranslationUId);
        }

        [Action("Update memory translation pair", Description = "Update memory translation pair")]
        public void UpdateMemoryTranslationPair(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] UpdateMemoryTranslationRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            mmt.Memories.Replace(input.Id, input.TranslationUId, input.SourceLanguage, input.TargetLanguage,
                input.OriginalSentence, input.Translation);
        }

        [Action("Import memory from tmx", Description = "Import memory from tmx file")]
        public ImportMemoryResponse ImportMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ImportMemoryRequest input)
        {
            var result = ImportMemoryFromTmx(authenticationCredentialsProviders, input.MemoryId, input.File);

            return new ImportMemoryResponse()
            {
                ImportJobId = result.Id,
                MemoryId = result.Memory,
                Size = result.Size,
                Progress = result.Progress,
            };
        }

        [Action("Get import status", Description = "Get import status by Id")]
        public ImportMemoryResponse GetImportStatus(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetImportStatusRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            var importJob = mmt.Memories.GetImportStatus(input.ImportId);

            return new ImportMemoryResponse()
            {
                ImportJobId = importJob.Id,
                MemoryId = importJob.Memory,
                Size = importJob.Size,
                Progress = importJob.Progress,
            };
        }

        private ImportJob ImportMemoryFromTmx(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, long memoryId, byte[] file)
        {
            var _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.modernmt.com")
            };
            var apiKey = authenticationCredentialsProviders.First(p => p.KeyName == "apiKey").Value;
            _httpClient.DefaultRequestHeaders.Add("MMT-ApiKey", apiKey);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/memories/{memoryId}/content");
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
            using (var stream = new MemoryStream(file))
            {
                multipartFormDataContent.Add(new StreamContent(stream), "tmx", "tmx");

                httpRequestMessage.Content = multipartFormDataContent;
                var result = _httpClient.Send(httpRequestMessage).Content.ReadAsStringAsync().Result;
                JObject jObject = JObject.Parse(result);
                JToken jToken = jObject["data"];
                ImportJob importJob = (ImportJob)jToken.ToObject(typeof(ImportJob));
                return importJob;
            }
        }
    }
}
