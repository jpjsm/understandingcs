using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConcurrentRequestsExampleClient
{
    class Program
    {
        private static HashSet<TaskStatus> terminatedTaskStatuses = new[]
        {
            TaskStatus.RanToCompletion,
            TaskStatus.Canceled,
            TaskStatus.Faulted
        }.ToHashSet<TaskStatus>();

        private static ConcurrentQueue<string> responses = new ConcurrentQueue<string>();

        static void Main(string[] args)
        {
            var randomNumberService = new RandomNumberService(
                url: "https://localhost:12345",
                maxConcurrentRequests: 8,
                timeoutSeconds: 5
            );

            List<Task> runningTasks = new List<Task>();

            for (int i = 0; i < 1500; i++)
            {
                runningTasks.Add(
                    Task.Run(
                        async () =>
                        {
                            responses.Enqueue(
                                $"Requesting random number {await randomNumberService.GetRandomNumber()}"
                            );
                        }
                    )
                );
            }

            bool finished = true;
            do
            {
                finished = runningTasks.Aggregate(
                    true,
                    (result, next) => result && terminatedTaskStatuses.Contains(next.Status)
                );

                var statusCounts = runningTasks
                    .GroupBy(t => t.Status, g => g)
                    .Select(g => $"{g.Key, -20} {g.Count()}");

                Console.WriteLine(new string('-', 80));
                foreach (string statusCount in statusCounts)
                {
                    Console.WriteLine(statusCount);
                }
                Console.WriteLine(new string('-', 80));
                Thread.Sleep(500);
            } while (!finished);

            Console.Write("Press any key to terminate the program.");
            Console.ReadLine();
        }
    }
}
