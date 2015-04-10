using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffSender.Exceptions
{
    public interface IUnloggableException
    {
        string Title { get; set; }
    }
}
