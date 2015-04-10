using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Forms.ProgressEvents
{
    public class SendActions
    {
        public Action<int, SendProgress> ProgressChange { get; set; }

        public Action<SendStart> ProcessStarted { get; set; }
        public Action<SendFinished> ProcessFinished { get; set; }
        public Action<Exception> ProcessError { get; set; }
    }
}
