using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingSecureString
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] test1 = { 't', 'e', 's', 't' };
            string test2 = "test";

            SecureString s1 = new SecureString();
            for (int i = 0; i < test1.Length; i++)
            {
                s1.AppendChar(test1[i]);
            }

            s1.MakeReadOnly();

            SecureString s2 = new SecureString();
            for (int i = 0; i < test2.Length; i++)
            {
                s2.AppendChar(test2[i]);
            }

            s2.MakeReadOnly();

            if (s1 != s2)
            {
                throw new ApplicationException("Not equal");
            }

        }
    }
}
