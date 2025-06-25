using System;

namespace RandomNumbersWithDifferentDistributions
{
    public class RandomNormal : Random
    {
        private static readonly Random rnd = new();
        private const double span = 84.0;
        private const double stdDev_size = 12.0;
        private const double stdDev = stdDev_size/span;
        private const double mean = 0.5;
        private const double max_sample = 1.0 - 1.0/(span*span);

        /// *******************************************************************
        /// Sample returns a random number with a distribution similar to the 
        /// normal distribution, with a fixed mean of 0.5 and a stdDev of 
        /// 1/7 ≈ 0.14286.
        /// This stdDev allows to have 3.5 stdDevs on each side of the mean
        /// With a min value of zero and a max value of less than 1.0.
        ///
        /// See: [Random Gaussian Variables](https://stackoverflow.com/a/218600)
        /// The following output shows the binned output for 100,000 random numbers
        /* 
        0----+----1----+----2----+----3----+----4----+----5----+----6----+----7----+----8----+----9----+----0----+----
        0.0000 +
        0.0323 +
        0.0645 +
        0.0968 +**
        0.1290 +***
        0.1613 +*****
        0.1935 +********
        0.2258 +*************
        0.2581 +*********************
        0.2903 +*****************************
        0.3226 +**************************************
        0.3548 +***********************************************
        0.3871 +********************************************************
        0.4194 +*****************************************************************
        0.4516 +**********************************************************************
        0.4839 +***********************************************************************
        0.5161 +**********************************************************************
        0.5484 +****************************************************************
        0.5806 +********************************************************
        0.6129 +************************************************
        0.6452 +**************************************
        0.6774 +*****************************
        0.7097 +********************
        0.7419 +**************
        0.7742 +*********
        0.8065 +*****
        0.8387 +***
        0.8710 +*
        0.9032 +
        0.9355 +
        0.9677 +

        */
        protected override double Sample()
        {
            double u1 = 1.0 - rnd.NextDouble();
            double u2 = 1.0 - rnd.NextDouble();
            double rndStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double s = mean + stdDev * rndStdNormal;
            s = s < 0 ? 0 : s;
            s = s >= max_sample ? max_sample : s;
            return s;
        }

        public override double NextDouble()
        {
            return Sample();
        }

        public override int Next()
        {
            return (int) (Sample() * int.MaxValue);
        }

        public override int Next(int maxValue)
        {
            return (int) (Sample() * maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            int span =(int) ((maxValue - minValue) * Sample());

            return minValue + span;
        }

        public override void NextBytes(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(Next(0,256));
            }
        }
    }
}