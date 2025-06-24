using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuplesNamed
{
    class Program
    {
        private static readonly (int min, int max, string temperatureFeelingC)[] TemperatureRangesC = new (int min, int max, string temperatureFeeling)[]
        {
           (int.MinValue, -18,"Freezing")
           , (-18, -10, "Bracing")
           , (-10, -1, "Chilly")
           , (-1, 4, "Cool")
           , (4, 10, "Mild")
           , (10, 15, "Warm")
           , (15, 21, "Balmy")
           , (21, 26, "Hot")
           , (26, 30, "Sweltering")
           , (30, 50, "Scorching")
           , (50, int.MaxValue, "Burning")
        };

        private static string GetTempFeels(int tempCentigrades)
        {
            int index = 0;
            while (!( tempCentigrades >= TemperatureRangesC[index].min &&  tempCentigrades < TemperatureRangesC[index].max))
            {
                index++;
            }

            return TemperatureRangesC[index].temperatureFeelingC;
        }
        static void Main(string[] args)
        {
            int[] temperatures = { -20, -18, -15, -10, -5, -1, 0, 4, 6, 10, 12, 15, 18, 21, 24, 26, 28, 30, 37, 50, 55};
            foreach (int t in temperatures)
            {
                Console.WriteLine("A temperature of {0}°C feels like: {1}",t, GetTempFeels(t));
            }
        }
    }
}
