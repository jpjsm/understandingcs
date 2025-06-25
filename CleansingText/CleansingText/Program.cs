using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleansingText
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Hello\x8\tWorld\x1!";
            var cleansedText = new string(text.Select(c => char.IsWhiteSpace(c) ? ' ' : c).Where(c => !char.IsControl(c)).ToArray());
            Console.WriteLine(text);
            Console.WriteLine(cleansedText);
        }
    }
}
