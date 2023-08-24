using Apps.ModernMT.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.ModernMT.Models.Memories.Requests;

public class ImportMemoryRequest
{
    [Display("Memory")]
    [DataSource(typeof(MemoryDataHandler))]
    public string MemoryId { get; set; }

    public File File { get; set; }
}