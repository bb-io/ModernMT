namespace Apps.ModernMT.Models.Memories.Requests;

public class UpdateMemoryRequest : MemoryRequest
{
    public string? Name { get; set; }

    public string? Description { get; set; }
}