using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class RabbitMQReceiver : IMessageQueueReceiver
    {
        static readonly Semaphore semaphore = new Semaphore(2,2);

        public IConnection Connection { get; private set; }
        public string QueueName { get; private set; }
        public bool IsStarted { get; private set; }
        private EventingBasicConsumer consumer;
        private IModel model;
        public IDictionary<string, string> MessageStringProperties { get; private set; }

        public RabbitMQReceiver(IConnection connection, string queueName, IDictionary<string, string> messageStringProperties)
        {
            Connection = connection;
            QueueName = queueName;
            MessageStringProperties = messageStringProperties;

            consumer = new EventingBasicConsumer();
            IsStarted = false;
        }
        
        public RabbitMQReceiver(IConnection connection, string queueName)
        {
            Connection = connection;
            QueueName = queueName;
            MessageStringProperties = null;

            consumer = new EventingBasicConsumer();
            IsStarted = false;
        }

        public event EventHandler<QueueMessageEventArgs> MessageReceived;

        public void Start()
        {
            if (!IsStarted)
            {
                IsStarted = true;
                model = Connection.CreateModel();
                consumer.Received += consumer_Received;
                IDictionary<string, object> dict = null;
                if (MessageStringProperties != null)
                {
                   dict = MessageStringProperties.ToDictionary(k => k.Key, j => (object)j.Value);
                }

                model.QueueDeclare(QueueName, true, false, false, dict);

                model.BasicQos(0, 1, false);
                //model.QueueBind(QueueName, QueueName, "", dict);
                //var consumer = new QueueingBasicConsumer(channel);
                model.BasicConsume(QueueName, true,consumer);
            }
        }

        void consumer_Received(IBasicConsumer sender, BasicDeliverEventArgs args)
        {
            semaphore.WaitOne();
            Task T = Task.Factory.StartNew(
                () =>
                {
                    var message = Encoding.UTF8.GetString(args.Body);
                    int dots = message.Split('.').Length - 1;
                    //sender.Model.BasicAck(args.DeliveryTag, false);
                    var arg = new QueueMessageEventArgs(message);
                    MessageReceived.Invoke(this, arg);
                    semaphore.Release();
                });
        }

        public void Stop()
        {
            if (IsStarted)
            {
                IsStarted = false; 
                model.Close();
                model.Dispose();
                consumer.Received -= consumer_Received;

            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
