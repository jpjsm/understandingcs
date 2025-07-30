using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace vCard
{
    public static class FileManager
    {

        public static (List<vCard> cards, List<(int linenumber, string line)> rejects) ReadStream(StreamReader contentlines, string sourcefile = "")
        {
            if (contentlines == null)
                throw new ArgumentNullException(nameof(contentlines));


            List<vCard> results = new List<vCard>();
            List<(int linenumber, string line)> rejects = new List<(int linenumber, string line)>();

            List<ContentLine> cardContentLines = new List<ContentLine>();
            List<(int linenumber, string line)> cardLines = new List<(int linenumber, string line)>();
            string line;
            int linenumber = 0;
            Match match;
            ContentLine cl = null;
            bool inCard = false;
            bool inFoldableLine = false;
            while ((line = contentlines.ReadLine()) != null)
            {
                linenumber++;

                if (line.StartsWith(@"\n"))
                {
                    line = " " + line.Substring(2);
                }

                if (line.EndsWith("\r"))
                {
                    line = line.Substring(0, line.Length - 1);
                }

                cardLines.Add((linenumber, line));

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                match = Regex.Match(line, RegexPatterns.ContentLinePatternNonStandard, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                if (match.Success)
                {
                    cl = new ContentLine(match);

                    if (!inCard)
                    {
                        if (string.Compare(cl.Name, "BEGIN", StringComparison.InvariantCultureIgnoreCase) != 0)
                        {
                            rejects.Add((linenumber, line));
                            continue;
                        }

                        inCard = true;
                    }
                    else
                    {
                        if (string.Compare(cl.Name, "BEGIN", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            cardLines.RemoveAt(cardLines.Count - 1);
                            rejects.AddRange(cardLines);
                            cardLines.Clear();
                            cardContentLines.Clear();
                            cardLines.Add((linenumber, line));
                            continue;
                        }
                    }

                    cardContentLines.Add(cl);
                    inFoldableLine = !Regex.IsMatch(cl.Name, "BEGIN|VERSION|END", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

                    if (string.Compare(cl.Name, "END", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        results.Add(new vCard(cardLines, cardContentLines, sourcefile));
                        cardLines.Clear();
                        cardContentLines.Clear();
                        inCard = false;
                    }

                    continue;
                }

                match = Regex.Match(line, RegexPatterns.FoldedLinePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                if (match.Success && inFoldableLine)
                {
                    cl.AddFoldedLIne(match.Groups["foldedline"].Value);
                    continue;
                }

                rejects.AddRange(cardLines);
                cardLines.Clear();
                cardContentLines.Clear();
                inCard = false;
            }

            return (results, rejects);
        }

        public static (List<vCard> cards, List<(int linenumber, string line)> rejects) ReadFile(string cardsfilepath)
        {
            if (string.IsNullOrWhiteSpace(cardsfilepath))
                throw new ArgumentNullException(cardsfilepath);

            if (!File.Exists(cardsfilepath))
                throw new FileNotFoundException(cardsfilepath);

            List<vCard> cards = new List<vCard>();
            List<(int linenumber, string line)> rejects = new List<(int linenumber, string line)>();
            

            using (StreamReader cardsreader = new StreamReader(cardsfilepath, detectEncodingFromByteOrderMarks: true))
            {
                (cards, rejects) = ReadStream(cardsreader, cardsfilepath);
            }

            return (cards, rejects);
        }

        public static List<(string filepath, (List<vCard> cards, List<(int linenumber, string line)> rejects))> ReadFiles(string folderpath, string pattern = "*.vcf", bool recurse = false)
        {
            if (!Directory.Exists(folderpath))
                throw new DirectoryNotFoundException(folderpath);

            List<(string filepath, (List<vCard> cards, List<(int linenumber, string line)> rejects))> results = new List<(string filepath, (List<vCard> cards, List<(int linenumber, string line)> rejects))>();

            if (string.IsNullOrWhiteSpace(pattern))
            {
                pattern = "*.vcf";
            }

            var files = Directory.EnumerateFiles(folderpath, pattern, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach (string filepath in files)
            {
                results.Add((filepath, ReadFile(filepath)));
            }


            return results;
        }

    }
}
