using Blackbird.Applications.Sdk.Common;
using ModernMT.Model;

namespace Apps.ModernMT.Models.Memories.Responses;

public class ImportMemoryResponse
{
    [Display("Import job ID")]
    public string ImportJobId { get; set; }

    [Display("Memory ID")]
    public string MemoryId { get; set; }

    public int Size { get; set; }

    public float Progress { get; set; }
        
    public ImportMemoryResponse(ImportJob result)
    {
        ImportJobId = result.Id;
        MemoryId = result.Memory.ToString();
        Size = result.Size;
        Progress = result.Progress;
    }
}