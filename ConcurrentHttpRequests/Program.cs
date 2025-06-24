using System;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ConcurrentHttpRequests
{
    class TestContext
    {
        public string? TestName;
        public string? OpaServer;
        public string? GroupId;
        public int RepetitionsRequested;
        public long BatchId;
    }

    class Results
    {
        public TestContext? TestInfo;
        public int RepetitionId;
        public int ThreadNum;
        public DateTime Timestamp;
        public int StatusCode;
        public string? Query;
        public string? OpaResponse;
        public TimeSpan ExecutionTime;
    }
    internal class Program
    {
        // HttpClient lifecycle management best practices:
        // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use

        private static Dictionary<string, HttpClient> opaHttpClients = new() {
            {"standalone", new HttpClient()  { BaseAddress = new Uri("http://10.227.237.78:29007"),} },
            {"sidecar", new HttpClient()  { BaseAddress = new Uri("http://10.227.237.78:29005"),} },
        };

        private static string[] dataloads = {
            // Original data
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-2\"], \"primary_location\": \"us-east-2\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-west-1\"], \"primary_location\": \"us-west-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\", \"us-east-2\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\", \"us-west-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-2\", \"us-west-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\", \"us-east-2\", \"us-west-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            // Generated
            "{ \"input\": { \"no_of_replicas\": 2, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 1, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 4, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 5, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",

            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-2\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-3\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-4\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-5\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",

            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-2\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-3\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-4\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-5\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",

            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"True\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",

            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"false\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",

            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"2 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"3 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"4 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"5 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"alert\"}}",

            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"nothing\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"erase\", \"action_on_violation\":\"alert\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"alert\", \"action_on_violation\":\"alert\"}}",

            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"nothing\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"call 911\"}}",
            "{ \"input\": { \"no_of_replicas\": 3, \"locations\":[\"us-east-1\"], \"primary_location\": \"us-east-1\", \"is_replicas_across_regions\": \"false\", \"is_replication_with_data_sovereignty\" : \"True\", \"retention_period\": \"1 year\", \"action_on_expiry\": \"delete\", \"action_on_violation\":\"party\"}}",
         };

        private static Random rnd = new Random();

        private static ConcurrentBag<Results> testResults = new ConcurrentBag<Results>();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Concurrent HTTP requests!");

            int desiredRepetitions = 100;
            int repetitions = (1 + desiredRepetitions/dataloads.Length) * dataloads.Length; // to make sure dataloads are used evenly
            int[] concurrency_levels = new int[] {1,2,4,8, 16};

            string testName = "Long_test";
            long batchId = DateTime.UtcNow.Ticks/TimeSpan.TicksPerSecond;
            foreach(var opaHttpClient in opaHttpClients)
            {
                Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Starting tests towards '{opaHttpClient.Key}' OPA: {opaHttpClient.Value.BaseAddress}");

                foreach (int concurrency_level in concurrency_levels)
                {
                    Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] sleeping 20 seconds before the test");
                    Thread.Sleep(20000);
                    Task[] tasks = new Task[concurrency_level];

                    for (int i = 0; i < concurrency_level; i++)
                    {
                        TestContext testContext = new TestContext() {
                            TestName = testName,
                            OpaServer = opaHttpClient.Key, 
                            GroupId = $"{concurrency_level}.{i:D2}", 
                            RepetitionsRequested = repetitions,
                            BatchId = batchId,
                        };

                        tasks[i] = Task.Factory.StartNew((object? obj) =>
                            {
                                TestContext? testContext = obj as TestContext;
                                if (testContext == null)
                                    return;

                                for (int j = 0; j < testContext.RepetitionsRequested; j++)
                                {
                                    Results results = new Results() {
                                        TestInfo = testContext,
                                        RepetitionId = j,
                                        ThreadNum = Thread.CurrentThread.ManagedThreadId,
                                        Query = dataloads[j % dataloads.Length]
                                    };

                                    using StringContent load = new(results.Query, Encoding.UTF8, "text/plain");
                                    using HttpRequestMessage request = new(HttpMethod.Post, "/v1/data/persistence/policies");
                                    request.Content = load;

                                    results.Timestamp = DateTime.UtcNow;
                                    DateTime stopwatch = DateTime.Now;
                                    try
                                    {
                                        using HttpResponseMessage response = opaHttpClient.Value.Send(request);

                                        results.StatusCode = (int)response.StatusCode;
                                        results.OpaResponse =  response.Content.ReadAsStringAsync().Result;
                                        results.ExecutionTime = DateTime.Now - stopwatch;
                                    }
                                    catch (Exception ex)
                                    {
                                        results.StatusCode = 0;
                                        results.OpaResponse = ex.Message;
                                        results.ExecutionTime = DateTime.Now - stopwatch;
                                    }

                                    testResults.Add(results);
                                }

                            },
                            testContext
                        );

                        Console.WriteLine($"Started task: {testContext.OpaServer} - {testContext.GroupId} - {testContext.RepetitionsRequested} repetitions requested");
                    }

                    Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Waiting all tasks to complete");
                    Task.WaitAll(tasks);

                    Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Writing results to file");
                    using (StreamWriter outputFile = new StreamWriter($"OPA-{testName}-{batchId}-{opaHttpClient.Key}-concurrency_level_{concurrency_level}.txt", append: false))
                    {
                        outputFile.WriteLine("OpaServer\tGroupId\tRepetitionsRequested\tRepetitionId\tThreadNum\tTimestamp\tStatusCode\tExecutionTime_TotalMilliseconds\tQuery\tOpaResponse");
                        foreach (Results line in testResults)
                            outputFile.WriteLine($"{line.TestInfo!.OpaServer}\t{line.TestInfo!.GroupId}\t{line.TestInfo.RepetitionsRequested}\t{line.RepetitionId}\t{line.ThreadNum}\t{line.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")}\t{line.StatusCode}\t{line.ExecutionTime.TotalMilliseconds.ToString("n3")}\t{line.Query!.Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n")}\t{line.OpaResponse!.Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n")}");
                    }

                    testResults.Clear();

                    Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] sleeping 20 seconds after the test the test");
                    Thread.Sleep(20000);
                }
            }

            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Test completed.");
        }
    }
}