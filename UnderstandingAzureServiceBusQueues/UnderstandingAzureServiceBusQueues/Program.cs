using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace UnderstandingAzureServiceBusQueues
{
    public static class QueueConnector
    {
        // Thread-safe. Recommended that you cache rather than recreating it
        // on every request.
        public static QueueClient OrdersQueueClient;

        // Obtain these values from the Management Portal
        //public const string Namespace = "jpjofre-ns";
        //public const string SasName = "RootManageSharedAccessKey";
        public const string SasConnectionString = "Endpoint=sb://jpjofre-ns.servicebus.windows.net/;SharedAccessKeyName=WorkerRole;SharedAccessKey=RM1AOEJFhKDNR5sHLxwlOeos0HHV6rgwbnnWPNDwA48=";

        // The name of your queue
        public const string QueueName = "OrdersQueue";
        //public const string QueueName = "jpjofre-sbqueue";

        public static NamespaceManager CreateNamespaceManager()
        {
            return NamespaceManager.CreateFromConnectionString(SasConnectionString);

            // Create the namespace manager which gives you access to
            // management operations
            //var uri = ServiceBusEnvironment.CreateServiceUri(
            //    "sb", Namespace, String.Empty);
            //var tP = TokenProvider.CreateSharedAccessSignatureTokenProvider(SasName, SasConnectionString);
            //return new NamespaceManager(uri, tP);
        }

        public static void Initialize()
        {
            // Using Http to be friendly with outbound firewalls
            ServiceBusEnvironment.SystemConnectivity.Mode =
                ConnectivityMode.Http;

            // Create the namespace manager which gives you access to
            // management operations
            var namespaceManager = CreateNamespaceManager();

            // Create the queue if it does not exist already
            // var foo = namespaceManager.GetQueues();
            //if (!namespaceManager.QueueExists(QueueName))
            //{
            //    namespaceManager.CreateQueue(QueueName);
            //}

            // Get a client to the queue
            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);
            OrdersQueueClient = messagingFactory.CreateQueueClient(
                QueueName);
        }
    }

    class Program
    {
        public static readonly char[] progress = { '|', '/', '-', '\\' };
        static void Main(string[] args)
        {
            BrokeredMessage msg;
            QueueConnector.Initialize();

            int messagesToSend = 2000;
            long ticks = DateTime.Now.Ticks;
            for (int i = 0; i < messagesToSend; i++)
            {
                msg = new BrokeredMessage();
                msg.Properties.Add("Hello", string.Format("World {0}", ticks + i));
                msg.MessageId = Guid.NewGuid().ToString("N");

                QueueConnector.OrdersQueueClient.Send(msg);
                Console.Write("{0}\r", progress[i%progress.Length]);
            }

            //// Configure the callback options
            //OnMessageOptions options = new OnMessageOptions();
            //options.AutoComplete = false;
            //options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            //QueueConnector.OrdersQueueClient.OnMessage((receivedmsg) =>
            //{
            //    try
            //    {
            //        // Process message from queue
            //        Console.WriteLine("MessageID: " + receivedmsg.MessageId);
            //        Console.WriteLine("Test Property: " + receivedmsg.Properties["Hello"]);

            //        // Remove message from queue
            //        receivedmsg.Complete();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        receivedmsg.Abandon();
            //    }
            //},
            //options);

            //System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Done waiting");
        }
    }
}
