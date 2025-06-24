using System.Collections;

namespace SimpleCode
{
    public static class Extensions
    {
        public static void ShuffleList<T>(this List<T> list, int shuffleTimes = 0)
        {
            // If there's no list, or there's only one element
            // do nothing => there's nothing to shuffle
            if (list == null || list.Count < 2)
            {
                return;
            }

            // If no number of shuffles is specified
            // shuffle the list half of the times
            if (shuffleTimes < 1)
            {
                shuffleTimes = list.Count / 2;
            }

            Random rnd = new Random();

            for (int i = 0; i < shuffleTimes; i++)
            {
                int left = rnd.Next(list.Count);
                int right = rnd.Next(list.Count);
                while (right == left)
                {
                    right = rnd.Next(list.Count);
                }

                // this is the simplest way to swap values
                (list[right], list[left]) = (list[left], list[right]);
            }
        }
    }

    internal class Program
    {
        static Random rnd = new Random();

        static Dictionary<char, ConsoleColor> bg =
            new()
            {
                { 'R', ConsoleColor.Red },
                { 'G', ConsoleColor.Green },
                { 'B', ConsoleColor.Blue },
                { 'b', ConsoleColor.Black },
                { 'w', ConsoleColor.White },
            };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Robert!");
            List<char> original = new() { 'R', 'R', 'R', 'R', 'R', 'B', 'G', 'G', 'G' };

            PrintAsMatrix(original, 3, "Original");

            for (int i = 1; i < 10; i++)
            {
                List<char> shuffled = original.ToList();
                shuffled.ShuffleList(i);

                PrintAsMatrix(shuffled, 3, $"Shuffled {i} times");
            }

            PrintBanner("done", 40);
        }

        static void PrintBanner(string title, int width = 80)
        {
            title = title.Trim();
            string banner = "=";
            title += ((title.Length & 0x1) == 0x1) ? " " : "";

            if (title.Length < (width - 2))
            {
                banner = new string('=', ((width - title.Length - 2) >> 1));
            }
            int titleHalfLength = title.Length >> 1;
            Console.WriteLine();
            Console.WriteLine($"{banner} {title} {banner}");
            Console.WriteLine();
        }

        static void PrintAsMatrix(List<char> list, int width, string msg)
        {
            if (width < 1)
            {
                width = (int)Math.Ceiling(Math.Sqrt(list.Count));
            }

            PrintBanner(msg, 40);

            int i = 0;
            int row = 0;
            while (i < list.Count)
            {
                Console.BackgroundColor = bg.GetValueOrDefault(list[i], ConsoleColor.Black);
                Console.Write($"{list[i]}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
                i++;
                if (i / width != row)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine();
                    row++;
                }
            }
        }
    }
}
