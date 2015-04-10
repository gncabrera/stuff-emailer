using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Exceptions
{
    [Serializable]
    public class ValidationException : Exception, IUnloggableException
    {
        public string Title { get; set; }

        public ValidationException(string message)
            : base(message)
        {
            Title = "Please, check the values";
        }

        public ValidationException(string title, string message)
            : base(message)
        {
            Title = title;
        }
        public ValidationException(string title, string message, Exception inner)
            : base(message, inner)
        {
            Title = title;
        }

    }
}
