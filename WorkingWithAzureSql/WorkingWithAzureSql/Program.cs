using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithAzureSql
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder cnxBuilder = new SqlConnectionStringBuilder();
            cnxBuilder.DataSource = "enfp520tvg.database.windows.net";
            cnxBuilder.InitialCatalog = "MOCA";
            cnxBuilder.UserID = "moca_readonly";
            cnxBuilder.Password = "asset_me(3)";
            cnxBuilder.IntegratedSecurity = false;

            string cnxString = cnxBuilder.ConnectionString;
            string query =
@"
Select Name
    from [dim_MOCAMSAssetRack]
";
            List<string> uniqueRackNames = new List<string>();
            using (SqlConnection cnx = new SqlConnection(cnxString))
            {
                cnx.Open();
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            uniqueRackNames.Add(reader[0].ToString());
                        }
                    }
                }
            }

            Console.WriteLine(string.Join(Environment.NewLine, uniqueRackNames));
        }
    }
}
