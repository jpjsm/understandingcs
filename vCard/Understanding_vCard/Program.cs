using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Understanding_vCard
{
    using vCard;

    class Program
    {
        static void Main(string[] args)
        {
            string[] TestFileFolders = {
                @"C:\Users\jpjofresm\Dropbox\Apps\MCBackupPro",
                @"E:\GIT\juanpablo.jofre@bitbucket.org\understandingcs\vCard\Contactos",
                @"C:\Users\jpjofresm\OneDrive\Contactos"
            };

            List<vCard> cards = new List<vCard>();

            List<(string filepath, (List<vCard> cards, List<(int linenumber, string line)> rejects) readitems)> vCardsReadFile = new List<(string filepath, (List<vCard> cards, List<(int linenumber, string line)> rejects))>();

            foreach (string testfolder in TestFileFolders)
            {
                vCardsReadFile.AddRange(FileManager.ReadFiles(testfolder, recurse: true));
            }

            List<(string _filepath, int linenumber, string rejectedtext)> rejectedwithcontent = new List<(string _filepath, int linenumber, string rejectedtext)>();
            foreach (var (filepath, readitems) in vCardsReadFile)
            {
                cards.AddRange(readitems.cards);
                foreach (var r in readitems.rejects)
                {
                    if (!string.IsNullOrWhiteSpace(r.line))
                    {
                        rejectedwithcontent.Add((filepath, r.linenumber, r.line));
                    }
                }
            }

            Console.WriteLine();
            int cardCount = 0;
            Dictionary<string, int> propertySummary = new Dictionary<string, int>();
            Dictionary<string, List<vCard>> duplicateGroupsSHA256 = new Dictionary<string, List<vCard>>();
            foreach (vCard card in cards)
            {
                cardCount++;
                foreach (ContentLine contentline in card.CardElements)
                {
                    if (!propertySummary.ContainsKey(contentline.Name))
                    {
                        propertySummary.Add(contentline.Name, 0);
                    }

                    propertySummary[contentline.Name]++;
                }

                if (!duplicateGroupsSHA256.ContainsKey(card.Sha256))
                {
                    duplicateGroupsSHA256[card.Sha256] = new List<vCard>();
                }

                duplicateGroupsSHA256[card.Sha256].Add(card);
            }

            foreach (string propertyname in propertySummary.Keys.OrderBy(k => k))
            {
                Console.WriteLine("{0,-32:N0} {1,10:N0}", propertyname, propertySummary[propertyname]);
            }

            Console.WriteLine("Total Cards                      {0,10:N0}", cardCount);

            int totalduplicategroupsSHA256 = 0;
            int totalduplicatecardsSHA256 = 0;
            Dictionary<int, int> GroupSizesSummarySHA256 = new Dictionary<int, int>();
            List<vCard> uniqueCards = new List<vCard>();


            foreach (KeyValuePair<string, List<vCard>> kvp in duplicateGroupsSHA256)
            {
                uniqueCards.Add(kvp.Value[0]);
                if (!GroupSizesSummarySHA256.ContainsKey(kvp.Value.Count))
                {
                    GroupSizesSummarySHA256.Add(kvp.Value.Count, 0);
                }

                GroupSizesSummarySHA256[kvp.Value.Count]++;

                if (kvp.Value.Count > 1)
                {
                    totalduplicategroupsSHA256++;
                    totalduplicatecardsSHA256 += (kvp.Value.Count - 1);
                }
            }

            Console.WriteLine("Total duplicate groups SHA256    {0,10:N0}", totalduplicategroupsSHA256);
            Console.WriteLine("Total duplicate cards  SHA256    {0,10:N0}", totalduplicatecardsSHA256);

            Console.WriteLine("Elements duplicated              __Groups__");
            foreach (int gs in GroupSizesSummarySHA256.Keys.OrderByDescending(k => k))
            {
                Console.WriteLine("{0,-32:N0} {1,10:N0}", gs, GroupSizesSummarySHA256[gs]);
            }


            Console.WriteLine("Press any key to finish");
            Console.ReadKey(true);
        }
    }
}
