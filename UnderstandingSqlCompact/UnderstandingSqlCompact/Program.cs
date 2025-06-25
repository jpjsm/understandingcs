using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingSqlCompact
{
    class Program
    {
        private const string dbFileName = "UnderstandingSqlCompact.sdf";
        static void Main(string[] args)
        {
            if (File.Exists(dbFileName))
            {
                File.Delete(dbFileName);
            }

            string cnxString = "Data Source = " + dbFileName;

            SqlCeEngine engine = new SqlCeEngine(cnxString);
            engine.CreateDatabase();

            using (SqlCeConnection cnx = new SqlCeConnection(cnxString))
            {
                cnx.Open();

                using (SqlCeCommand cmd = cnx.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE TestTbl (col1 INT PRIMARY KEY, col2 NTEXT, col3 MONEY)";
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
