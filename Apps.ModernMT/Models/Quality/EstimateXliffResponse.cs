
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.ModernMT.Models.Quality
{
    public class EstimateXliffResponse
    {
        public FileReference File { get; set; }

        [Display("Average Score")]
        public double AverageScore { get; set; }
    }
}
