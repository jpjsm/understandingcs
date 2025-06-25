using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDive_WorkingWithArrays
{
    public class Skydive
    {
        public const double G = 9.81;
        public const double AirDensity = 1.14;

        private int arraysize;
        private double[] time;
        private double[] velocity;
        private double[] acceleration;

        private double finaltime;
        private double deltatime;
        private double dragcoefficient;
        private double crossarea;
        private double divermass;

        public Skydive(double tf, double delta, double drag, double area, double mass)
        {
            double t0 = 0;
            finaltime = tf;
            deltatime = delta;
            dragcoefficient = drag;
            crossarea = area;
            divermass = mass;

            arraysize = Convert.ToInt32((finaltime - t0) / delta) + 1;
            time = new double[arraysize];
            velocity = new double[arraysize];
            acceleration = new double[arraysize];

            double currentDragCoefficient = (drag * AirDensity * area) / (2 * mass);

            time[0] = t0;
            acceleration[0] = G;
            velocity[0] = 0;
            for (int i = 1; i < arraysize; i++)
            {
                time[i] = time[i - 1] + delta;
                velocity[i] = velocity[i - 1] + acceleration[i - 1] * delta;
                acceleration[i] = G - currentDragCoefficient * velocity[i] * velocity[i];
            }
        }

        public double[] GetTimeSeries()
        {
            double[] data = new double[arraysize];
            for (int i = 0; i < arraysize; i++)
            {
                data[i] = time[i];
            }

            return data;
        }

        public double[] GetAccelerationSeries()
        {
            double[] data = new double[arraysize];
            for (int i = 0; i < arraysize; i++)
            {
                data[i] = acceleration[i];
            }

            return data;
        }

        public double[] GetVelocityseries()
        {
            double[] data = new double[arraysize];
            for (int i = 0; i < arraysize; i++)
            {
                data[i] = velocity[i];
            }

            return data;
        }

        public int GetSeriesSize()
        {
            return arraysize;
        }

        public double GetDiverMass()
        {
            return divermass;
        }
    }
}
