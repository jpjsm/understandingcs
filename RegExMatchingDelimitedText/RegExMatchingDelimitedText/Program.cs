using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegExMatchingDelimitedText
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] testcases =
            {
                "a passport needs a \"2\\\"x3\\\" likeness\" of the holder",
                "Darth Symbol: \"/-|-\\\\\" or \"[^-^]\""
            };

            string quotedstringpattern = "\"(?<text>(\\\\.|[^\\\"])*)\"";
            Console.WriteLine("Quoted string pattern: {0}", quotedstringpattern);
            foreach (string test in testcases)
            {
                var matches = Regex.Matches(test, quotedstringpattern);
                foreach (Match match in matches)
                {

                    Console.WriteLine("TestCase: {0} : {1}", test, match.Groups["text"].Value);
                }
            }
        }
    }
}
