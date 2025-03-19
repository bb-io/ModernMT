using Apps.ModernMT.Api;
using Apps.ModernMT.Models.Translations.Requests;
using Apps.ModernMT.Models.Translations.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using MoreLinq;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Apps.ModernMT.Actions.Base;

namespace Apps.ModernMT.Actions;

[ActionList]
public class TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : BaseActions(invocationContext, fileManagementClient)
{
    [Action("Translate", Description = "Translate into specified language")]
    public TranslationResponse TranslateIntoLanguage([ActionParameter] TranslationRequest input)
    {
        if (string.IsNullOrEmpty(input.Text))
        {
            throw new PluginMisconfigurationException("The input did not contain any text to translate. Please make sure the text is not empty.");
        }

        if (input.SourceLanguage == input.TargetLanguage)
        {
            throw new PluginMisconfigurationException("The source language and target language are equal. This is not allowed. Please change the source or target language.");
        }

        var client = new ModernMtClient(Credentials);
        var translation = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Text,
            input.Hints?.Select(long.Parse).ToArray(), input.Context, input.CreateOptions());

        return new()
        {
            TranslatedText = translation.TranslationText,
            ContextVector = translation.ContextVector,
            Characters = translation.Characters,
            BilledCharacters = translation.BilledCharacters,
            DetectedLanguage = translation.DetectedLanguage,
            Alternatives = translation.AltTranslations?.ToList(),
        };
    }

    [Action("Translate multiple", Description = "Translate multiple texts into specified language")]
    public MultipleTranslationResponse TranslateMultiple(
        [ActionParameter] MultipleTranslationRequest input)
    {
        if (input.SourceLanguage == input.TargetLanguage)
        {
            throw new PluginMisconfigurationException("The source language and target language are equal. This is not allowed. Please change the source or target language.");
        }

        var client = new ModernMtClient(Credentials);
        var translations = client.Translate(input.SourceLanguage, input.TargetLanguage, input.Texts.ToList(),
            input.Hints?.Select(long.Parse).ToArray(), input.Context, input.CreateOptions());

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

    [Action("Translate XLIFF", Description = "Translate an XLIFF 1.2 document into specified language")]
    public async Task<XliffTranslationResponse> TranslateXliff(
        [ActionParameter] TranslateXliffRequest input,
        [ActionParameter, Display("Bucket size", Description = "Specify the number of translation units to be processed at once. Default value: 15")]
        int? bucketSize = 15)
    {
        if (input.SourceLanguage == input.TargetLanguage)
        {
            throw new PluginMisconfigurationException("The source language and target language are equal. This is not allowed. Please change the source or target language.");
        }

        var xliffDocument = await GetXliffDocumentFromFile(input.File);
        var sources = xliffDocument.TranslationUnits.Select(x => x.Source).ToList();
        var results = new List<string>();
        var billedChars = 0;
        
        var batchSize = bucketSize ?? 15;
        var client = new ModernMtClient(Credentials);
        
        foreach (var batch in sources.Batch(batchSize))
        {
            var translations = client.Translate(
                input.SourceLanguage, 
                input.TargetLanguage, 
                batch.ToList(),
                input.Hints?.Select(long.Parse).ToArray(), 
                input.Context, 
                input.CreateOptions());

            results.AddRange(translations.Select(x => x.TranslationText));
            billedChars += translations.Sum(x => x.BilledCharacters);
        }

        if (results.Count != sources.Count)
        {
            throw new Exception($"The number of translations does not match the number of source texts. Expected: {sources.Count}, Actual: {results.Count}");
        }

        for (int i = 0; i < xliffDocument.TranslationUnits.Count; i++)
        {
            xliffDocument.TranslationUnits[i].Target = results[i];
        }
        
        var updatedFile = xliffDocument.ToStream();
        var finalFile = await fileManagementClient.UploadAsync(updatedFile, input.File.ContentType, input.File.Name);
        return new XliffTranslationResponse 
        { 
            TranslatedFile = finalFile, 
            BilledCharacters = billedChars 
        };
    }
}