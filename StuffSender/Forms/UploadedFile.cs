using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Forms
{
    public class UploadedFile
    {
        public string FilePath { get; set; }
        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }

        public override bool Equals(object obj)
        {
            var incoming = obj as UploadedFile;
            if (incoming == null)
                return false;
            if (incoming.FilePath == null)
                return false;
            return incoming.FilePath.Equals(FilePath);
        }
    }
}
