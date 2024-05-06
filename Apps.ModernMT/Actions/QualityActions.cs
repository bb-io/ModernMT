using Apps.ModernMT.Api;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.ModernMT.Models.Quality;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Linq;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Xliff.Utils;
using Blackbird.Xliff.Utils.Models;

namespace Apps.ModernMT.Actions
{
    [ActionList]
    public class QualityActions : BaseInvocable
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        private readonly IFileManagementClient _fileManagementClient;

        public QualityActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(invocationContext)
        {
            _fileManagementClient = fileManagementClient;
        }

        [Action("Estimate Quality", Description = "Allows to evaluate the quality of a translation or a list of translations")]
        public QualityEstimationResponse Estimate([ActionParameter] QualityEstimationRequest input)
        {
            var client = new ModernMtClient(Creds);
            var response = client.Qe(input.SourceLanguage, input.TargetLanguage, input.Sentence, input.Translation);
            return new()
            {
                Score = response.Score
            };
        }

        [Action("Estimate XLIFF Quality", Description = "Evaluate the quality of a translated XLIFF file")]
        public async Task<EstimateXliffResponse> EstimateXliff([ActionParameter] EstimateXliffRequest Input)
        {
            var xliffDocument = await LoadAndParseXliffDocument(Input.File);
            var src = Input.SourceLanguage ?? xliffDocument.SourceLanguage;
            var tgt = Input.TargetLanguage ?? xliffDocument.TargetLanguage;
            var results = new Dictionary<string, double>();

            var file = await _fileManagementClient.DownloadAsync(Input.File);
            string fileContent;
            Encoding encoding;
            using (var inFileStream = new StreamReader(file, true))
            {
                encoding = inFileStream.CurrentEncoding;
                fileContent = inFileStream.ReadToEnd();
            }

            foreach (var transunit in xliffDocument.TranslationUnits)
            {
                var client = new ModernMtClient(Creds);
                var response = client.Qe(src, tgt, transunit.Source, transunit.Target);
                results.Add(transunit.Id, response.Score);

                fileContent = Regex.Replace(fileContent, @"(<trans-unit id=""" + transunit.Id + @""")", @"${1} extradata=""" + response.Score + @"""");

            }

            if (Input.Threshold != null && Input.Condition != null && Input.State != null)
            {
                var filteredTUs = new List<string>();
                switch (Input.Condition)
                {
                    case ">":
                        filteredTUs = results.Where(x => x.Value > Input.Threshold).Select(x => x.Key).ToList();
                        break;
                    case ">=":
                        filteredTUs = results.Where(x => x.Value >= Input.Threshold).Select(x => x.Key).ToList();
                        break;
                    case "=":
                        filteredTUs = results.Where(x => x.Value == Input.Threshold).Select(x => x.Key).ToList();
                        break;
                    case "<":
                        filteredTUs = results.Where(x => x.Value < Input.Threshold).Select(x => x.Key).ToList();
                        break;
                    case "<=":
                        filteredTUs = results.Where(x => x.Value <= Input.Threshold).Select(x => x.Key).ToList();
                        break;
                }

                fileContent = UpdateTargetState(fileContent, Input.State, filteredTUs);
            }


            return new EstimateXliffResponse
            {
                AverageScore = results.Average(x => x.Value),
                File = await _fileManagementClient.UploadAsync(new MemoryStream(encoding.GetBytes(fileContent)), MediaTypeNames.Text.Xml, Input.File.Name)
            };
        }

        private string UpdateTargetState(string fileContent, string state, List<string> filteredTUs)
        {
            var tus = Regex.Matches(fileContent, @"<trans-unit[\s\S]+?</trans-unit>").Select(x => x.Value);
            foreach (var tu in tus.Where(x => filteredTUs.Any(y => y == Regex.Match(x, @"<trans-unit id=""(\d+)""").Groups[1].Value)))
            {
                string transformedTU = Regex.IsMatch(tu, @"<target(.*?)state=""(.*?)""(.*?)>") ?
                    Regex.Replace(tu, @"<target(.*?state="")(.*?)("".*?)>", @"<target${1}" + state + "${3}>")
                    : Regex.Replace(tu, "<target", @"<target state=""" + state + @"""");
                fileContent = Regex.Replace(fileContent, Regex.Escape(tu), transformedTU);
            }
            return fileContent;
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

    }
}
