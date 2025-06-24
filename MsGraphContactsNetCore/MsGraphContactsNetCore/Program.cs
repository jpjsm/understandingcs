using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;

// using Microsoft.Graph.

namespace MsGraphContactsNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build a client application.
            IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder
                        .Create("c0e78865-7b9b-477f-82ff-8f389652db5d")
                        .Build();
            // Create an authentication provider by passing in a client application and graph scopes.
            string[] graphScopes = new string[] {
                "contacts.read",
                "OrgContact.Read.All",
                "Directory.Read.All",
                "user.read",
                "User.ReadWrite",
                "User.ReadBasic.All",
                "User.Read.All",
                "User.ReadWrite.All",
                "Directory.Read.All",
                "Directory.ReadWrite.All",
                "Directory.AccessAsUser.All"
            };

            InteractiveAuthenticationProvider authProvider = new InteractiveAuthenticationProvider(publicClientApplication, graphScopes);

            // Create a new instance of GraphServiceClient with the authentication provider.
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            var userContacts = graphClient.Contacts.Request()
            foreach (var item in graphClient.Contacts.Request().)
            {

            }
            // GET https://graph.microsoft.com/v1.0/me

            //User user = graphClient.Me.Request().GetAsync();

            Console.WriteLine("Hello World!");
        }
    }
}
