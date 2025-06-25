using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDuplicateFiles
{
    using System.Security.Cryptography;
    using System.IO;
    using System.Linq;
    using System.Text;
    using DuplicateFiles;

    [TestClass]
    public class UnitTestDuplicateFiles
    {
        private string tmpfolder;
        private const string sourceFolder = "Source";
        private const string compareFolder = "Compare";

        public UnitTestDuplicateFiles()
        {
            string tmpLocation = Path.GetTempPath();

            do
            {
                tmpfolder = "UnitTestDuplicateFiles_" + DateTime.Now.Ticks.ToString();
            } while (Directory.Exists(Path.Combine(tmpLocation, tmpfolder)));

            tmpfolder = Directory.CreateDirectory(Path.Combine(tmpLocation, tmpfolder)).FullName;
        }

        [TestInitialize]
        public void SetupFilesAndFolders()
        {
            // Create Source and Compare Folders
            string srcfolder = Path.Combine(tmpfolder, sourceFolder);
            string cmpfolder = Path.Combine(tmpfolder, compareFolder);
            Directory.CreateDirectory(srcfolder);
            Directory.CreateDirectory(cmpfolder);

            // Make files
            int len = 64;
            StringBuilder basetext;
            byte[] dat;
            for (int i = 0; i < 5; i++)
            {
                basetext = new StringBuilder(len + 10);
                dat = new byte[len + 3];
                for (int k = 0; k < len; k++)
                {
                    basetext.Append(Char.ConvertFromUtf32((0x20 + k) % 95));
                    dat[k+3] = (byte)(k % 0xFF);
                }

                for (int j = 0; j < 4; j++)
                {
                    string srctxtfilename = Path.Combine(srcfolder, String.Format("src_{0}_{1}.txt", i, j));
                    string srcdatfilename = Path.Combine(srcfolder, String.Format("src_{0}_{1}.dat", i, j));
                    string cmptxtfilename = Path.Combine(cmpfolder, String.Format("cmp_{0}_{1}.txt", i, j));
                    string cmpdatfilename = Path.Combine(cmpfolder, String.Format("cmp_{0}_{1}.dat", i, j));
                    File.WriteAllText(srctxtfilename, String.Format("SRC{0}{1}:{2}", i, j, basetext.ToString()));
                    File.WriteAllText(cmptxtfilename, String.Format("CMP{0}{1}:{2}", i, j, basetext.ToString()));

                    dat[0] = (byte)i;
                    dat[1] = (byte)j;
                    dat[2] = (byte)1;

                    File.WriteAllBytes(srcdatfilename, dat);

                    dat[2] = (byte)2;
                    File.WriteAllBytes(cmpdatfilename, dat);
                }

                len <<= 1;
            }
        }

        [TestCleanup]
        public void CleanupFilesAndFolders()
        {
            Directory.Delete(tmpfolder, recursive: true);

            
        }

        [TestMethod]
        public void TestNoDuplicates()
        {
            var foo = DuplicateFiles.GetDuplicates(new[] { new DirectoryInfo(tmpfolder) });
            //Assert.IsTrue(foo.Count > 0);
        }
    }
}
