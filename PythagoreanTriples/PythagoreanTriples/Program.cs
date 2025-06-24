using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PythagoreanTriples
{
    class Program
    {
        public static void FindTriples(
            int upperlimit,
            Dictionary<long, Dictionary<long, long>> triples,
            Dictionary<long, List<(long a, long b)>> hypotenuses
            )
        {
            triples.Clear();
            hypotenuses.Clear();

            for (long a = 1; a < upperlimit; a++)
            {
                for (long b = (a + 1); b < (upperlimit + 1); b++)
                {
                    long a2 = a * a;
                    long b2 = b * b;

                    long c = (int)Math.Sqrt(a2 + b2);
                    long c2 = c * c;

                    if (c2 == (a2 + b2))
                    {
                        bool iscongruent = false;
                        foreach (long A in triples.Keys)
                        {
                            foreach (long B in triples[A].Keys)
                            {
                                if ((a % A) == 0)
                                {
                                    long scale = (a / A);
                                    if (B * scale == b)
                                    {
                                        iscongruent = true;
                                        break;
                                    }
                                }

                                if (iscongruent)
                                {
                                    break;
                                }
                            }
                        }

                        if (!iscongruent)
                        {
                            if (!triples.ContainsKey(a))
                            {
                                triples.Add(a, new Dictionary<long, long>());
                            }

                            if (!triples[a].ContainsKey(b))
                            {
                                triples[a].Add(b, c);
                            }

                            if (!hypotenuses.ContainsKey(c))
                            {
                                hypotenuses.Add(c, new List<(long a, long b)>());
                            }

                            hypotenuses[c].Add((a, b));
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Dictionary<long, Dictionary<long, long>> triples = new Dictionary<long, Dictionary<long, long>>();
            Dictionary<long, List<(long a, long b)>> hypotenuses = new Dictionary<long, List<(long a, long b)>>();

            int maximagesize = (1 << 14) + (1 << 12) + (1 << 11);
            //int maximagesize = 4096;
            Console.WriteLine("Max side: {0:N0}", maximagesize);

            FindTriples(maximagesize, triples, hypotenuses);

            int lines = 0;
            using (StreamWriter t = File.CreateText("PythagoreanTriples.txt"))
            {
                foreach (long a in triples.Keys.OrderBy(k => k))
                {
                    foreach (long b in triples[a].Keys.OrderBy(l => l))
                    {
                        t.WriteLine("{0,10:N0} {1,10:N0} {2,10:N0}", a, b, triples[a][b]);
                        lines++;
                    }
                }

                t.Flush();
                t.Close();
            }

            Console.WriteLine("Total Pythagorean triples: {0:N0}", lines);

            lines = 0;
            using (StreamWriter t = File.CreateText("Hypotenuses.txt"))
            {
                foreach (long h in hypotenuses.Keys.OrderBy(k => k))
                {
                    t.WriteLine(
                        "{0,6:N0}: [{1}]",
                        h,
                        string.Join(", ", hypotenuses[h].Select(p => string.Format("({0:N0}, {1:N0})", p.a, p.b))));
                    lines++;
                }

                t.Flush();
                t.Close();
            }

            Console.WriteLine("Total hypotenuse groups: {0:N0}", lines);

            Bitmap image = new Bitmap(maximagesize, maximagesize);

            foreach (long a in triples.Keys.OrderBy(k => k))
            {
                foreach (long b in triples[a].Keys.OrderBy(l => l))
                {
                    image.SetPixel((int)a, (int)b, Color.Red); 
                }
            }

            for (int i = 0; i < maximagesize; i++)
            {
                image.SetPixel(0, i, Color.Red);
                image.SetPixel(i, 0, Color.Red);
                image.SetPixel(maximagesize - 1, i, Color.Red);
                image.SetPixel(i, maximagesize - 1, Color.Red);
            }

            image.Save("PythagoreanTriples.png");

        }
    }
}
