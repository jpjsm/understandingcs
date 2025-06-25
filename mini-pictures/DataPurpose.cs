using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_pictures
{
    internal enum DataPurpose
    {
        Train,
        Test,
        Generic
    }

    internal static class DataPurposeExtensions
    {
        private static HashSet<string> data_purposes = new HashSet<string>(Enum.GetNames(typeof(DataPurpose)).Select(p => p.ToLowerInvariant()));

        public static HashSet<string> DataPurposes { get { return data_purposes; } }

        public static bool IsDataPurpose(string purpose)
        {  
            return data_purposes.Contains(purpose.ToLowerInvariant());
        }

        public static string GetName(this DataPurpose purpose) 
        {
            if (!Enum.IsDefined(typeof(DataPurpose), purpose))
                throw new ArgumentException("Undefined!!");
            return Enum.GetName(typeof(DataPurpose), purpose);
        }
    }
}
