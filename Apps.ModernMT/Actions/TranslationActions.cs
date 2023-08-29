using Apps.ModernMT.Api;
using Apps.ModernMT.Models.Translations.Requests;
using Apps.ModernMT.Models.Translations.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using ModernMT.Model;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using System.Transactions;

namespace Apps.ModernMT.Actions;

[ActionList]
public class TranslationActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TranslationActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Translate", Description = "Translate into specified language")]
    public TranslationResponse TranslateIntoLanguage([ActionParameter] TranslationRequest input)
    {
        var client = new ModernMtClient(Creds);
        var translation = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, input.Hints?.Split(',').Select(x => long.Parse(x)).ToArray(), input.Context, input.CreateOptions() );

        return new()
        {
            TranslatedText = translation.TranslationText,
            ContextVector = translation.ContextVector,
            Characters = translation.Characters,
            BilledCharacters= translation.BilledCharacters,
            DetectedLanguage= translation.DetectedLanguage,
            Alternatives= translation.AltTranslations?.ToList(),
        };
    }

    [Action("Translate multiple", Description = "Translate multiple texts into specified language")]
    public MultipleTranslationResponse TranslateMultiple(
        [ActionParameter] MultipleTranslationRequest input)
    {
        var client = new ModernMtClient(Creds);
        var translations = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Texts.ToList(), input.Hints?.Split(',').Select(x => long.Parse(x)).ToArray(), input.Context, input.CreateOptions());

        return new()
        {
            Translations = translations.Select(t => new TranslationResponse
            {
                TranslatedText = t.TranslationText,
                ContextVector = t.ContextVector,
                Characters = t.Characters,
                BilledCharacters = t.BilledCharacters,
                DetectedLanguage = t.DetectedLanguage,
                Alternatives = t.AltTranslations?.ToList(),
            }),
        };
    }
}