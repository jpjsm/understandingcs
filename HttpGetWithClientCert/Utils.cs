using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsRetrievalByCert
{
    public static class Utils
    {
        public static void GetErrmsgs(ref List<string> errmsgs, ArgumentException exArgExc)
        {
            errmsgs.Add($"exArgExc: {exArgExc.ParamName}, {exArgExc.Message}");

            GetErrmsgs(ref errmsgs, exArgExc.InnerException);
        }

        public static void GetErrmsgs(ref List<string> errmsgs, SqlException exSql)
        {
            foreach (SqlError sqlError in exSql.Errors)
            {
                errmsgs.Add($"Severity:{sqlError.Class} [{sqlError.Source}.{sqlError.Procedure}: {sqlError.Number}] {sqlError.Message}");
            }

            GetErrmsgs(ref errmsgs, exSql.InnerException);
        }

        public static void GetErrmsgs(ref List<string> errmsgs, Exception ex)
        {
            errmsgs.Add($"HResult: {ex.HResult}: {ex.Message}");

            if (ex.TargetSite != null)
            {
                errmsgs.Add(new string('-', 120));
                errmsgs.Add(ex.TargetSite.Name);
            }

            if (ex.Source != null)
            {
                errmsgs.Add(new string('-', 120));
                errmsgs.Add(ex.Source);
            }

            if (ex.StackTrace != null)
            {
                errmsgs.Add(new string('-', 120));
                errmsgs.Add(ex.StackTrace);
            }

            Exception _ex = ex.InnerException;

            while (_ex != null)
            {
                errmsgs.Add(new string('=', 120));

                errmsgs.Add($"HResult: {_ex.HResult}: {_ex.Message}");

                if (_ex.TargetSite != null)
                {
                    errmsgs.Add(new string('-', 120));
                    errmsgs.Add(_ex.TargetSite.Name);
                }

                if (_ex.Source != null)
                {
                    errmsgs.Add(new string('-', 120));
                    errmsgs.Add(_ex.Source);
                }

                if (_ex.StackTrace != null)
                {
                    errmsgs.Add(new string('-', 120));
                    errmsgs.Add(_ex.StackTrace);
                }

                errmsgs.Add(new string('=', 120));

                _ex = _ex.InnerException;
            }
        }
    }
}
