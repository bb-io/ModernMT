using Apps.ModernMT.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.ModernMT.Models.Memories.Requests;

public class ImportMemoryRequest
{
    [Display("Memory")]
    [DataSource(typeof(MemoryDataHandler))]
    public string MemoryId { get; set; }

    public byte[] File { get; set; }
}