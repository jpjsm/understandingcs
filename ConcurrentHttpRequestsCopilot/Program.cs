namespace ConcurrentHttpRequestsCopilot;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        var urls = new List<string> {
            "http://example.com",
            "http://example.org",
            // Add more URLs here, up to 8
        };

        var tasks = new List<Task<string>>();
        foreach (var url in urls)
        {
            tasks.Add(FetchWebsite(url));
        }

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }

    static async Task<string> FetchWebsite(string url)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
}