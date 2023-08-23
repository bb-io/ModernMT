using Apps.ModernMT.Constants;
using Apps.ModernMT.Models.Error.Response;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json;

namespace Apps.ModernMT.Api.Http;

public class ModernMtRestClient : HttpClient
{
    public ModernMtRestClient()
    {
        BaseAddress = Urls.ApiUrl.ToUri();
    }

    public async Task<T> ExecuteWithHandling<T>(HttpRequestMessage request)
    {
        using var response = await ExecuteWithHandling(request);

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content)!;
    }
    
    public async Task<HttpResponseMessage> ExecuteWithHandling(HttpRequestMessage request)
    {
        var response = await SendAsync(request);

        if (response.IsSuccessStatusCode)
            return response;

        var content = await response.Content.ReadAsStringAsync();
        
        var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
        throw new(error.Error.Message);
    }
}