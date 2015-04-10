using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StuffSender.Utilities
{
    public class Todoist
    {
        public ILog logger = LogManager.GetLogger(typeof(Todoist));


        private int tries = 0;
        public TodoistUploadResponse UploadFile(string path)
        {
            Exception e = null;
            do
            {
                try
                {
                    return PerformUpload(path);
                }
                catch (Exception exc)
                {
                    e = exc;
                    tries++;
                    Thread.Sleep(5000);
                }
            } while (tries < 3);

            throw e;
        }

        private TodoistUploadResponse PerformUpload(string path)
        {
            var address = TodoistUrl.GetUploadAddress();
            logger.Debug("Uploading file [" + path + "] to address [" + address + "]");

            using (WebClient client = new WebClient())
            {
                var response = client.UploadFile(address, path);
                var str = System.Text.Encoding.Default.GetString(response);

                var uploadResponse = JsonConvert.DeserializeObject<TodoistUploadResponse>(str);
                logger.Debug("File uploaded: " + uploadResponse.ToString());
                Thread.Sleep(2 * 1000);
                return uploadResponse;
            }
        }
    }

    class TodoistUrl
    {
        private static string Token = Configuration.UploadToken;
        public static string GetUploadAddress()
        {
            return "https://todoist.com/API/uploadFile?token=" + Token;

        }
    }
}
