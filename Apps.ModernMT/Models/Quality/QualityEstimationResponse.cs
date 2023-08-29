using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Quality
{
    public class QualityEstimationResponse
    {
        [Display("Score")]
        public double Score { get; set; }
    }
}
