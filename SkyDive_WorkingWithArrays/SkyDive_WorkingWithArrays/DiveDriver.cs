using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDive_WorkingWithArrays
{
    public class DiveDriver
    {

        public static bool PromptAndWait(string prompt)
        {
            Console.Write(prompt);
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            return (key.KeyChar == 'Y') || (key.KeyChar == 'y');
        }

        static void Main(string[] args)
        {
            string diverName;
            double diverWeight;
            int intendedDives;
            int generatedDives;

            double skydiversurface;
            double skydiverdragcoeff;
            double divelength;
            double stepsize;


            Skydiver diver;

            while (PromptAndWait("Do you want to calculate dives for a new diver (y/[n])?  "))
            {
                Console.Write("Enter diver name or id and press <ENTER>: ");
                diverName = Console.ReadLine();

                Console.Write("Enter diver weight and press <ENTER>: ");
                diverWeight = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter how many dives you anticipate to generate and press <ENTER>: ");
                intendedDives = Convert.ToInt32(Console.ReadLine());

                diver = new Skydiver(diverName, diverWeight, intendedDives);

                generatedDives = 0;
                do
                {
                    Console.Write("Enter dive length and press <ENTER>: ");
                    divelength = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Enter step size for this dive and press <ENTER>: ");
                    stepsize = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Enter diver drag coefficient for this dive and press <ENTER>: ");
                    skydiverdragcoeff = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Enter diver cross sectional area for this dive and press <ENTER>: ");
                    skydiversurface = Convert.ToDouble(Console.ReadLine());

                    diver.AddDive(divelength, stepsize, skydiverdragcoeff, skydiversurface);

                    Console.WriteLine("Sample of generated data.");

                    Skydive currentDive = diver.GetDive(generatedDives);
                    int samplestep = 1;

                    if (currentDive.GetSeriesSize() > 10)
                    {
                        samplestep = currentDive.GetSeriesSize() / 10;
                    }

                    double[] times = currentDive.GetTimeSeries();
                    double[] velocities = currentDive.GetVelocityseries();

                    for (int i = 0; i < 9; i++)
                    {
                        Console.WriteLine("{0,12:F4}\t{1,12:F4}", times[i * samplestep], velocities[i * samplestep]);
                    }

                    Console.WriteLine("{0,12:F4}\t{1,12:F4}", times[currentDive.GetSeriesSize() - 1], velocities[currentDive.GetSeriesSize() - 1]);


                    generatedDives++;
                } while (generatedDives<intendedDives && PromptAndWait("Do you want to generate another dive (y/[n])?  "));

                // ToDo: Generate output file for Excel
            }
        }
    }
}
