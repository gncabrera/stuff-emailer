using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Utilities
{
    public class GmailEmailSender : EmailSender
    {
        public GmailEmailSender() :
            base(Configuration.Smtp, 
            Configuration.Port, 
            Configuration.From, 
            Configuration.Username, 
            Configuration.Password, 
            Configuration.EnableSsl)
        {

        }

    }
}
