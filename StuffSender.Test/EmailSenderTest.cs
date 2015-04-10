using Microsoft.VisualStudio.TestTools.UnitTesting;
using StuffSender.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Test
{
    [TestClass]
    public class EmailSenderTest
    {
         
        [TestMethod]
        public void ICanSendAnEmail()
        {
            var smtpHost = "smtp.gmail.com";
            var port = 587;
            var from = "myuser@gmail.com";
            var user = "myuser";
            var password = "mypass";
            var enableSsl = true;

            var sender = new EmailSender(smtpHost, port, from, user, password, enableSsl);

            var to = "inbox@facilethings.com";
            sender.SendMail(to, "Test", "Test" + DateTime.Now.ToString());

        }
    }
}
