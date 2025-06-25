using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ParseJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic o2 = JObject.Parse(File.ReadAllText(@"json1.json"));
            var foo = o2["prediction"]["entities"]["$instance"]["outages"][0]["text"];
            Console.WriteLine(foo);
            dynamic bar = o2.prediction.entities.SelectToken("$instance").outages.First.text;
            Console.WriteLine(bar);
        }
    }
}
