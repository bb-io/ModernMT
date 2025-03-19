using System.Globalization;
using Apps.ModernMT.Constants;
using Blackbird.Applications.Sdk.Common;
using ModernMT.Model;

namespace Apps.ModernMT.Dtos;

public class MemoryDto(Memory memory)
{
    [Display("ID")]
    public string Id { get; set; } = memory.Id.ToString();
    public string Name { get; set; } = memory.Name;

    public string Description { get; set; } = memory.Description;

    [Display("Created on")]
    public DateTime CreatedOn { get; set; } = DateTime.ParseExact(memory.CreationDate, Formats.MemoryDateFormat, null, DateTimeStyles.None);
}