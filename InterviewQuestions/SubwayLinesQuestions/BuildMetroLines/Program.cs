using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubwayLines
{
    public class Link
    {
        public string NodeA { get; set; }
        public string NodeB { get; set; }
        public int DistanceBetweenNodes { get; set; }
    }

    public class Station
    {
        public string Name { get; set; }
        public string Line { get; set; }
        public int DistanceToHub { get; set; }
    }
    class Program
    {
        public const string hubName = "_";

        static void Main(string[] args)
        {

            Dictionary<string, Station> stations = BuildStationLayout(GenerateTestStationLinks());

            Debug.Assert(DistanceBetweenStations(hubName, hubName, stations) == 0);
            Debug.Assert(DistanceBetweenStations("A", hubName, stations) == 1);
            Debug.Assert(DistanceBetweenStations("A", "B", stations) == 2);
            Debug.Assert(DistanceBetweenStations("C", "F", stations) == 30);
            try
            {
                Debug.Assert(DistanceBetweenStations("C", "G", stations) == 3);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message!= "Unknown station(s)")
                {
                    throw;
                }
            }

        }

        static List<Link> GenerateTestStationLinks()
        {
            List<Link> stations = new List<Link>();

            stations.Add(new Link() { NodeA = "A", NodeB = "_", DistanceBetweenNodes = 1 });
            stations.Add(new Link() { NodeA = "A", NodeB = "B", DistanceBetweenNodes = 2 });
            stations.Add(new Link() { NodeA = "C", NodeB = "B", DistanceBetweenNodes = 3 });

            stations.Add(new Link() { NodeA = "_", NodeB = "D", DistanceBetweenNodes = 7 });
            stations.Add(new Link() { NodeA = "D", NodeB = "E", DistanceBetweenNodes = 8 });
            stations.Add(new Link() { NodeA = "F", NodeB = "E", DistanceBetweenNodes = 9 });

            return stations;
        }

        static Dictionary<string, Station> BuildStationLayout(List<Link> links)
        {
            Dictionary<string, Station> stations = new Dictionary<string, Station>();

            Dictionary<string, List<Tuple<string, int>>> nodes = new Dictionary<string, List<Tuple<string, int>>>();
            List<Tuple<string, int>> hubNodes = new List<Tuple<string, int>>();

            // build list of stations associated to HUB
            // build traversing node list
            foreach (var link in links)
            {
                if (link.NodeA == hubName || link.NodeB == hubName)
                {
                    if (link.NodeA == hubName)
                    {
                        hubNodes.Add(new Tuple<string, int>(link.NodeB, link.DistanceBetweenNodes));
                        continue;
                    }

                    hubNodes.Add(new Tuple<string, int>(link.NodeA, link.DistanceBetweenNodes));
                    continue;
                }

                if (nodes.ContainsKey(link.NodeA))
                {
                    nodes[link.NodeA].Add(new Tuple<string, int>(link.NodeB, link.DistanceBetweenNodes));
                }
                else
                {
                    nodes.Add(link.NodeA, new List<Tuple<string, int>>() { new Tuple<string, int>(link.NodeB, link.DistanceBetweenNodes) });
                }

                if (nodes.ContainsKey(link.NodeB))
                {
                    nodes[link.NodeB].Add(new Tuple<string, int>(link.NodeA, link.DistanceBetweenNodes));
                }
                else
                {
                    nodes.Add(link.NodeB, new List<Tuple<string, int>>() { new Tuple<string, int>(link.NodeA, link.DistanceBetweenNodes) });
                }
            }

            int line = 1;
            string lineName;
            stations.Add(hubName, new Station() { Name = hubName, Line = string.Empty, DistanceToHub = 0 });

            foreach (Tuple<string, int> hubNode in hubNodes)
            {
                lineName = line.ToString("Line_00");
                string currentNode = hubName;
                string nextNode = hubNode.Item1;
                int distance = hubNode.Item2;

                do
                {
                    stations.Add(nextNode, new Station() { Name = nextNode, Line = lineName, DistanceToHub = distance });

                    if (nodes[nextNode].Any(t => t.Item1==currentNode))
                    {
                        nodes[nextNode].Remove(nodes[nextNode].First(t => t.Item1 == currentNode));
                    }

                    if (nodes[nextNode].Count == 0)
                    {
                        break;
                    }

                    currentNode = nextNode;
                    nextNode = nodes[currentNode].First().Item1;
                    distance+= nodes[currentNode].First().Item2;

                } while (true);

                line++;
            }

            return stations;
        }

        static int DistanceBetweenStations(string a, string b, Dictionary<string, Station> stations)
        {
            if (!stations.ContainsKey(a) || !stations.ContainsKey(b))
            {
                throw new ArgumentException("Unknown station(s)");
            }

            if (a==hubName)
            {
                return stations[b].DistanceToHub;
            }

            if (b==hubName)
            {
                return stations[a].DistanceToHub;
            }

            if (stations[a].Line == stations[b].Line)
            {
                return Math.Abs(stations[a].DistanceToHub - stations[b].DistanceToHub);
            }

            return stations[a].DistanceToHub + stations[b].DistanceToHub;
        }

        static List<Link> GenerateRandomStationLinks()
        {
            List<Link> links = new List<Link>();
            Random rnd = new Random();
            List<char> stations = new List<char>();
            for (char i = 'A'; i <= 'Z'; i++)
            {
                stations.Add(i);
            }

            List<List<char>> lines = new List<List<char>>();
            int lineLength, stationId;
            while (stations.Count > 0)
            {
                if (stations.Count > 4)
                {
                    lineLength = rnd.Next(stations.Count / 3, stations.Count * 2 / 3);
                }
                else
                {
                    lineLength = stations.Count;
                }

                List<char> newLIne = new List<char>();
                newLIne.Add('_');
                for (int i = 0; i < lineLength; i++)
                {
                    stationId = rnd.Next(stations.Count);
                    newLIne.Add(stations[stationId]);
                    stations.RemoveAt(stationId);
                }

                lines.Add(newLIne);
            }

            string stationsFilename = "Stations.txt";
            if (File.Exists(stationsFilename))
            {
                File.Delete(stationsFilename);
            }

            int distance, a, b;
            foreach (var line in lines)
            {
                for (int i = 0; i < line.Count - 1; i++)
                {
                    distance = rnd.Next(3, 10);

                    // randomly select node order
                    if (rnd.NextDouble() >= 0.5)
                    {
                        a = i;
                        b = i + 1;
                    }
                    else
                    {
                        a = i + 1;
                        b = i;
                    }

                    File.AppendAllText(stationsFilename, string.Format("{0}\t{1}\t{2}{3}", line[a], line[b], distance, Environment.NewLine));
                    links.Add(new Link() { NodeA = line[a].ToString(), NodeB = line[b].ToString(), DistanceBetweenNodes = distance });
                }
            }

            return links;
        }


    }
}
