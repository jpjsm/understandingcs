using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; 
using System.Threading.Tasks;

namespace UnderstandingGeneralRegex
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words = new[]
            {
                "Elandoni"
                , "Abri_U"
                , "Totem_L"
                , "Elan_LOWER"
                , "clan_upper"
                , "zelandonia_lowers"
                , "lousadanii_uppers"
                , "tunnel"
                , "zulu"
            };

            string rx = @"^(.+)(_LOWER|_UPPER|_L|_U)$";
            foreach (string word in words)
            {
                var match = Regex.Match(word, rx, RegexOptions.IgnoreCase);

                Console.WriteLine(match.Success ? match.Groups[1].Value : word);

            }

            string[] folderExclusionPatterns = { "release", "(^|[^A-Za-z])test", "incubation", @"\\AIMS\\" };

            words = new[]
            {
                @"\dcmt_blackforest_latest_amd64\Datacenter\FR1\Datacenter.xml | \dcmt_blackforest_n_latest_amd64\Datacenter\FR1\Datacenter.xml",
                @"\dcmt_blackforest_latest_amd64\Datacenter\LG1\Datacenter.xml | \dcmt_blackforest_n_latest_amd64\Datacenter\LG1\Datacenter.xml",
                @"\dcmt_blackforest_n_latest_amd64\AIMS\Storage\AIMS2.0\DataCenter.xml",
                @"\dcmt_federal_latest_amd64\AIMS\Storage\AIMS2.0\DataCenter.xml",
                @"\dcmt_federal_latest_amd64\Datacenter\BN1\Datacenter.xml | \dcmt_federal_n_latest_amd64\Datacenter\BN1\Datacenter.xml",
                @"\dcmt_federal_latest_amd64\Datacenter\DM2\Datacenter.xml | \dcmt_federal_n_latest_amd64\Datacenter\DM2\Datacenter.xml",
                @"\dcmt_federal_n_latest_amd64\AIMS\Storage\AIMS2.0\DataCenter.xml",
                @"\dcmt_fujitsu_latest_amd64\Datacenter\TAT\Datacenter.xml | \dcmt_fujitsu_n_latest_amd64\Datacenter\TAT\Datacenter.xml",
                @"\dcmt_hpica_latest_amd64\Datacenter\CS1\Datacenter.xml | \dcmt_hpica_n_latest_amd64\Datacenter\CS1\Datacenter.xml"
            };

            string[] foldersFilteredForExclusions = null;
            if (folderExclusionPatterns != null && folderExclusionPatterns.Length > 0)
            {
                foldersFilteredForExclusions = words
                    .Where(f => folderExclusionPatterns.All(e => !Regex.Match(f, e, RegexOptions.IgnoreCase).Success))
                    .ToArray();
            }

        }
    }
}
