using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;


namespace vCard
{
    public class vCard
    {
        private readonly (int linenumber, string foldedline)[] originalcard;
        private List<ContentLine> cleancard = new List<ContentLine>();
        private string sourcefile;
        private readonly string uuid = Guid.NewGuid().ToString();
        private readonly string sha256;

        public string Sha256 { get { return sha256; } }
        public string[] GetOriginalCard { get { return originalcard.Select(l => l.foldedline).ToArray(); } }
        public string SourceFile { get { return sourcefile; } }
        public (int start, int end) LineRange { get; }
        public IReadOnlyList<ContentLine> CardElements { get { return cleancard; } }
        public string UUID { get { return uuid; } }

        public vCard(IList<(int linenumber, string foldedline)> FoldedLines, IList<ContentLine> contentLines, string sourcefile = "")
        {
            StringBuilder hash256sb = new StringBuilder();
            originalcard = FoldedLines.ToArray();
            LineRange = (originalcard[0].linenumber, originalcard[originalcard.Length - 1].linenumber);

            using (SHA256 _shah256 = SHA256.Create())
            {
                foreach (byte b in _shah256.ComputeHash(Encoding.UTF8.GetBytes(this.ToString())))
                {
                    hash256sb.Append(b.ToString("x2"));
                }                
            }

            sha256 = hash256sb.ToString();

            this.sourcefile = sourcefile;

            cleancard.AddRange(contentLines);
        }

        public void AddSourceFile(string sourcefile)
        {
            this.sourcefile = sourcefile;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is vCard other))
                return false;

            if (this.originalcard.Length != other.GetOriginalCard.Length)
                return false;

            if (this.Sha256 != other.Sha256)
                return false;

            for (int i = 0; i < this.originalcard.Length; i++)
            {
                if (this.originalcard[i].foldedline != other.GetOriginalCard[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Sha256.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("\x0A", GetOriginalCard);
        }
    }
}
