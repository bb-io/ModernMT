using Apps.ModernMT.Api;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.ModernMT.Models.Quality;

namespace Apps.ModernMT.Actions
{
    [ActionList]
    public class QualityActions : BaseInvocable
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public QualityActions(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        [Action("Estimate Quality", Description = "Allows to evaluate the quality of a translation or a list of translations")]
        public QualityEstimationResponse DetectLanguage([ActionParameter] QualityEstimationRequest input)
        {
            var client = new ModernMtClient(Creds);
            var response = client.Qe(input.SourceLanguage, input.TargetLanguage, input.Sentence, input.Translation);
            return new()
            {
                Score = response.Score
            };
        }
    }
}
