using Apps.ModernMT.Api;
using Apps.ModernMT.Models.Translations.Requests;
using Apps.ModernMT.Models.Translations.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Xliff.Utils.Models;
using Blackbird.Xliff.Utils;
using System.Xml.Linq;
using MoreLinq;

namespace Apps.ModernMT.Actions;

[ActionList]
public class TranslationActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private readonly IFileManagementClient _fileManagementClient;

    public TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
       invocationContext)
    {
        _fileManagementClient = fileManagementClient;
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

    [Action("Translate Xliff", Description = "Translate an XLIFF 1.2 document into specified language")]
    public async Task<XliffTranslationResponse> TranslateXliff([ActionParameter] TranslateXliffRequest input, [ActionParameter,
         Display("Bucket size",
             Description = "Specify the number of translation units to be processed at once. Default value: 15")]
        int? bucketSize)
    {
        var xliffDocument = await LoadAndParseXliffDocument(input.File);
        var sources = xliffDocument.TranslationUnits.Select(x => x.Source).ToList();

        List<string> allTranslatedTexts = new List<string>();
        int BilledChars = 0;

        foreach (var batch in sources.Batch(bucketSize.GetValueOrDefault(15)))
        {
            var client = new ModernMtClient(Creds);
            var translations = client.Translate(input.SourceLanguage, input.TargetLanguage, batch.ToList(), input.Hints?.Split(',').Select(x => long.Parse(x)).ToArray(), input.Context, input.CreateOptions());
            allTranslatedTexts.AddRange(translations.Select(x => x.TranslationText));
            BilledChars += translations.Sum(x => x.BilledCharacters);
        }

        var updatedDocument = UpdateXliffDocumentWithTranslations(xliffDocument, allTranslatedTexts);
        var fileReference = await UploadUpdatedDocument(updatedDocument, input.File);

        return new XliffTranslationResponse { TranslatedFile = fileReference};
    }
    private async Task<XliffDocument> LoadAndParseXliffDocument(FileReference inputFile)
    {
        var stream = await _fileManagementClient.DownloadAsync(inputFile);
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var xliffDoc = XDocument.Load(memoryStream);
        return XliffDocument.FromXDocument(xliffDoc,
            new XliffConfig { RemoveWhitespaces = true, CopyAttributes = true });
    }

    private async Task<FileReference> UploadUpdatedDocument(XDocument xliffDocument, FileReference originalFile)
    {
        var outputMemoryStream = new MemoryStream();
        xliffDocument.Save(outputMemoryStream);
        outputMemoryStream.Position = 0;

        string contentType = originalFile.ContentType ?? "application/xml";
        return await _fileManagementClient.UploadAsync(outputMemoryStream, contentType, originalFile.Name);
    }

    private XDocument UpdateXliffDocumentWithTranslations(XliffDocument xliffDocument, List<string> translatedTexts)
    {
        var updatedUnits = xliffDocument.TranslationUnits.Zip(translatedTexts, (unit, translation) =>
        {
            unit.Target = translation;
            return unit;
        }).ToList();

        return xliffDocument.UpdateTranslationUnits(updatedUnits);
    }
}