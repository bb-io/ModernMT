using Blackbird.Applications.Sdk.Common;

namespace Apps.ModernMT.Models.Memories.Requests;

public class GetImportStatusRequest
{
    [Display("Import ID")]
    public string ImportId { get; set; }
}