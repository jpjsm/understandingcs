using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexMatchesGroupsCaptures
{
    class Program
    {
        static void Main(string[] args)
        {
            string pattern1 = "(an)";
            string test1 = "Danana Banana";

            MatchCollection matches = Regex.Matches(test1, pattern1);

            foreach (Match match in matches)
            {
                Console.WriteLine("Matches: ", match.Groups);
            }

            string pattern2 = @"[^\r\n]";
            string test2 = "abc\rdef\nghi\r\n";

            MatchCollection matches2 = Regex.Matches(test2, pattern2);

            foreach (Match match in matches2)
            {
                Console.WriteLine("Matches: ", match.Groups);
            }

            string pattern1A = "(an)+";

            matches = Regex.Matches(test1, pattern1A);

            foreach (Match match in matches)
            {

                Console.WriteLine("Captures: {0}", match.Captures.Count);
                foreach (Capture capture in match.Captures)
                {
                    Console.WriteLine("   @{1}   {0}", capture.Value, capture.Index);
                }

                Console.WriteLine("Groups: {0}", match.Groups.Count);

                foreach (Group group in match.Groups)
                {
                    Console.WriteLine("   Group captures: {0}", group.Captures.Count);
                    foreach (Capture capture in group.Captures)
                    {
                        Console.WriteLine("      @{1}   {0}", capture.Value, capture.Index);
                    }

                    Console.WriteLine("   @{1}   {0}", group.Value, group.Index);
                }
            }
        }
    }
}
