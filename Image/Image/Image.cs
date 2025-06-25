using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image
{
    public class Image
    {
        private int width;
        private int height;
        private UInt64[] data;

        public int Width => width;
        public int Height => height;
        public int Length => width * height;

        public UInt64[,] Picture
        {
            get
            {
                UInt64[,] picture = new UInt64[height, width];
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        picture[h, w] = data[width * h + w];
                    }
                }

                return picture;
            }
        }

        public Image(int w, int h)
        {
            if (w < 0 || h < 0)
            {
                throw new ArgumentException("Arguments must be greater than zero.");
            }

            UInt64 maxsize = int.MaxValue;
            if (maxsize < ((UInt64)w * (UInt64)h))
            {
                throw new ApplicationException("Arguments exceed max image size.");
            }

            data = new UInt64[w * h];
            width = w;
            height = h;
        }

        public void Update(UInt64[] d)
        {
            if (d == null || d.Length != (width * height))
            {
                throw new ArgumentException("Invalid argument: either null or size doesn't match.");
            }

            d.CopyTo(data, 0);
        }

        public void Rotate90()
        {
            UInt64[] new_data = new UInt64[Length];
            int new_height = width;
            int new_width = height;
            int h, w, new_i;
            for (int i = 0; i < Length; i++)
            {
                h = Math.DivRem(i, width, out w);
                new_i = new_width * w + new_width - (h + 1);
                new_data[new_i] = data[i];
            }

            new_data.CopyTo(data, 0);
            height = new_height;
            width = new_width;
        }

        public void Rotate90_v2()
        {
            int new_height = width;
            int delta_height = new_height - 1;
            int new_width = height;
            int delta_width = new_width - 1;
            int index = delta_width;
            UInt64 prev = data[0];
            UInt64 next = 0;

            bool width_turn = true;
            while (index != 0)
            {
                next = data[index];
                data[index] = prev;
                prev = next;
                if (width_turn)
                {
                    index =(int)( ((UInt64)delta_height + (UInt64)index) % (UInt64)Length);
                }
                else
                {
                    index = (int)(((UInt64)delta_width + (UInt64)index) % (UInt64)Length);
                }

                width_turn = !width_turn;
            }

            data[index] = prev;
            height = new_height;
            width = new_width;
        }

        public void Print()
        {
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        Console.Write("{0} ", (char)data[width * h + w]);
                    }

                Console.WriteLine();
                }
        }
    }
}
