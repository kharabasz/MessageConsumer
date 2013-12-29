using Data;
using Data.Commands;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MessageConsumer
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var Connection = new ConnectionFactory() { HostName = "192.168.10.103", UserName = "test", Password = "test" }.CreateConnection())
            {
                IMessageQueueReceiver receiver = new RabbitMQReceiver(Connection, "WorkerQueue");
                receiver.MessageReceived += receiver_MessageReceived;
                receiver.Start();

                Console.WriteLine("Listening for messages...");
                Console.ReadKey();
                receiver.Dispose();
            }
        }

        static void receiver_MessageReceived(object sender, QueueMessageEventArgs e)
        {
            Console.WriteLine(" [x] Received # {0} Message {1:MM/dd/yyyy HH:mm:ss}", e.Message, DateTime.Now);
            

            XDocument doc = XDocument.Load(new StringReader(e.Message));
            var name = doc.Root.Name.ToString();
            ICommand cmd = null;

            if(name == "ExtractionCommand")
            {
                cmd = Deserialize<ExtractionCommand>(e.Message);
            }

            if (cmd != null)
            {
                cmd.Execute(e.Message);
            }
            Console.WriteLine(" [x] Done");
        }

        private static T Deserialize<T>(string message) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;

            using (TextReader reader = new StringReader(message))
            {
                result = serializer.Deserialize(reader) as T;

            }

            return result;
        }

    }
}
