﻿//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System.IO;
using System.Configuration;
using Microsoft.ServiceBus;

namespace WorkerRole2WithSBQueue
{
    //******************************************************************************************************
    // This will show you how to perform common scenarios using the Microsoft Azure Service messaging using 
    // the Microsoft Azure WebJobs SDK. The scenarios covered include triggering a function when a new message comes
    // on a queue or topic, and sending a message to another queue or topic.     
    // 
    // In this sample, the Program class starts the JobHost and creates the demo data. The Functions class
    // contains methods that will be invoked when messages are placed on the Service Bus, based on the attributes in 
    // the method headers.
    //
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    //
    // TODO: 1. Create a Storage Account and Service Bus Messaging namespace through the Azure Portal 
    //       2. Open app.config and paste your Storage connection string into the AzureWebJobsDashboard and
    //           AzureWebJobsStorage connection string settings.       
    //       3. Paste your Service Bus connection string into the AzureWebJobsServiceBus connection string setting.
    //*****************************************************************************************************
    class Program
    {
        private static string _servicesBusConnectionString;

        private static NamespaceManager _namespaceManager;

        public static void Main()
        {
            if (!VerifyConfiguration())
            {
                Console.ReadLine();
                return;
            }

            _servicesBusConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsServiceBus"].ConnectionString;
            _namespaceManager = NamespaceManager.CreateFromConnectionString(_servicesBusConnectionString);
            CreateStartMessage();

            JobHostConfiguration config = new JobHostConfiguration()
            {
                ServiceBusConnectionString = _servicesBusConnectionString,
            };

            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }

        private static bool VerifyConfiguration()
        {
            string webJobsDashboard = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
            string webJobsStorage = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;
            string servicesBusConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsServiceBus"].ConnectionString;

            bool configOK = true;
            if (string.IsNullOrWhiteSpace(webJobsDashboard) || string.IsNullOrWhiteSpace(webJobsStorage))
            {
                configOK = false;
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");
            }
            if (string.IsNullOrWhiteSpace(servicesBusConnectionString))
            {
                configOK = false;
                Console.WriteLine("Please add your Service Bus connection string in App.config");
            }
            return configOK;
        }
        private static void CreateStartMessage()
        {
            Console.WriteLine("Creating Demo data");
            Console.WriteLine("Functions will store logs in the 'azure-webjobs-hosts' container in the specified Azure storage account. The functions take in a TextWriter parameter for logging.");

            if (!_namespaceManager.QueueExists(Functions.StartQueueName))
            {
                _namespaceManager.CreateQueue(Functions.StartQueueName);
            }

            QueueClient queueClient = QueueClient.CreateFromConnectionString(_servicesBusConnectionString, Functions.StartQueueName);

            var message = new BrokeredMessage("Hello");
            message.ContentType = "plain/text";
            queueClient.Send(message);

            queueClient.Close();
        }
    }
}
