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
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Extensions;
using System.Collections;

namespace Apps.ModernMT.Actions;

[ActionList]
public class TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : BaseActions(invocationContext, fileManagementClient)
{
    [Action("Translate text", Description = "Translate a small piece of text into the specified language")]
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

    [Action("Translate", Description = "Translate file content retrieved from a CMS or file storage. The output can be used in compatible actions")]
    public async Task<XliffTranslationResponse> TranslateFile([ActionParameter] TranslateFileRequest input)
    {
        if (input.SourceLanguage == input.TargetLanguage)
        {
            throw new PluginMisconfigurationException("The source language and target language are equal. This is not allowed. Please change the source or target language.");
        }

        var client = new ModernMtClient(Credentials);

        var stream = await fileManagementClient.DownloadAsync(input.File);
        var content = await Transformation.Parse(stream);
        var segmentTranslations = content
            .GetSegments()
            .Where(x => !x.IsIgnorbale && x.IsInitial)
            .Batch(100)
            .Process(batch => client.Translate(
                input.SourceLanguage, 
                input.TargetLanguage, 
                batch.Select(x => x.GetSource()).ToList(),
                input.Hints?.Select(long.Parse).ToArray(), 
                input.Context, 
                input.CreateOptions())
            );


        var billedCharacters = 0;
        foreach(var (segment, translation) in segmentTranslations)
        {
            segment.SetTarget(translation.TranslationText);
            segment.State = SegmentState.Translated;
            billedCharacters += translation.BilledCharacters;
        }

        if (input.OutputFileHandling == null || input.OutputFileHandling == "xliff")
        {
            var xliffStream = content.Serialize().ToStream();
            var fileName = input.File.Name.EndsWith("xliff") || input.File.Name.EndsWith("xlf") ? input.File.Name : input.File.Name + ".xliff";
            var uploadedFile = await fileManagementClient.UploadAsync(xliffStream, "application/xliff+xml", fileName);
            return new XliffTranslationResponse { File = uploadedFile, BilledCharacters = billedCharacters };
        }
        else
        {
            var resultStream = content.Target().Serialize().ToStream();
            var uploadedFile = await fileManagementClient.UploadAsync(resultStream, input.File.ContentType, input.File.Name);
            return new XliffTranslationResponse { File = uploadedFile, BilledCharacters = billedCharacters };
        }
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
            File = finalFile, 
            BilledCharacters = billedChars 
        };
    }
}