using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventsRetrievalByCert
{
    class Program
    {
        public static IConfigurationRoot Configuration;
        static async Task Main(string[] args)
        {
            (HttpStatusCode, dynamic, List<string>) httpMethodResult;

            HttpStatusCode statuscode;
            dynamic httpcontent;
            List<string> errmsgs;

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"secrets/appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            Configuration = builder.Build();
            httpMethodResult = await HttpGetWithClientCertAsync(
                Configuration["cxp:baseaddress"],
                Configuration["cxp:path"],
                Configuration["cxp:query"],
                Configuration["cxp:clientcertfile"]);
            (statuscode, httpcontent, errmsgs) = httpMethodResult;
            Console.WriteLine($"HTTP status: {statuscode} [{(int)statuscode}]");
            Console.WriteLine($"Response content: {httpcontent.ToString()}");
            Console.WriteLine($"Error messages: ${string.Join(Environment.NewLine, errmsgs)}");


        }

        public static async Task<(HttpStatusCode, dynamic, List<string>)> HttpGetWithClientCertAsync(string baseaddress, string path, string query,  string certLocation)
		{
            HttpStatusCode statuscode = HttpStatusCode.Continue;
            dynamic httpcontent = new JObject();
            List<string> errmsgs = new List<string>();

            X509Certificate2 certificate = new X509Certificate2(certLocation, string.Empty);
			HttpClientHandler handler = new HttpClientHandler();
			handler.ClientCertificateOptions = ClientCertificateOption.Manual;
			handler.ClientCertificates.Add(certificate);

			using (var client = new HttpClient(handler))
			{
                try
                {
                    client.BaseAddress = new Uri(baseaddress);
                    HttpResponseMessage response = await client.GetAsync($"{path}{query}");
                    statuscode = response.StatusCode;
                    string response_data = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(response_data))
                    {
                        if (string.Compare(response.Content.Headers.ContentType.MediaType, "application/json", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            httpcontent = JsonConvert.DeserializeObject(response_data);
                        }
                        else
                        {
                            httpcontent = new JObject();
                            httpcontent.response_data = response_data;
                        }
                    }

                }
                catch (HttpRequestException httpx)
                {
                    statuscode = httpx.StatusCode ?? HttpStatusCode.InternalServerError;

                    Utils.GetErrmsgs(ref errmsgs, (Exception)httpx);
                }
                catch (Exception ex)
                {
                    statuscode = HttpStatusCode.InternalServerError;
                    Utils.GetErrmsgs(ref errmsgs, ex);

                }
            }

			return (statuscode, httpcontent, errmsgs);
		}
	}
}
