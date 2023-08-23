using Apps.ModernMT.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using ModernMT;

namespace Apps.ModernMT.Api;

public class ModernMtClient : ModernMTService
{
    public ModernMtClient(IEnumerable<AuthenticationCredentialsProvider> creds) : base(GetApiKey(creds))
    {
    }
        
    private static string GetApiKey(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        return creds.Get(CredsNames.ApiKey).Value;
    }
}