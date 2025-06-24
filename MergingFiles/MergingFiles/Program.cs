using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergingFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string filesFolderName = @"C:\Data\Reconciliate\Incoming";
            string[] deviceTypes = { "NetworkDevice", "Rack", "Server" };
            string extension = ".tsv";

            foreach (string deviceType in deviceTypes)
            {
                SaveData(
                    filesFolderName,
                    deviceType + ".All" + extension,
                    GetLocalData(filesFolderName, "*" + deviceType + extension));
            }
        }

        private static string[] GetLocalData(string folder, string filenamePattern)
        {
            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException(folder);
            }

            List<string> results = new List<string>();
            foreach (var item in Directory.GetFiles(folder, filenamePattern))
            {
                results.AddRange(File.ReadAllLines(item));
            }

            return results.ToArray();
        }

        private static void SaveData(string folder, string filename, IEnumerable<string> data, bool overwrite = true)
        {
            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException(folder);
            }

            string destinationPath = Path.Combine(folder, filename);

            if (!overwrite && File.Exists(destinationPath))
            {
                throw new IOException("File already exists!!");
            }

            File.WriteAllLines(destinationPath, data);
        }
    }
}
