using Apps.ModernMT.Api;
using Apps.ModernMT.Models.ContextVector.Requests;
using Apps.ModernMT.Models.ContextVector.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.ModernMT.Actions;

[ActionList]
public class ContextVectorActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public ContextVectorActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get context vector from text", Description = "Get context vector from text")]
    public ContextVectorResponse GetContextVectorFromText([ActionParameter] ContextVectorRequest input)
    {
        var client = new ModernMtClient(Creds);
        var contextVector = client
            .GetContextVector(input.SourceLanguage, input.TargetLanguage, input.Text);
            
        return new()
        {
            ContextVector = contextVector
        };
    }
}