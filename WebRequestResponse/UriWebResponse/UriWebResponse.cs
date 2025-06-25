using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebRequestResponse
{
    public class UriWebResponse
    {
        public string ResolvedUrl { get; private set; }
        public Uri UriGiven { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public string HtmlText { get; private set; }
        public int ExceptionsAllowed { get; private set; }
        public WebExceptionStatus[] ExeptionsReceived { get; private set; }
        public string[] ExceptionMessages { get; private set; }
        public int RetriesLeft { get; private set; }

        public class TestedExpected
        {
            public Uri Tested { get; set; }
            public Uri Expected { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public WebExceptionStatus LastExeptionReceived { get; set; }
            public int RetriesLeft { get; set; }
            public bool? TestResult { get; set; }
            public Uri Resolved { get; set; }
        }

        public int StatusValue
        {
            get { return (int)StatusCode; }
        }

        public UriWebResponse()
        {
            UriGiven = new Uri("net.tcp:127.0.0.1");
            ResolvedUrl = UriGiven.AbsolutePath;
            StatusCode = 0;
            HtmlText = null;
        }

        public UriWebResponse(Uri uri, bool getcontent = false, int exceptionsAllowed = 3)
        {
            UriGiven = uri;
            ResolvedUrl = string.Empty;
            StatusCode = 0;
            HtmlText = null;
            ExceptionsAllowed = exceptionsAllowed;
            ExeptionsReceived = new WebExceptionStatus[exceptionsAllowed];
            ExceptionMessages = new string[exceptionsAllowed];
            #region Get web response
            WebResponse response = null;
            WebRequest request = null;
            RetriesLeft = exceptionsAllowed;
            bool ready = false;
            do
            {
                request = WebRequest.Create(UriGiven);
                request.Credentials = null;
                request.UseDefaultCredentials = false;

                try
                {
                    response = request.GetResponse();
                    ready = true;
                }
                catch (WebException wex)
                {
                    switch (wex.Status)
                    {
                        case WebExceptionStatus.Success:
                            response = wex.Response;
                            ready = true;
                            break;

                        default:
                            ExeptionsReceived[ExceptionsAllowed - RetriesLeft] = wex.Status;
                            ExceptionMessages[ExceptionsAllowed - RetriesLeft] = wex.Message;
                            System.Diagnostics.Trace.WriteLine(string.Format("{0} --> {1}", UriGiven.AbsoluteUri, wex.Message));
                            RetriesLeft--;

                            if (wex.Response != null)
                            {
                                response = wex.Response;
                                ready = true;
                                break;
                            }

                            request = null;
                            if (response != null)
                            {
                                response.Close();
                                response.Dispose();
                                response = null;
                            }

                            System.Threading.Thread.Sleep(5000);
                            break;
                    }
                }

            } while (!ready && RetriesLeft > 0);


            if (response != null)
            {
                switch (uri.Scheme.ToLowerInvariant())
                {
                    case "http":
                    case "https":
                        #region HHTP(s)
                        HttpWebResponse httpresponse = response as HttpWebResponse;
                        if (httpresponse == null)
                        {
                            break;
                        }

                        StatusCode = httpresponse.StatusCode;
                        ResolvedUrl = httpresponse.ResponseUri.AbsoluteUri;
                        HtmlText = string.Empty;
                        if (getcontent)
                        {
                            using (StreamReader reader = new StreamReader(httpresponse.GetResponseStream()))
                            {
                                try
                                {
                                    HtmlText = reader.ReadToEnd();
                                }
                                catch (Exception xfoo)
                                {
                                    if (xfoo.Message == "Hello")
                                    {

                                    }

                                    throw;
                                }

                                reader.Close();
                            }
                        }
                        #endregion
                        break;

                    case "file":
                        #region FILE
                        FileWebResponse fileresponse = response as FileWebResponse;
                        if (fileresponse == null)
                        {
                            return;
                        }

                        StatusCode = 0;
                        ResolvedUrl = fileresponse.ResponseUri.AbsolutePath;
                        #endregion
                        break;

                    case "ftp":
                    case "gopher":
                    case "mailto":
                    case "news":
                        throw new NotImplementedException(uri.Scheme.ToLowerInvariant() + " schema not implemented !");

                    default:
                        throw new NotImplementedException(uri.Scheme.ToLowerInvariant() + ": unknown schema. Not implemented !");
                }

                response.Close();
                response.Dispose();
            }



            request = null;
            #endregion
        }

        public static List<TestedExpected> TestUriWebResponse(ref List<TestedExpected> testitems)
        {
            ConcurrentBag<TestedExpected> subjects = new ConcurrentBag<TestedExpected>(testitems);
            ConcurrentBag<TestedExpected> nextRoundOfSubjects;

            do
            {
                nextRoundOfSubjects = new ConcurrentBag<TestedExpected>();
                Parallel.ForEach(
                    subjects,
                    (s) =>
                    {
                        UriWebResponse testedsubject = new UriWebResponse(s.Tested);
                        s.StatusCode = testedsubject.StatusCode;
                        s.LastExeptionReceived = testedsubject.ExeptionsReceived[testedsubject.ExceptionsAllowed - testedsubject.RetriesLeft];
                        if (testedsubject.StatusValue == 0)
                        {
                            s.RetriesLeft--;
                            if (s.RetriesLeft > 0)
                            {
                                nextRoundOfSubjects.Add(s);
                            }
                        }
                        else if (testedsubject.StatusValue >= 200 && testedsubject.StatusValue < 400)
                        {
                            s.TestResult = false;
                            s.Resolved = new Uri(testedsubject.ResolvedUrl);
                            if (string.Compare(  s.Expected.AbsoluteUri, testedsubject.ResolvedUrl, StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                s.TestResult = true;
                            }
                        }
                    });

                subjects = nextRoundOfSubjects;
            } while (subjects.Count > 0);

            return testitems;
        }
    }
}
