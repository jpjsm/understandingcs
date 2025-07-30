using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace WebRequestResponse
{
    class Test2
    {
        public const string RootPath = "https://msdn.microsoft.com/en-us/powershell/reference";

        public static readonly string[] Versions =
        {
            "3.0",
            "4.0",
            "5.0",
            "5.1"
        };

        public static readonly string[] RelativePaths =
        {
            "readme",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Add-Content-for-FileSystem",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Clear-Content-for-FileSystem",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Get-ChildItem-for-FileSystem",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Get-Content-for-FileSystem",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Get-Item-for-FileSystem",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Remove-Item-for-FileSystem",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Set-Content-for-FileSystem",
            "Microsoft.PowerShell.Core/Providers/FileSystem-Provider/Test-Path-for-FileSystem",
            "Microsoft.PowerShell.Security/providers/certificate-provider",
            "Microsoft.PowerShell.Security/providers/get-childitem-for-certificate",
            "Microsoft.PowerShell.Security/providers/move-item-for-certificate",
            "Microsoft.PowerShell.Security/providers/new-item-for-certificate",
            "Microsoft.PowerShell.Security/providers/remove-item-for-certificate",
            "Microsoft.WsMan.Management/providers/wsman-provider",
            "microsoft.wsman.management/providers/wsman-provider"
        };

        public static string PathReport = "PathReport.tsv";

        static void Main(string[] args)
        {
            List<string> pathsToValidate = new List<string>();
            ConcurrentDictionary<string, UriWebResponse> validatedPaths = new ConcurrentDictionary<string, UriWebResponse>();
            File.WriteAllText(PathReport, string.Empty);

            foreach (string version in Versions)
            {
                foreach (string relpath in RelativePaths)
                {
                    string path = Path.Combine(RootPath, version, relpath).Replace("\\", "/").ToLowerInvariant();
                    pathsToValidate.Add(path);
                }
            }

            Parallel.ForEach(
                    pathsToValidate,
                    (p) => 
                    {
                        validatedPaths.TryAdd(p, new UriWebResponse(new Uri(p)));
                    }
                );

            string format = "{0,6}\t{1,-120}\t{2}";
            List<string> lines = new List<string>();
            lines.Add(string.Format(format,"Value","Url","Status"));
            foreach (string path in pathsToValidate)
            {
                if (validatedPaths.ContainsKey(path))
                {
                    lines.Add(string.Format(format, validatedPaths[path].StatusValue, path, validatedPaths[path].StatusCode));
                }
                else
                {
                    lines.Add(string.Format(format,-1, path, "n/a"));
                }
            }

            File.WriteAllLines(PathReport, lines);
        }
    }
}
