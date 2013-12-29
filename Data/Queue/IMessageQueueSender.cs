using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IMessageQueueSender
    {
        string QueueName { get; }
        void Send<T>(T msg) where T : class;
        void Send<T>(T msg, IDictionary<string, string> messageStringProperties) where T : class;
    }
}
