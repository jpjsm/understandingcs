using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Duplicates
{
    public class Duplicates
    {
        private DirectoryInfo sourceInfo { get; set; }
        private string filter { get; set; }
        private List<List<string>> duplicateGroups = new List<List<string>>();

        public string[][] DuplicateGroups
        {
            get
            {
                string[][] results = new string[duplicateGroups.Count][];
                for (int i = 0; i < duplicateGroups.Count; i++)
                {
                    results[i] = duplicateGroups[i].ToArray();
                }

                return results;
            }
        }

        public Duplicates(string sourceFolder)
        {
            if (string.IsNullOrWhiteSpace(sourceFolder))
            {
                throw new ArgumentNullException(sourceFolder);
            }

            if (!Directory.Exists(sourceFolder))
            {
                throw new DirectoryNotFoundException(sourceFolder);
            }

            sourceInfo = new DirectoryInfo(sourceFolder);
            filter = "*.*";

            FindDuplicates();
        }

        private void FindDuplicates()
        {
            var groupBySize = sourceInfo.EnumerateFiles(filter, SearchOption.AllDirectories)
                                    .GroupBy(f => f.Length).ToList();
            var groupsWithCountGreaterThanTwo = groupBySize.Where(g => g.Count() > 1).ToList();

            foreach (IGrouping<long, FileInfo> lengthGroup in sourceInfo.EnumerateFiles(filter, SearchOption.AllDirectories)
                                    .GroupBy(f => f.Length)
                                    .Where(g => g.Count() > 1))
            {
                Dictionary<string, List<string>> duplicates = new Dictionary<string, List<string>>();
                foreach (FileInfo item in lengthGroup)
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        using (Stream filestream = File.OpenRead(item.FullName))
                        {
                            string hash = BitConverter.ToString(md5.ComputeHash(filestream));
                            if (!duplicates.ContainsKey(hash))
                            {
                                duplicates.Add(hash, new List<string>());
                            }

                            duplicates[hash].Add(item.FullName);
                        }
                    }
                }

                duplicateGroups.AddRange(duplicates.Where(kvp => kvp.Value.Count > 1).Select(kvp => kvp.Value));
            }
        }
    }
}
