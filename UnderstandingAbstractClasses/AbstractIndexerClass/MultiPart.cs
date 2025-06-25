using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractIndexerClass
{
    public abstract class MultiPart
    {
        private readonly Dictionary<string, int> partLookup;
        private readonly string[] parts;

        public int Count
        {
            get { return parts.Length; }
        }

    }
}
