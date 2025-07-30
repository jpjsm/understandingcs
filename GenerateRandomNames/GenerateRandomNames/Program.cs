using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRandomNames
{
    class Program
    {
        static string GenerateRandomName()
        {
            var replacements = "ABCDEFGHIJKLMNOPQRSTUV";
            StringBuilder b = new StringBuilder(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            b.Replace("==", "");
            for (int i = 0; i < b.Length; i++)
            {
                if(b[i] == '+' || b[i] == '/')
                {
                    b[i] = replacements[i];
                }
            }

            while(b[0] > 'Z' || b[0] < 'A')
            {
                char c = b[0];
                b = b.Remove(0, 1);
                b.Append(c);
            }

            return b.ToString();
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(GenerateRandomName());
            }
        }
    }
}
