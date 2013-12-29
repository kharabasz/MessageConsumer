using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class RabbitMQSender : IMessageQueueSender
    {
        public IConnection Connection { get; private set; }
        public string QueueName { get; private set; }

        public RabbitMQSender(IConnection connection, string queueName)
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

        public void Send<T>(T msg) where T : class
        {
            using (var channel = Connection.CreateModel())
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(msg.GetType());
                channel.QueueDeclare(QueueName, true, false, false, null);
                TextWriter writer = new StringWriter();
                x.Serialize(writer, msg);
                var body = Encoding.UTF8.GetBytes(writer.ToString());
                IBasicProperties basicProperties = channel.CreateBasicProperties();
                basicProperties.DeliveryMode = 2;
                channel.BasicPublish("", QueueName, basicProperties, body);
            }
        }

        public void Send<T>(T msg, IDictionary<string, string> messageStringProperties) where T : class
        {
            using (var channel = Connection.CreateModel())
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(msg.GetType());
                IBasicProperties basicProperties = channel.CreateBasicProperties();
                basicProperties.Headers = messageStringProperties.ToDictionary(k => k.Key, j => (object)j.Value);
                basicProperties.DeliveryMode = 2;

                channel.QueueDeclare(QueueName, true, false, false, null);
                TextWriter writer = new StringWriter();
                x.Serialize(writer, msg);
                var body = Encoding.UTF8.GetBytes(writer.ToString());

                channel.BasicPublish("", QueueName, basicProperties, body);
            }
        }

    }
}
