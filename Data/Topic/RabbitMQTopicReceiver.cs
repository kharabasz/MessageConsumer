using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class RabbitMQTopicReceiver : IMessageTopicReceiver
    {
        public IConnection Connection { get; private set; }
        public string QueueName { get; private set; }
        public string Topic { get; private set; }
        public bool IsStarted { get; private set; }
        private EventingBasicConsumer consumer;
        private IModel model;
        public IDictionary<string, string> MessageStringProperties { get; private set; }

        public RabbitMQTopicReceiver(IConnection connection, string queueName, string topic, IDictionary<string, string> messageStringProperties)
        {
            Connection = connection;
            QueueName = queueName;
            MessageStringProperties = messageStringProperties;
            Topic = topic;
            consumer = new EventingBasicConsumer();
            IsStarted = false;
        }

        public RabbitMQTopicReceiver(IConnection connection, string queueName, string topic)
        {
            Connection = connection;
            QueueName = queueName;
            MessageStringProperties = null;
            Topic = topic;
            consumer = new EventingBasicConsumer();
            IsStarted = false;
        }

        public event EventHandler<TopicMessageEventArgs> MessageReceived;

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
                    MessageStringProperties.ToDictionary(k => k.Key, j => (object)j.Value);
                }

                model.ExchangeDeclare(QueueName, ExchangeType.Topic);
                var queueName = model.QueueDeclare();
                model.QueueBind(queueName, QueueName, Topic, dict);

                model.BasicConsume(queueName, true,consumer);
            }
        }

        void consumer_Received(IBasicConsumer sender, BasicDeliverEventArgs args)
        {
            var message = Encoding.UTF8.GetString(args.Body);
            int dots = message.Split('.').Length - 1;
            //sender.Model.BasicAck(args.DeliveryTag, false);
            var arg = new TopicMessageEventArgs(message);
            MessageReceived.Invoke(this, arg);
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
