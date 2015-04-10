using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Utilities
{
    public class EmailSender
    {
        public static ILog logger = LogManager.GetLogger(typeof(EmailSender));

        public EmailSender(string smtpHost, int port, string from, string user, string password, bool enableSsl)
        {
            _SmtpHost = smtpHost;
            _Port = port;
            _From = from;
            _User = user;
            _Password = password;
            _EnableSsl = enableSsl;
        }

        private string _SmtpHost { get; set; }
        private int _Port { get; set; }
        private string _From { get; set; }
        private string _User { get; set; }
        private string _Password { get; set; }
        private bool _EnableSsl { get; set; }

        public void SendMail(string mailTo, string subject, string body, bool isBodyHtml = false, string filepath = null, string filename = null)
        {
            logger.Debug("Sending mail to [" + mailTo + "] with subject [" + subject + "]");

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(_SmtpHost);

            if (filepath != null)
            {
                logger.Debug("Attaching [" + filepath + "]");
                Attachment attachment = new Attachment(filepath);

                if (filename != null)
                    attachment.Name = filename;

                mail.Attachments.Add(attachment);
            }

            mail.From = new MailAddress(_From);
            mail.To.Add(mailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;

            SmtpServer.Port = _Port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(_User, _Password);
            SmtpServer.EnableSsl = _EnableSsl;

            SmtpServer.Send(mail);
            logger.Debug("Email to [" + mailTo + "] has been sent!");
        }

    }
}
