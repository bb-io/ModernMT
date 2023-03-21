using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Memories.Responses
{
    public class ImportMemoryResponse
    {
        public string ImportJobId { get; set; }

        public long MemoryId { get; set; }

        public int Size { get; set; }

        public float Progress { get; set; }
    }
}
