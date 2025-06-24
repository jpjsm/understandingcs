using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebRequestResponse
{
    class Test1
    {
        public static string[] AssetIds =
        {
            "005a0eae-0d1b-4fd0-a8f8-135487794106",
            "0858eece-ddcb-4525-89d1-4732c5f54c48",
            "0e494537-dfdf-437a-8b27-c21e30aa1f9f",
            "0fe3ff11-a1f7-43b9-8c85-f92d52641395",
            "117316fe-2f82-4fc9-b5dc-794ae9e3f258",
            "119f968e-618e-439c-b76c-cdd17e6df27c",
            "1f46eeb4-49d7-4bec-bb29-395d9b42f54a",
            "277ba77f-f2be-44d7-8f15-23069faf0a4b",
            "27a05dbd-4b69-48a3-8d55-b295f6225f15",
            "2ae0f9bc-105b-4363-8410-7f94a3c12fa3",
            "2e9df935-45e8-44ba-a66a-2de2dd61f3f5",
            "2f08fac8-2872-4a11-930e-af03a8c4a00d",
            "2faf9754-ad9e-4fe4-a937-49cfb75e7816",
            "31d25e13-f05e-48ad-ae8d-9fd097ca18e8",
            "38a9887b-ce1a-4bde-be4e-98012efae204",
            "4b175fd4-37ef-4fe1-8e75-06205d4cea12",
            "4c3d8d36-4f7a-4211-996f-64110e4b2eb7",
            "4d594e54-2c28-4052-b3f8-1c27ea724561",
            "4ed2b1e1-fde4-4425-90a0-87774477fefa",
            "55e2974f-3314-48d2-8b1b-abdea6b303cb",
            "61d40692-5300-4de9-a9b5-bae31815e105",
            "622a7dda-b90f-44d8-9f69-d7b50d40130a",
            "67038d02-6598-49c6-b5bd-77b59d445abe",
            "6c176030-de74-40f3-8f48-7b4d871c3238",
            "6fff9b86-86df-4440-b7b7-8124b22088fc",
            "75cf79bb-4db6-4a67-8c36-3d20754e2190",
            "807a5b29-3f02-4b97-8eed-854869936017",
            "86d8b4af-af2c-4a27-9519-2c9fd420be3d",
            "906b4b41-7da8-4330-9363-e7164e5e6970",
            "a8b56d7e-cebd-4049-9184-62926ef448e2",
            "a90ffe5f-a576-4c4a-b392-822bcec17ffd",
            "af616c24-e122-4098-930e-1e3ea2080ade",
            "b71febbe-2769-4e5d-a754-8373ab1a848e",
            "b8b08aac-9069-43cb-a224-1cf8a663aad4",
            "c555334d-3000-4fc4-a076-1486c3ed27ec",
            "c62a363a-caa2-4b6e-a079-d6e8543bc4c6",
            "d7f353cd-ebd7-462a-bd57-1498dc8b88a6",
            "de1b4217-de99-45cd-a12c-35e87b0c8466",
            "dee5f65f-eae2-42de-b369-5bed1a38ac21",
            "e8b394a3-e87c-42e5-ad9e-5a1576da6701",
            "ea3f16e3-e8dc-48e7-9ae7-96808b8a941e",
            "f4c69467-d5ce-4da2-87d5-d2ae74be8acb",
            "f98b4219-60df-408b-bdc8-994f920fc7bd",
            "fcff151c-88d1-4b84-a9a9-8e3b1a155413"
        };

        public static string AssetIdFound = "AssetIdFound.tsv";
        public static string AssetIdNotHttp = "AssetIdNotHttp.tsv";
        public static string AssetIdStatusNotOK = "AssetIdStatusNotOK.tsv";

        static void Main(string[] args)
        {
            File.WriteAllText(AssetIdFound,string.Empty);
            File.WriteAllText(AssetIdNotHttp, string.Empty);
            File.WriteAllText(AssetIdStatusNotOK, string.Empty);


            foreach (string assetid in AssetIds)
            {
                string line = string.Empty;
                WebRequest request = WebRequest.Create("https://technet.microsoft.com/en-us/library/" + assetid);

                WebResponse response = request.GetResponse();
                HttpWebResponse httpresponse = response as HttpWebResponse;
                if (httpresponse == null)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    line = assetid + "\tNo HTTP response";
                    Console.WriteLine(line);

                    File.AppendAllLines(AssetIdNotHttp, new[] { assetid });
                    continue;
                }

                if (httpresponse.StatusCode != HttpStatusCode.OK)
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    line = assetid + "\t" + httpresponse.StatusCode.ToString();
                    Console.WriteLine(line);

                    File.AppendAllLines(AssetIdStatusNotOK, new[] { line });
                    continue;
                }

                string htmltext = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    htmltext = reader.ReadToEnd();
                    reader.Close();

                    Match match = Regex.Match(htmltext, "<title>([-_ A-Z0-9]+)</title>", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Green;
                        line = assetid + "\t" + match.Groups[1].Value;
                        Console.WriteLine(line);
                    }
                    else
                    {
                        Console.WriteLine(htmltext);
                    }


                    File.AppendAllLines(AssetIdFound, new[] { line });
                }
            }
        }
    }
}
