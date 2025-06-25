using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnderstandingReddogDatacenterInfo
{
    public class DeviceInformation
    {
        public string DatacenterLocation { get; set; }
        public string ClusterName { get; set; }
        public string ClusterDatacenter { get; set; }
        public string MachinePoolName { get; set; }
        public string MachinePoolDatacenter { get; set; }

        public override string ToString()
        {
            return string.Join(
                "\t",
                this.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .OrderBy(p => p.Name)
                    .Select(p => p.GetValue(this).ToString()));
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            // this might be a good enough solution for this family of classes, as they are mainly string properties
            foreach (var propertyInfo in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (propertyInfo.GetValue(this).ToString() != propertyInfo.GetValue(obj).ToString())
                {
                    return false;

                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().ToCharArray().Sum(c => Convert.ToInt32(c));
        }
        public static string TsvHeader<T>()
        {
            return string.Join(
                "\t",
                typeof(T)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .OrderBy(p => p.Name)
                    .Select(p => p.Name));
        }
    }

    public class RackInformation: DeviceInformation
    {
        public string RackLocation { get; set; }
        public string RackKey { get; set; }
        public string RackKeyNoUpperLower { get; set; }
    }
    public class BladeInformation: RackInformation
    {
        public string BladeId { get; set; }
        public string BladeLocation { get; set; }
        public string BladeAssetTag { get; set; }
        public string BladeKey { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Internal data
            const string DatacenterIsolationPattern = "^(.+?)(PRO?D|STAGE|INT|AGG|UFC|FCC)";
            const string UpperLowerSuffixPattern = "^(.+?)_([Ll]([Oo][Ww][Ee][Rr])?|[Uu]([Pp][Pp][Ee][Rr])?)$";
            Dictionary<string, string> machinePoolToCluster = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            List<BladeInformation> allBlades = new List<BladeInformation>();
            List<BladeInformation> prdNonFccBlades = new List<BladeInformation>();
            List<BladeInformation> nonPrdNonFccBlades = new List<BladeInformation>();
            List<BladeInformation> prdFccBlades = new List<BladeInformation>();
            List<BladeInformation> nonPrdFccBlades = new List<BladeInformation>();
            List<Tuple<string, string>> clustersWithNoMachinePools = new List<Tuple<string, string>>();
            HashSet<DeviceInformation> clustersInfo = new HashSet<DeviceInformation>();
            HashSet<RackInformation> racksInfo = new HashSet<RackInformation>();

            string datacenterLocation = string.Empty;
            string clusterName = string.Empty;
            string machinePoolDatacenter = string.Empty;
            string clusterDatacenter = string.Empty;
            string simplifiedRackLocation = string.Empty;
            List<string> incompleteDatacenters = new List<string>();


            // Get all datacenter types defined in assembly; this should be one per cs file in datacenterClasses folder
            // --> Dictionary<DatacenterCode, SpecificDatacenterType>
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            var datacenterTypes = currentAssembly.DefinedTypes
                .Where(t => t.Name == "Datacenter")
                .Select(t => t.UnderlyingSystemType)
                .ToDictionary(t => t.FullName.Split('.')[1], t => t);

            // Placeholder for loaded Datacenters
            Dictionary<string, object> genericDc = new Dictionary<string, object>();
            List<string> failedDatacenters = new List<string>();

            // ...on the makes of a generic method using 'utils.LoadDatacenter' 
            MethodInfo loadDatacenter = typeof(utils).GetMethod("LoadDatacenter", BindingFlags.Static| BindingFlags.Public);
            MethodInfo caller;

            #region Process datacenter files
            foreach (string datacenterFilename in Directory.EnumerateFiles("./Data", "*.Datacenter.xml", SearchOption.TopDirectoryOnly))
            {
                string datacenterCode = Path.GetFileName(datacenterFilename).Split('.')[0];
                object[] parameters = new object[] { datacenterFilename };
                caller = loadDatacenter.MakeGenericMethod(datacenterTypes[datacenterCode]);
                dynamic currentDatacenter = caller.Invoke(null, parameters);
                genericDc.Add(datacenterCode, currentDatacenter);

                PropertyInfo[] currentDatacenterProperties =
                    datacenterTypes[datacenterCode].UnderlyingSystemType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                #region Verify all needed information is available
                if (!(currentDatacenterProperties.Any(n => n.Name == "Rack") &&
                    currentDatacenterProperties.Any(n => n.Name == "Cluster") &&
                    currentDatacenterProperties.Any(n => n.Name == "MachinePool")))
                {
                    incompleteDatacenters.Add(datacenterCode);
                    continue;
                }
                #endregion

                datacenterLocation = currentDatacenter.Location.Trim().ToUpperInvariant();
                dynamic racks = currentDatacenter.Rack;
                dynamic clusters = currentDatacenter.Cluster;

                #region build-up MachinePool to Cluster information
                foreach (var cluster in clusters)
                {
                    clusterName = cluster.Name.Trim().ToUpperInvariant();
                    dynamic machinePools = cluster.MachinePool;
                    if(machinePools == null)
                    {
                        clustersWithNoMachinePools.Add(new Tuple<string, string>(datacenterLocation, clusterName));
                        continue;
                    }

                    foreach (var machinePool in machinePools)
                    {
                        string machinePoolName = machinePool.Name.Trim().ToUpperInvariant();
                        machinePoolToCluster[machinePoolName] = clusterName;
                    }
                }
                #endregion

                #region Process Racks and Blades
                foreach (var rack in racks)
                {
                    dynamic blades = rack.Blade;
                    foreach (var blade in blades)
                    {
                        string machinePoolName = blade.MachinePool.Trim().ToUpperInvariant();

                        var bladeInfo = new BladeInformation()
                        {
                            DatacenterLocation = datacenterLocation,
                            ClusterName = string.Empty,
                            MachinePoolName = machinePoolName,
                            RackLocation = rack.Location.Trim().ToUpperInvariant(),
                            BladeId = blade.BladeId.Trim().ToUpperInvariant(),
                            BladeLocation = blade.Location.Trim().ToUpperInvariant(),
                            BladeAssetTag = blade.Asset.Trim().ToUpperInvariant()
                        };

                        allBlades.Add(bladeInfo);
                    }
                }
                #endregion
            }
            #endregion

            #region Complete data information
            foreach (var bladeInfo in allBlades)
            {
                // Update cluster names, once all datacenter files have been processed; to reduce unmatched machine pools
                if (machinePoolToCluster.ContainsKey(bladeInfo.MachinePoolName))
                {
                    bladeInfo.ClusterName = machinePoolToCluster[bladeInfo.MachinePoolName];
                }

                // Find MachinePool datacenter
                machinePoolDatacenter = string.Empty;
                Match machinePoolDatacenterMatch = Regex.Match(bladeInfo.MachinePoolName, DatacenterIsolationPattern);
                if (machinePoolDatacenterMatch.Success)
                {
                    machinePoolDatacenter = machinePoolDatacenterMatch.Groups[1].Value;
                }

                bladeInfo.MachinePoolDatacenter = machinePoolDatacenter;

                // Assign RackKey
                simplifiedRackLocation = bladeInfo.RackLocation;
                if (machinePoolDatacenter != string.Empty)
                {
                    Match simplifiedRackLocationMatch = Regex.Match(simplifiedRackLocation, string.Format("^{0}(.+)$", machinePoolDatacenter));
                    if (simplifiedRackLocationMatch.Success)
                    {
                        simplifiedRackLocation = simplifiedRackLocationMatch.Groups[1].Value;
                    }
                }

                bladeInfo.RackKey = string.Format("{0}|{1}", machinePoolDatacenter, simplifiedRackLocation);

                // Remove _U, _L, _UPPER, _LOWER from simplifiedRackLocation
                bladeInfo.RackKeyNoUpperLower = bladeInfo.RackKey;
                Match upperLowerSuffixMatch = Regex.Match(bladeInfo.RackKey, UpperLowerSuffixPattern);
                if (upperLowerSuffixMatch.Success)
                {
                    bladeInfo.RackKeyNoUpperLower= upperLowerSuffixMatch.Groups[1].Value;
                }

                // Assign BladeKey
                bladeInfo.BladeKey = string.Format("{0}|{1}|{2}", machinePoolDatacenter, simplifiedRackLocation, bladeInfo.BladeLocation);

                // Find Cluster datacenter
                clusterDatacenter = string.Empty;
                Match clusterDatacenterMatch = Regex.Match(bladeInfo.ClusterName, DatacenterIsolationPattern);
                if (clusterDatacenterMatch.Success)
                {
                    clusterDatacenter = clusterDatacenterMatch.Groups[1].Value;
                }

                bladeInfo.ClusterDatacenter = clusterDatacenter;

                // Get cluster and rack info
                clustersInfo.Add(
                    new DeviceInformation()
                    {
                        ClusterDatacenter = bladeInfo.ClusterDatacenter,
                        ClusterName = bladeInfo.ClusterName,
                        DatacenterLocation = bladeInfo.DatacenterLocation,
                        MachinePoolDatacenter = bladeInfo.MachinePoolDatacenter,
                        MachinePoolName = bladeInfo.MachinePoolName
                    });
                racksInfo.Add(
                    new RackInformation()
                    {
                        ClusterDatacenter = bladeInfo.ClusterDatacenter,
                        ClusterName = bladeInfo.ClusterName,
                        DatacenterLocation = bladeInfo.DatacenterLocation,
                        MachinePoolDatacenter = bladeInfo.MachinePoolDatacenter,
                        MachinePoolName = bladeInfo.MachinePoolName,
                        RackKey = bladeInfo.RackKey,
                        RackKeyNoUpperLower = bladeInfo.RackKeyNoUpperLower,
                        RackLocation = bladeInfo.RackLocation
                    });

                // split blade info for analysis purposes
                SplitBladeInfoForReporting(prdNonFccBlades, nonPrdNonFccBlades, prdFccBlades, nonPrdFccBlades, bladeInfo);
            }
            #endregion

            File.WriteAllLines("AllClusterMachinepool.tsv", new[] { DeviceInformation.TsvHeader<DeviceInformation>() }.Concat(clustersInfo.Select(b => b.ToString())));
            File.WriteAllLines("AllRacks.tsv", new[] { RackInformation.TsvHeader<RackInformation>() }.Concat(racksInfo.Select(b => b.ToString())));


            File.WriteAllLines("AllBlades.tsv", new[] { BladeInformation.TsvHeader<BladeInformation>() }.Concat(allBlades.Select(b => b.ToString())));
            File.WriteAllLines("PrdFccBlades.tsv", new[] { BladeInformation.TsvHeader<BladeInformation>() }.Concat(prdFccBlades.Select(b => b.ToString())));
            File.WriteAllLines("PrdBlades.tsv", new[] { BladeInformation.TsvHeader<BladeInformation>() }.Concat(prdNonFccBlades.Select(b => b.ToString())));

            File.WriteAllLines("Not-PrdFccBlades.tsv", new[] { BladeInformation.TsvHeader<BladeInformation>() }.Concat(nonPrdFccBlades.Select(b => b.ToString())));
            File.WriteAllLines("Not-PrdBlades.tsv", new[] { BladeInformation.TsvHeader<BladeInformation>() }.Concat(nonPrdNonFccBlades.Select(b => b.ToString())));

            File.WriteAllLines("ClustersWithNoMachinePools.tsv", clustersWithNoMachinePools.Select(c => string.Format(c.Item1 + "\t" + c.Item2)));
            Console.WriteLine("Total processed datacenters: {0:N0}", genericDc.Count);
            Console.WriteLine(string.Join(Environment.NewLine, genericDc.Keys));
            Console.WriteLine("Total skipped datacenters: {0:N0}", incompleteDatacenters.Count);
            Console.WriteLine(string.Join(Environment.NewLine, incompleteDatacenters));
            Console.WriteLine("Total failed datacenters   : {0:N0}", failedDatacenters.Count);
            Console.WriteLine(string.Join(Environment.NewLine, failedDatacenters));
        }

        private static void SplitBladeInfoForReporting(List<BladeInformation> prdNonFccBlades, List<BladeInformation> nonPrdNonFccBlades, List<BladeInformation> prdFccBlades, List<BladeInformation> nonPrdFccBlades, BladeInformation bladeInfo)
        {
            if (bladeInfo.ClusterName.Contains("PRD"))
            {
                if (bladeInfo.ClusterName.Contains("FCC") || bladeInfo.ClusterName.Contains("UFC"))
                {
                    prdFccBlades.Add(bladeInfo);
                }
                else
                {
                    prdNonFccBlades.Add(bladeInfo);
                }
            }
            else if (bladeInfo.ClusterName != string.Empty)
            {
                if (bladeInfo.ClusterName.Contains("FCC") || bladeInfo.ClusterName.Contains("UFC"))
                {
                    nonPrdFccBlades.Add(bladeInfo);
                }
                else
                {
                    nonPrdNonFccBlades.Add(bladeInfo);
                }
            }
            else if (bladeInfo.MachinePoolName.Contains("PRD"))
            {
                if (bladeInfo.MachinePoolName.Contains("FCC") || bladeInfo.MachinePoolName.Contains("UFC"))
                {
                    prdFccBlades.Add(bladeInfo);
                }
                else
                {
                    prdNonFccBlades.Add(bladeInfo);
                }
            }
            else
            {
                if (bladeInfo.MachinePoolName.Contains("FCC") || bladeInfo.MachinePoolName.Contains("UFC"))
                {
                    nonPrdFccBlades.Add(bladeInfo);
                }
                else
                {
                    nonPrdNonFccBlades.Add(bladeInfo);
                }
            }
        }
    }
    public static class utils
    {
        public static T LoadDatacenter<T>(string pathname)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (!File.Exists(pathname))
            {
                throw new FileNotFoundException(pathname);
            }

            using (StreamReader reader = new StreamReader(pathname))
            {
                T datacenter = (T)serializer.Deserialize(reader);
                return datacenter;
            }
        }

    }
}
