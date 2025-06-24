using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHDocVw;

namespace LaunchInternetExploerer
{
    class Program
    {
        static void Main(string[] args)
        {
            InternetExplorer ie = new InternetExplorer();
            ie.Visible = true;
            ie.Navigate("http://msdn.microsoft.com/powershell");
            ie.Refresh();
        }
    }
}
