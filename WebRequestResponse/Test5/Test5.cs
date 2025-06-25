using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebRequestResponse
{
    class Test5
    {
        static void Main(string[] args)
        {
            string tested, expected;
            string[] datalines = File.ReadAllLines("TN_redirects_to_MSDN.tsv");
            List<UriWebResponse.TestedExpected> testsubjects = new List<UriWebResponse.TestedExpected>();
            foreach (string line in datalines)
            {
                string[] items = line.Split('\t');
                tested = items[0];
                expected = items.Length == 2 && !string.IsNullOrWhiteSpace(items[1]) ? items[1] : "http://127.0.0.1";

                testsubjects.Add(new UriWebResponse.TestedExpected()
                {
                    Tested = new Uri(tested),
                    Expected = new Uri(expected),
                    RetriesLeft = 3
                });
            }

            List<UriWebResponse.TestedExpected> validatedsubjects = UriWebResponse.TestUriWebResponse(ref testsubjects);

            List<string> failedlines = new List<string>();
            string failedline;
            failedlines.Add(string.Format("{0}\t{1}\t{2}\t{3}\t{4}", "Expected", "Resolved", "Tested", "StatusCode", "LastExeptionReceived"));
            foreach (UriWebResponse.TestedExpected failedsubject in validatedsubjects.Where(s => (s.TestResult) == null || !s.TestResult.Value))
            {
                failedline = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", failedsubject.Expected, failedsubject.Resolved, failedsubject.Tested, failedsubject.StatusCode, failedsubject.LastExeptionReceived);
                Console.WriteLine(failedline);
                failedlines.Add(failedline);
            }

            File.WriteAllLines("FailedLines.tsv", failedlines);
        }
    }
}
