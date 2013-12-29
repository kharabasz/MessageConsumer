using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class RabbitMQTopicSender : IMessageTopicSender
    {
        public IConnection Connection { get; private set; }
        public string QueueName { get; private set; }

        public RabbitMQTopicSender(IConnection connection, string queueName)
        {
            Connection = connection;
            QueueName = queueName;
            //factory = new ConnectionFactory()
            //{
            //    HostName = "10.4.107.61",
            //    UserName = "test",
            //    Password = "test"
            //};
            // factory.CreateConnection();
        }

        public void Send<T>(T msg, string topic) where T : class
        {
            using (var channel = Connection.CreateModel())
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(msg.GetType());
                channel.ExchangeDeclare(QueueName, ExchangeType.Topic);

                TextWriter writer = new StringWriter();
                x.Serialize(writer, msg);
                var body = Encoding.UTF8.GetBytes(writer.ToString());
                IBasicProperties basicProperties = channel.CreateBasicProperties();

                channel.BasicPublish(QueueName, topic, basicProperties, body);
            }
        }

        public void Send<T>(T msg, string topic, IDictionary<string, string> messageStringProperties) where T : class
        {
            using (var channel = Connection.CreateModel())
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(msg.GetType());
                IBasicProperties basicProperties = channel.CreateBasicProperties();
                basicProperties.Headers = messageStringProperties.ToDictionary(k => k.Key, j => (object)j.Value);

                channel.ExchangeDeclare(QueueName, ExchangeType.Topic);

                TextWriter writer = new StringWriter();
                x.Serialize(writer, msg);
                var body = Encoding.UTF8.GetBytes(writer.ToString());

                channel.BasicPublish(QueueName, topic, basicProperties, body);
            }
        }

    }
}
