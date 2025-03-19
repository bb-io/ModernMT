using Apps.ModernMT.Api;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.ModernMT.Models.Quality;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Text;
using Apps.ModernMT.Actions.Base;
using System.Globalization;
using MoreLinq;

namespace Apps.ModernMT.Actions;

[ActionList]
public class QualityActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : BaseActions(invocationContext, fileManagementClient)
{
    [Action("Estimate Quality", Description = "Allows to evaluate the quality of a translation or a list of translations")]
    public QualityEstimationResponse Estimate([ActionParameter] QualityEstimationRequest input)
    {
        var client = new ModernMtClient(Credentials);
        var response = client.Qe(input.SourceLanguage, input.TargetLanguage, input.Sentence, input.Translation);
        return new()
        {
            Score = response.Score
        };
    }

    [Action("Estimate XLIFF Quality", Description = "Evaluate the quality of a translated XLIFF file")]
    public async Task<EstimateXliffResponse> EstimateXliff(
        [ActionParameter] EstimateXliffRequest input,
        [ActionParameter, Display("Bucket size", Description = "Specify the number of translation units to be processed at once. Default value: 15")] int? bucketSize = 15)
    {
        var xliffDocument = await GetXliffDocumentFromFile(input.File);
        var results = new Dictionary<string, double>();
        
        var batchSize = bucketSize ?? 15;
        var client = new ModernMtClient(Credentials);
        
        foreach (var batch in xliffDocument.TranslationUnits.Batch(batchSize))
        {
            foreach (var transunit in batch)
            {
                var response = client.Qe(input.SourceLanguage, input.TargetLanguage, transunit.Source, transunit.Target);
                results.Add(transunit.Id, response.Score);

                var formattedScore = response.Score.ToString("F2", CultureInfo.InvariantCulture);
                var attribute = transunit.Attributes.FirstOrDefault(x => x.Key == "extradata");
                if (!string.IsNullOrEmpty(attribute.Key))
                {
                    transunit.Attributes.Remove(attribute.Key);
                    transunit.Attributes.Add("extradata", formattedScore);
                }
                else
                {
                    transunit.Attributes.Add("extradata", formattedScore);
                }
            }
        }

        if (input.Threshold != null && input.Condition != null && input.State != null)
        {
            var filteredTUs = new List<string>();
            switch (input.Condition)
            {
                case ">":
                    filteredTUs = results.Where(x => x.Value > input.Threshold).Select(x => x.Key).ToList();
                    break;
                case ">=":
                    filteredTUs = results.Where(x => x.Value >= input.Threshold).Select(x => x.Key).ToList();
                    break;
                case "=":
                    filteredTUs = results.Where(x => x.Value == input.Threshold).Select(x => x.Key).ToList();
                    break;
                case "<":
                    filteredTUs = results.Where(x => x.Value < input.Threshold).Select(x => x.Key).ToList();
                    break;
                case "<=":
                    filteredTUs = results.Where(x => x.Value <= input.Threshold).Select(x => x.Key).ToList();
                    break;
            }

            filteredTUs.ForEach(x =>
            {
                var translationUnit = xliffDocument.TranslationUnits.FirstOrDefault(tu => tu.Id == x);
                if (translationUnit != null)
                {
                    var stateAttribute = translationUnit.Attributes.FirstOrDefault(x => x.Key == "state");
                    if (!string.IsNullOrEmpty(stateAttribute.Key))
                    {
                        translationUnit.Attributes.Remove(stateAttribute.Key);
                        translationUnit.Attributes.Add("state", input.State);
                    }
                    else
                    {
                        translationUnit.Attributes.Add("state", input.State);
                    }
                }
            });
        }

        var stream = xliffDocument.ToStream();
        return new EstimateXliffResponse
        {
            AverageScore = results.Average(x => x.Value),
            File = await fileManagementClient.UploadAsync(stream, MediaTypeNames.Text.Xml, input.File.Name)
        };
    }
}
