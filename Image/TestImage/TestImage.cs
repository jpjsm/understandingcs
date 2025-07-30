using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestImage
{
    using Image;

    class Program
    {
        static void Main(string[] args)
        {
            Image img1 = new Image(7, 3);
            UInt64[] d = new UInt64[7 * 3];
            for (char i = 'a'; i <= 'u'; i++)
            {
                d[i - 'a'] = i;
            }

            img1.Update(d);

            img1.Print();

            img1.Rotate90();

            img1.Print();
        }
    }
}
