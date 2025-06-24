using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    using Topography;

    class Program
    {
        static void Main(string[] args)
        {
            double[,] map1 = new double[3, 3];
            map1[0, 0] = 2.0;
            map1[0, 1] = 1.0;
            map1[0, 2] = 0.0;
            map1[1, 0] = 0.0;
            map1[1, 1] = 2.0;
            map1[1, 2] = 1.0;
            map1[2, 0] = 1.0;
            map1[2, 1] = 0.0;
            map1[2, 2] = 2.0;

            AreaMap area1 = new AreaMap(map1);

            foreach (Point divide in area1.ContinentalDivision.OrderByDescending(d => d.Y).ThenBy(d => d.X))
            {
                Console.WriteLine("Area1: [{0,4:N0},{1,4:N0}]", divide.X, divide.Y);
            }

            double[,] map2 = new double[13, 6];

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    map2[i, j] = (36 - (i - 6) * (i - 6)) * Math.Exp(1.0 + j / 20.0);
                }
            }

            AreaMap area2 = new AreaMap(map2);

            foreach (Point divide in area2.ContinentalDivision.OrderByDescending(d => d.Y).ThenBy(d => d.X))
            {
                Console.WriteLine("Area2: [{0,4:N0},{1,4:N0}]", divide.X, divide.Y);
            }
        }
    }
}
