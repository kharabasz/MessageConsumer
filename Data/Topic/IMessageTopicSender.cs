using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IMessageTopicSender
    {
        string QueueName { get; }
        void Send<T>(T msg, string topic) where T : class;
        void Send<T>(T msg, string topic, IDictionary<string, string> messageStringProperties) where T : class;
    }
}
