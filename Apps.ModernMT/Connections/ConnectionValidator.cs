
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.ModernMT.Connections
{
    public class ConnectionValidator : IConnectionValidator
    {
        public async ValueTask<ConnectionValidationResponse> ValidateConnection(
       IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
        {
            return new() { IsValid = true };
        }
    }
}
