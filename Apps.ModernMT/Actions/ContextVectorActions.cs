using Apps.ModernMT.Api;
using Apps.ModernMT.Models.ContextVector.Requests;
using Apps.ModernMT.Models.ContextVector.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.ModernMT.Actions;

[ActionList("Context vectors")]
public class ContextVectorActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public ContextVectorActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get context vector from text", Description = "Analyze the given content, compare it with the memories content, and automatically create a context vector which can be used in the attempt to maximize the translation quality")]
    public ContextVectorResponse GetContextVectorFromText([ActionParameter] ContextVectorRequest input)
    {
        var client = new ModernMtClient(Creds);
        var contextVector = input.Limit == null ?
            client.GetContextVector(input.SourceLanguage, input.TargetLanguage, input.Text, input.Hints?.Split(',').Select(x => long.Parse(x)).ToArray()) :
            client.GetContextVector(input.SourceLanguage, input.TargetLanguage, input.Text, input.Hints?.Split(',').Select(x => long.Parse(x)).ToArray(), (int) input.Limit) ;
            
        return new()
        {
            ContextVector = contextVector
        };
    }
}