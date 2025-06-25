using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_lang_syntax
{
    public static class Utils
    {
        public static int GetLine(ref char[] s, int lim)
        {
            int c = 0;
            int i = 0;
            while (--lim > 0 && (c = System.Console.Read())!= -1 && ((char)c !='\n'))
                s[i++] = (char)c;
            if ((char)c == '\n')
                s[i++] = (char)c;

            s[i] = '\0';
            return i;                
        }

        public static int StrIndex(char[] s, char[] t)
        {
            int i, j, k;
            for (i = 0; i < s.Length && s[i] != '\0'; i++)
            {
                for(j = i, k = 0; k < t.Length && t[k] != '\0' && s[j] == t[k]; j++, k++)
                {
                    // Do nothing, the comparison code is in the for
                }

                if(k>0 && (k == t.Length || t[k] == '\0'))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
