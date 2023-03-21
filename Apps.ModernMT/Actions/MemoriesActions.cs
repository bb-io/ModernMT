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

namespace Apps.ModernMT.Actions
{
    public class MemoriesActions
    {
        [Action("Get all memories", Description = "Get all memories")]
        public AllMemoriesResponse GetAllMemories(AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
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
        public MemoryResponse GetMemory(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            MemoryRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var memory = mmt.Memories.Get(input.Id);
            return new MemoryResponse()
            {
                Id = memory.Id,
                Name = memory.Name, 
                CreatedOn = memory.CreationDate,
            };
        }

        [Action("Create memory", Description = "Create memory")]
        public void CreateMemory(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            CreateMemoryRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            mmt.Memories.Create(input.Name, input.Description);
        }

        [Action("Update memory", Description = "Update memory by id")]
        public void UpdateMemory(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            UpdateMemoryRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            mmt.Memories.Edit(input.Id, input.Name, input.Description);
        }

        [Action("Delete memory", Description = "Delete memory by id")]
        public void DeleteMemory(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            DeleteMemoryRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            mmt.Memories.Delete(input.Id);
        }

        [Action("Add translation to memory", Description = "Add translation pair to memory")]
        public void AddTranslationToMemory(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            TranslationToMemoryRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            mmt.Memories.Add(input.Id, input.SourceLanguage, input.TargetLanguage, input.OriginalSentence, input.Translation);
        }
    }
}
