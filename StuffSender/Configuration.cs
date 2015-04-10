using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender
{
    public static class Configuration
    {

        public static string InboxEmail { get { return GetValue<string>("InboxEmail"); } }
        public static string Smtp { get { return GetValue<string>("Smtp"); } }
        public static int Port { get { return GetValue<int>("Port"); } }
        public static string From { get { return GetValue<string>("From"); } }
        public static string Username { get { return GetValue<string>("Username"); } }
        public static string Password { get { return GetValue<string>("Password"); } }
        public static bool EnableSsl { get { return GetValue<bool>("EnableSsl"); } }
        public static string UploadToken { get { return GetValue<string>("UploadToken"); } }

        private static T GetValue<T>(string key, T defaultValue = default(T))
        {

            var value = ConfigurationManager.AppSettings[key];

            if (value != null)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static int MaxNextActions { get { return GetValue<int>("MaxNextActions", 30); } }
    }
}
