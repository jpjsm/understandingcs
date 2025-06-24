using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindMetadataInTopics
{
    class Program
    {
        private const string metadataBlockPattern =
            @"(?smi)(?:\s*\r\n)*---\s*\r\n(?:(?<property>[A-Za-z.]+\s*:\s+.+)\r\n)+---\s*\r\n";

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException("No paramters provided");
            }

            Regex metadataRegex = new Regex(metadataBlockPattern);

            foreach (string mdDocumentsFolder in args)
            {
                if (!Directory.Exists(mdDocumentsFolder))
                {
                    Console.WriteLine("Skipping folder '{0}', it does not exist.", mdDocumentsFolder);
                    continue;
                }

                DirectoryInfo currentFolder = new DirectoryInfo(mdDocumentsFolder);

                foreach (FileInfo mdDocument in currentFolder.EnumerateFiles("*.md", SearchOption.AllDirectories))
                {
                    if(mdDocument.Directory.FullName.ToLowerInvariant().Contains("ignore"))
                    {
                        continue;
                    }

                    Console.WriteLine("{0} <-- {1}", mdDocument.Name, mdDocument.Directory);
                    string content = File.ReadAllText(mdDocument.FullName);
                    MatchCollection matches = metadataRegex.Matches(content);

                    foreach (Match match in matches)
                    {
                        Console.WriteLine("Total groups: {0}", match.Groups.Count);

                        foreach (Group group in match.Groups)
                        {
                            Console.WriteLine("   {0}", group.Value);

                            foreach (Capture capture in group.Captures)
                            {
                                Console.WriteLine("   »   {0}", capture.Value);
                            }
                        }

                        Console.WriteLine("   « « «");
                    }

                    Console.WriteLine("-----------------------------------------------------------------------------");
                }
            }
        }
    }
}
