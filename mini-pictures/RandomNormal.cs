namespace mini_pictures
{
    public class RandomNormal : Random
    {
        private Random rnd = new();
        protected override double Sample()
        {
            double dispersion = 9; // Number of StdDev included in the range
            double stdDevResolution = 16;
            double sigma = dispersion * stdDevResolution;
            double max_sample = 1.0 - 1.0 / Convert.ToDouble(sigma * sigma);
            double mean = 0.5;
            double stdDev = 1 / dispersion;
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
            return (int)(Sample() * int.MaxValue);
        }

        public override int Next(int maxValue)
        {
            return (int)(Sample() * maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            int span = (int)((maxValue - minValue) * Sample());

            return minValue + span;
        }

        public override void NextBytes(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(Next(0, 256));
            }
        }
    }
}
