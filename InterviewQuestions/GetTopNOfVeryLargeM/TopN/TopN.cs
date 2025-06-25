using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTopNOfVeryLargeM
{
    public class TopN<T> where T : IComparable
    {
        private T[] top;
        private int n;
        private int last = -1;

        public int N
        {
            get { return n; }
        }

        public T[] Top
        {
            get
            {
                return last != -1 ? top.Take(last + 1).ToArray() : null;
            }
        }

        public TopN(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "N values must be greater than zero (0).");
            }

            this.n = n;
            top = new T[n];
        }

        public void Scan(T item)
        {
            if (last > -1 && item.CompareTo(top[last]) > 0)
            {
                int i = last;

                do
                {
                    if (i < n - 1)
                    {
                        top[i + 1] = top[i];
                        if (i == last)
                        {
                            last++;
                        }
                    }

                    i--;
                } while (i >= 0 && item.CompareTo(top[i]) > 0);

                top[i + 1] = item;
            }
            else
            {
                if (last < n - 1)
                {
                    last++;
                    top[last] = item;
                }
            }

            // Working code
            //for (int i = 0; i < n; i++)
            //{
            //    if (top[i] == null)
            //    {
            //        top[i] = item;
            //        return;
            //    }

            //    if (item.CompareTo(top[i]) > 0)
            //    {
            //        for (int j = n - 1; j > i; j--)
            //        {
            //            top[j] = top[j - 1];
            //        }

            //        top[i] = item;

            //        return;
            //    }
            //}
        }
    }
}
