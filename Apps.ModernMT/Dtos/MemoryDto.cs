using System.Globalization;
using Apps.ModernMT.Constants;
using Blackbird.Applications.Sdk.Common;
using ModernMT.Model;

namespace Apps.ModernMT.Dtos;

public class MemoryDto
{
    [Display("ID")]
    public string Id { get; set; }
    public string Name { get; set; }
        
    [Display("Created on")]
    public DateTime CreatedOn { get; set; }

        
    public MemoryDto(Memory memory)
    {
        Id = memory.Id.ToString();
        Name = memory.Name;
        CreatedOn = DateTime.ParseExact(memory.CreationDate, Formats.MemoryDateFormat, null, DateTimeStyles.None);
    }
}