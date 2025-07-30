using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawImageFile
{
    class Program
    {
        static void Main(string[] args)
        {
            int maximagesize = (1 << 14) + (1 << 12) + (1 << 11);
            Console.WriteLine("Side: {0:N0}",maximagesize);
            Bitmap image = new Bitmap(maximagesize, maximagesize);
            for (int i = 0; i < maximagesize; i++)
            {
                image.SetPixel(i, i, Color.Black);
            }

            image.Save("image.png");
        }
    }
}
