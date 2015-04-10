using StuffSender.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Forms
{
    public class SendProgress
    {
        public int Progress { get; set; }
        public UploadedFile UploadedFile { get; set; }
        public TodoistUploadResponse Response {get; set;}
        public UploadedFile NextFile { get; set; }
    }
}
