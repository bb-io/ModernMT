using Blackbird.Applications.Sdk.Common.Authentication;
using ModernMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT
{
    public class ModernMTClient : ModernMTService
    {
        private static string GetApiKey(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            return authenticationCredentialsProviders.First(p => p.KeyName == "apiKey").Value;
        }
        public ModernMTClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(GetApiKey(authenticationCredentialsProviders)) { }
    }
}
