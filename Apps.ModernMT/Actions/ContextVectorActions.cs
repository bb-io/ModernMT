using Apps.ModernMT.Models.ContextVector.Requests;
using Apps.ModernMT.Models.ContextVector.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using ModernMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Actions
{
    [ActionList]
    public class ContextVectorActions
    {
        [Action("Get context vector from text", Description = "Get context vector from text")]
        public ContextVectorResponse GetContextVectorFromText(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ContextVectorRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            var contextVectors = mmt.GetContextVector(input.SourceLanguage, input.TargetLanguages, input.Text);
            return new ContextVectorResponse()
            {
                ContextVectors = contextVectors
            };
        }
    }
}
