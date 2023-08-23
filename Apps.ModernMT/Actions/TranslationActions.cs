using Apps.ModernMT.Api;
using Apps.ModernMT.Models.Translations.Requests;
using Apps.ModernMT.Models.Translations.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using ModernMT.Model;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.ModernMT.Actions;

[ActionList]
public class TranslationActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TranslationActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Translate text", Description = "Translate into specified language")]
    public TranslationResponse TranslateIntoLanguage([ActionParameter] TranslationRequest input)
    {
        var client = new ModernMtClient(Creds);
        var translation = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Text);

        return new()
        {
            TranslatedText = translation.TranslationText
        };
    }

    [Action("Translate multiple texts", Description = "Translate multiple texts into specified language")]
    public MultipleTranslationResponse TranslateMultiple(
        [ActionParameter] MultipleTranslationRequest input)
    {
        var client = new ModernMtClient(Creds);
        var translations = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Texts.ToList());

        return new()
        {
            TranslatedTexts = translations.Select(t => t.TranslationText)
        };
    }

    [Action("Translate text with hints", Description = "Translate text with hints")]
    public TranslationResponse TranslateWithHints(
        [ActionParameter] TranslationWithHintsRequest input)
    {
        var client = new ModernMtClient(Creds);
        var translation = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Text,
            input.Hints?.ToArray());
        
        return new()
        {
            TranslatedText = translation.TranslationText
        };
    }

    [Action("Translate text with context", Description = "Translate text with specified context")]
    public TranslationResponse TranslateWithContext(
        [ActionParameter] TranslationWithContextRequest input)
    {
        var client = new ModernMtClient(Creds);
        var translation = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, null,
            input.Context);
        
        return new()
        {
            TranslatedText = translation.TranslationText
        };
    }

    [Action("Get translation options", Description = "Get translation options")]
    public TranslationOptionsResponse GetTranslationOptions(
        [ActionParameter] TranslationOptionsRequest input)
    {
        var client = new ModernMtClient(Creds);

        var translateOptions = input.NumberOfOptions is not null
            ? new TranslateOptions()
            {
                AltTranslations = input.NumberOfOptions.Value
            }
            : null;

        var translation = client.Translate(
            input.SourceLanguage,
            input.TargetLanguage,
            input.Text,
            input.Hints?.ToArray(),
            input.ContextVector,
            translateOptions);

        return new()
        {
            TranslatedText = translation.TranslationText,
            AlternativeOptions = translation.AltTranslations.ToList()
        };
    }
}