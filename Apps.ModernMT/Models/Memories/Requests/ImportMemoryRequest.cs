using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Models.Memories.Requests
{
    public class ImportMemoryRequest
    {
        public long MemoryId { get; set; }

        public byte[] File { get; set; }
    }
}
