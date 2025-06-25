using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Cache;


namespace Test_HttpUrlExists
{
    class Program
    {
        static string TestHttpUrl(string url, bool passUserCredentials = false)
        {
            string status = "N/A";
            string user = Guid.NewGuid().ToString();
            string pwd = Guid.NewGuid().ToString();
            NetworkCredential anonymous = new NetworkCredential(user, pwd);

            HttpWebRequest httpRequest = WebRequest.CreateHttp(url);
            httpRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            httpRequest.Credentials = passUserCredentials ? CredentialCache.DefaultNetworkCredentials : anonymous;
            httpRequest.KeepAlive = false;
            try
            {
                using (WebResponse webResponse = httpRequest.GetResponse())
                {
                    status = webResponse is HttpWebResponse ? ((HttpWebResponse)webResponse).StatusCode.ToString() : webResponse.ToString();
                }
               
            }
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.ProtocolError)
                {
                    status = (((HttpWebResponse)wex.Response).StatusCode).ToString();
                }
                else
                {
                    status = wex.Status.ToString();
                }
            }
            catch
            {
                throw;
            }

            httpRequest.Abort();
            return status;
        }

        static void Main(string[] args)
        {
            foreach (string url in File.ReadLines(@"C:\tmp\technet-urls.txt"))
            {
                Console.WriteLine("[{0,-24}] {1}", TestHttpUrl(url), url);
            }
        }
    }
}
