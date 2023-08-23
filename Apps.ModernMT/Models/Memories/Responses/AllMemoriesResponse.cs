using Apps.ModernMT.Dtos;

namespace Apps.ModernMT.Models.Memories.Responses;

public class AllMemoriesResponse
{
    public MemoryDto[] Memories { get; set; }
}