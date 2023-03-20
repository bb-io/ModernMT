using Apps.ModernMT.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Memories.Responses
{
    public class AllMemoriesResponse
    {
        public IEnumerable<MemoryDto> Memories { get; set; }
    }
}
