using Apps.ModernMT.Api;
using Apps.ModernMT.Models.LanguageDetection.Requests;
using Apps.ModernMT.Models.LanguageDetection.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.ModernMT.Actions;

[ActionList]
public class LanguageDetectionActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public LanguageDetectionActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
        
    [Action("Detect language", Description = "Allows to detect the language of an input text")]
    public DetectLanguageResponse DetectLanguage([ActionParameter] DetectLanguageRequest input)
    {
        var client = new ModernMtClient(Creds);
        var language = client.DetectLanguage(input.Text);
            
        return new()
        {
            Language = language.Language
        };
    }

    [Action("Detect multiple languages", Description = "Allows to detect the language of a list of texts")]
    public DetectMultipleLanguagesResponse DetectMultipleLanguages(
        [ActionParameter] DetectMultipleLanguagesRequest input)
    {
        var client = new ModernMtClient(Creds);
        var languages = client.DetectLanguage(input.Texts.ToList());
            
        return new()
        {
            Languages = languages.Select(l => l.Language).ToList()
        };
    }
}