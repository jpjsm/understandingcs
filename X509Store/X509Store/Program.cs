using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace JP_X509Store
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string certname in GetCertificateThumbprints())
            {
                Console.WriteLine(certname);
            }

            X509Certificate2 cert = null;

            foreach (string thumbprint in new string[] { "29E0896A15", "29E0896A15FBA38B755AD6ED68D4DA95E498931C", "9" })
            {
                if (TryGetCertificateByThumbprint(thumbprint, out cert))
                {
                    Console.WriteLine("{0} --> {1}", thumbprint, cert.Subject);
                }
                else
                {
                    Console.WriteLine("{0} Not Found", thumbprint);
                }
            }

        }

        public static bool TryGetCertificateByThumbprint(string thumb, out X509Certificate2 cert)
        {
            cert = null;

            foreach (StoreLocation loc in new StoreLocation[] { StoreLocation.CurrentUser, StoreLocation.LocalMachine })
            {
                X509Store certStore = new X509Store(StoreName.My, loc);
                try
                {
                    certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection certificateCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumb, validOnly: false);
                    if (certificateCollection.Count > 0)
                    {
                        cert = certificateCollection[0];
                        return true;
                    }
                    
                }
                catch (Exception)
                {
                    // do nothing
                }
                finally
                {
                    certStore.Close();
                }
            }

            return false;
        }

        public static List<string> GetCertificateThumbprints(StoreLocation storeLocation=StoreLocation.CurrentUser)
        {
            List<string> results = new List<string>();
            X509Store store = new X509Store(storeLocation);

            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection storecollection = (X509Certificate2Collection)store.Certificates;
                foreach (X509Certificate2 crt in storecollection)
                {
                    results.Add(crt.Thumbprint);
                }
            }
            finally
            {
                store.Close();
            }

            return results;
        }
    }
}
