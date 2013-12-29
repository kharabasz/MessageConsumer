using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IMessageTopicReceiver : IDisposable
    {
        string QueueName { get; }
        string Topic { get; }
        IDictionary<string, string> MessageStringProperties { get; }
        event EventHandler<TopicMessageEventArgs> MessageReceived;
        void Start();
        void Stop();
    }
}
