using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontendWebRole.Controllers
{
    using Models;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.ServiceBus;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Simply redirect to Submit, since Submit will serve as the
            // front page of this application
            return RedirectToAction("Submit");
        }

        // GET: /Home/Submit
        // Controller method for a view you will create for the submission
        // form
        public ActionResult Submit()
        {
            // Get a NamespaceManager which allows you to perform management and
            // diagnostic operations on your Service Bus queues.
            var namespaceManager = QueueConnector.CreateNamespaceManager();

            // Get the queue, and obtain the message count.
            var queue = namespaceManager.GetQueue(QueueConnector.QueueName);
            ViewBag.MessageCount = queue.MessageCount;


            return View();
        }

        // POST: /Home/Submit
        // Controller method for handling submissions from the submission
        // form
        [HttpPost]
        // Attribute to help prevent cross-site scripting attacks and
        // cross-site request forgery  
        [ValidateAntiForgeryToken]
        public ActionResult Submit(OnlineOrder order)
        {
            if (ModelState.IsValid)
            {
                // Create a message from the order
                var message = new BrokeredMessage(order);

                // Submit the order
                QueueConnector.OrdersQueueClient.Send(message);

                return RedirectToAction("Submit");
            }
            else
            {
                return View(order);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Azure Multi-Tier Sample description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Azure Multi-Tier Sample contact page.";

            return View();
        }
    }
}