using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT.Models.ContextVector.Responses;

public class ContextVectorResponse
{
    [Display("Context vector")]
    public string? ContextVector { get; set; }
}