using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace vCardUnitTests
{
    using vCard;
    [TestClass]
    public class FileManagerUnitTests
    {
        [TestMethod]
        public void ReadFiles()
        {
            int[] p = Misc.SmallPrimes;
            string[] TestFileFolders = {
                @"C:\Users\jpjofresm\Dropbox\Apps\MCBackupPro",
                @"E:\GIT\juanpablo.jofre@bitbucket.org\understandingcs\vCard\Contactos"
            };

            List<(string filepath, (List<vCard> cards, List<(int linenumber, string line)> rejects))> Results = new List<(string filepath, (List<vCard> cards, List<(int linenumber, string line)> rejects))>();

            foreach (string  testfolder in TestFileFolders)
            {
                Results.AddRange(FileManager.ReadFiles(testfolder, recurse: true));

            }
        }
    }
}
