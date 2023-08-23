using Apps.ModernMT.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.ModernMT.Api.Http;

public class ModernMtRestRequest : HttpRequestMessage
{
    public ModernMtRestRequest(string resource, HttpMethod method, IEnumerable<AuthenticationCredentialsProvider> creds)
        : base(method, resource)
    {
        var apiKey = creds.Get(CredsNames.ApiKey).Value;
        Headers.Add("MMT-ApiKey", apiKey);
    }

    public void AddFile(byte[] file, string name, string fileName)
    {
        if (Content is not MultipartFormDataContent multipartFormDataContent)
            multipartFormDataContent = new();
        
        var stream = new MemoryStream(file);
        multipartFormDataContent.Add(new StreamContent(stream), name, fileName);
        Content = multipartFormDataContent;
    }
}