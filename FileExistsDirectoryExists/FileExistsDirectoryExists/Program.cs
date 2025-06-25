using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileExistsDirectoryExists
{
    class Program
    {
        static void Main(string[] args)
        {
            string userTmpFolder = Path.GetTempPath();
            string[] folders = {
                Path.Combine(userTmpFolder, Guid.NewGuid().ToString()),
                Path.Combine(userTmpFolder, Guid.NewGuid().ToString()),
                Path.Combine(userTmpFolder, Guid.NewGuid().ToString())
            };

            List<string> files = new List<string>();
            for (int i = 0; i < folders.Length; i++)
            {
                Directory.CreateDirectory(folders[i]);
                for (int j = 0; j < 4; j++)
                {
                    string file = Path.Combine(folders[i], Guid.NewGuid().ToString() + ".dat");
                    files.Add(file);
                    File.Create(file);
                }
            }

            string format = "{0,-120} {1,6} {2,8}";
            Console.WriteLine(format,"Path", "IsFile", "IsFolder");
            foreach (var item in files.Concat(folders).OrderBy(s => s))
            {
                Console.WriteLine(format, item, File.Exists(item), Directory.Exists(item));
            }
        }
    }
}
