
using Apps.ModernMT.Actions;
using Apps.ModernMT.Models.LanguageDetection.Requests;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.ModernMT.Connections
{
    public class ConnectionValidator : IConnectionValidator
    {
        public async ValueTask<ConnectionValidationResponse> ValidateConnection(
       IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
        {
            var actions = new LanguageDetectionActions(new InvocationContext() { AuthenticationCredentialsProviders = authProviders });
            try
            {
                actions.DetectLanguage(new DetectLanguageRequest() { Text = "validate"});
                return new ConnectionValidationResponse
                {
                    IsValid = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new ConnectionValidationResponse
                {
                    IsValid = false,
                    Message = ex.Message
                };
            }
        }
    }
}
