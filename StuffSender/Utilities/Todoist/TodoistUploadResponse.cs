using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Utilities
{
    public class TodoistUploadResponse
    {
        public string file_type { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
        public int file_size { get; set; }
    }
}
