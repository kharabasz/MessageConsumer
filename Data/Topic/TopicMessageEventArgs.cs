using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class TopicMessageEventArgs
    {
        public string Message { get; private set; }

        public TopicMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
