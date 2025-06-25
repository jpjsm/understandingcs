using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultidimensionalArrays
{
    class Program
    {
        static void GetArrayInfo(string name, Array a)
        {
            Console.WriteLine("{0}", name);
            Console.WriteLine("   Rank: {0}", a.Rank);
            for (int i = 0; i < a.Rank; i++)
            {
                string dimname = string.Empty;

                switch (i)
                {
                    case 0:
                        dimname = "X";
                        break;
                    case 1:
                        dimname = "Y";
                        break;
                    case 2:
                        dimname = "Z";
                        break;
                    case 3:
                        dimname = "T";
                        break;
                    default:
                        dimname = i.ToString();
                        break;
                }

                Console.WriteLine("   Dimension {0} range [{1},{2}]", dimname, a.GetLowerBound(i),a.GetUpperBound(i));
            }

            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            int[,] twoDim = new int[2, 3];

            int[,,] threeDim = new int[2, 3, 4];

            GetArrayInfo("twoDim", twoDim);
            GetArrayInfo("threeDim", threeDim);

        }
    }
}
