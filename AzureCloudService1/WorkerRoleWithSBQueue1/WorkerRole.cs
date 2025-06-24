using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WorkerRoleWithSBQueue1
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
        public const string QueueName = "ordersqueue";
        public const string WorkerRoleConnectionString = "Endpoint=sb://jpjofre-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=O78wXLXRdwv/OEEdpywzSFEL9CpvJd4CMQbpn/MThn4=";

        // Azure Storage information where your message information will be placed
        public const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=jpjofredatastorage;AccountKey=5iFDaw0Wo7MwPNs5KRg22QChb5wXwdNdJNYEdS3dBPYfTL0oZWCYxgtjVIsasp36P7g24edOVZrTCDhe4M+Y5A==";
        public const string DefaultContainer = "WorkerRoleWithSBQueue1";
        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);
        AzureStorageOperations storage = new AzureStorageOperations(StorageConnectionString);

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        // Process the message
                        string blobName = string.Format("msg{0:D6}_{1}", receivedMessage.SequenceNumber, receivedMessage.MessageId);
                        StringBuilder blobData = new StringBuilder();
                        foreach (var kvp in receivedMessage.Properties)
                        {
                            blobData.AppendLine(string.Format("{0} = {1}", kvp.Key, kvp.Value));
                        }

                        storage.UploadBlob(DefaultContainer, blobName, blobData.ToString());
                        Trace.WriteLine("Processing Service Bus message: " + blobName);
                    }
                    catch(Exception ex)
                    {
                        // Handle any message processing specific exceptions here
                        Trace.TraceError("[ERROR Processing Service Bus message] " + ex.Message);
                    }

                    Trace.Flush();
                });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            Trace.WriteLine("Starting OnStart():");
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            //string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(WorkerRoleConnectionString);
            //if (!namespaceManager.QueueExists(QueueName))
            //{
            //    throw new ApplicationException("Required Azure Service Bus not available.");
            //}

            // Initialize the connection to Service Bus Queue
            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);
            Client = messagingFactory.CreateQueueClient(
                QueueName);

            //Client = QueueClient.CreateFromConnectionString(WorkerRoleConnectionString);
            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.WriteLine("Starting OnStop():");
            // Close the connection to Service Bus Queue
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
    }
}
