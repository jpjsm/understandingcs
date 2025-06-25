using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DuplicateFiles
{
    public static class DuplicateFiles
    {
        public static Dictionary<string, List<FileInfo>> GetDuplicates(
            IEnumerable<FileInfo> files,
            string filter,
            bool isRegex,
            HashAlgorithm hash)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }

            return files
                .Where(f => f.Exists &&
                            Regex.IsMatch(f.FullName, isRegex ? filter : filter.Replace("*", ".*")))
                .GroupBy(f => f.Length)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.ToArray())
                .GroupBy(
                    f =>
                        Convert.ToBase64String(
                            hash.ComputeHash(
                                File.ReadAllBytes(f.FullName))))
                .Where(g => g.Count() > 1)
                .ToDictionary(g => g.Key, g => g.ToList());

        }

        public static Dictionary<string, List<FileInfo>> GetDuplicates(
            IEnumerable<FileInfo> files)  
        {
            return GetDuplicates(files, "*", false, MD5.Create());
        }

        public static Dictionary<string, List<FileInfo>> GetDuplicates(
            IEnumerable<FileInfo> files, string filter)
        {
            return GetDuplicates(files, "*", false, MD5.Create());
        }

        public static Dictionary<string, List<FileInfo>> GetDuplicates(
            IEnumerable<FileInfo> files, string filter, bool isRegex)
        {
            return GetDuplicates(files, "*", isRegex, MD5.Create());
        }

        public static Dictionary<string, List<FileInfo>> GetDuplicates(
            IEnumerable<DirectoryInfo> folders,
            string filter = "*",
            bool isRegex = false)
        {
            return GetDuplicates(
                folders.SelectMany(f => f.GetFiles("*", SearchOption.AllDirectories)), filter, isRegex);
        }

        public static Dictionary<string,List<FileInfo>> GetDuplicates(
            IEnumerable<string> paths,
            string filter = "*",
            bool isRegex = false)
        {
            var files = paths
                .Where(p => File.Exists(Path.GetFullPath(p)))
                .Select(p => new FileInfo(p));

            var otherFiles = paths
                .Where(p => Directory.Exists(Path.GetFullPath(p)))
                .SelectMany(p => (new DirectoryInfo(p)).GetFiles("*", SearchOption.AllDirectories))
                .Where(f => Regex.IsMatch(f.FullName, isRegex ? filter : filter.Replace("*", ".*")));

            return GetDuplicates(files.Concat(otherFiles));
        }
    }
}
