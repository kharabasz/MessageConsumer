using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IMessageQueueReceiver : IDisposable
    {
        string QueueName { get; }
        IDictionary<string, string> MessageStringProperties { get; }
        event EventHandler<QueueMessageEventArgs> MessageReceived;
        void Start();
        void Stop();
    }
}
