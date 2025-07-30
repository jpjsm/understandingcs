using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LogRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            LogRecord r = new LogRecord() { 
                type = LogType.Information,
                FunctionName = "Foo",
                FunctionArguments = "[\"Hello\", \"world\"]",
                Message = "It's a nice day out there.",
                DurationMilliSec = 101.0101,
                status = CallStatus.Success
            };

            IDictionary<string, string> properties = r.Properties;
            Console.WriteLine("Listing Properties dictionary");
            foreach (KeyValuePair<string, string> kvp in properties)
            {
                Console.WriteLine($"{kvp.Key,-24}: {kvp.Value}");
            }

            Console.WriteLine(r.ToJson());
        }
    }
}
