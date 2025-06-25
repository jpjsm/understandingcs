using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebRequestResponse
{
    class Test4
    {
        public static string[] ExesUrls =
        {
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/5.1/Microsoft.PowerShell.Core/About/about_PowerShell_Ise.exe.md",
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/5.1/Microsoft.PowerShell.Core/About/about_PowerShell.exe.md",
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/5.0/Microsoft.PowerShell.Core/About/about_PowerShell_Ise.exe.md",
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/5.0/Microsoft.PowerShell.Core/About/about_PowerShell.exe.md",
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/4.0/Microsoft.PowerShell.Core/About/about_PowerShell_Ise.exe.md",
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/4.0/Microsoft.PowerShell.Core/About/about_PowerShell.exe.md",
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/3.0/Microsoft.PowerShell.Core/About/about_PowerShell_Ise.exe.md",
            "https://github.com/PowerShell/PowerShell-Docs/blob/staging/reference/3.0/Microsoft.PowerShell.Core/About/about_PowerShell.exe.md"
        };

        public static string PathReport = "PathReport.tsv";
        public const string ReportFormat = "{0,6}\t{1,-120}\t{2}";
        public const string ExceptionReportFormat = "{0,6}\t{1,-120}\t{2}\t{3}\t{4}";

        static void Main(string[] args)
        {
            ConcurrentDictionary<string, UriWebResponse> validatedPaths = new ConcurrentDictionary<string, UriWebResponse>();
            ConcurrentBag<string> failedPaths = new ConcurrentBag<string>();

            File.WriteAllText(PathReport, string.Empty);

            Parallel.ForEach(
                    ExesUrls,
                    (p) =>
                    {
                        validatedPaths.TryAdd(p, new UriWebResponse(new Uri(p)));

                        if (!validatedPaths.ContainsKey(p) || validatedPaths[p].StatusValue == 0)
                        {
                            failedPaths.Add(p);
                        }
                    }
                );

            if (failedPaths.Count > 0)
            {
                Parallel.ForEach(
                    failedPaths,
                    (f) =>
                    {
                        UriWebResponse testedPath = new UriWebResponse(new Uri(f));
                        if (!validatedPaths.TryAdd(f, testedPath))
                        {
                            validatedPaths[f] = testedPath;
                        }
                    });
            }

            List<string> lines = new List<string>();
            lines.Add(string.Format(ReportFormat, "Value", "Url", "Status"));
            foreach (string path in ExesUrls)
            {
                if (validatedPaths.ContainsKey(path))
                {
                    if (validatedPaths[path].StatusValue != 0)
                    {
                        lines.Add(string.Format(ReportFormat, validatedPaths[path].StatusValue, path, validatedPaths[path].StatusCode));
                    }
                    else
                    {
                        string exceptionCodes = string.Join(" | ", validatedPaths[path].ExeptionsReceived.Select(e => e.ToString()));
                        string exceptionMsgs = string.Join(" | ", validatedPaths[path].ExceptionMessages);
                        lines.Add(string.Format(ExceptionReportFormat, validatedPaths[path].StatusValue, path, validatedPaths[path].StatusCode, exceptionCodes, exceptionMsgs));
                    }
                }
                else
                {
                    lines.Add(string.Format(ReportFormat, -1, path, "n/a"));
                }
            }

            File.WriteAllLines(PathReport, lines);

        }
    }
}
