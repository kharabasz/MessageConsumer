using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class QueueMessageEventArgs
    {
        public string Message { get; private set; }

        public QueueMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
