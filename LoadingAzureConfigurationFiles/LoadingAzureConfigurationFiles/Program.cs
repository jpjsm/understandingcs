using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;

namespace LoadingAzureConfigurationFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string keyvaultAddress = CloudConfigurationManager.GetSetting("Microsoft.KeyVault.Url");
        }
    }
}
