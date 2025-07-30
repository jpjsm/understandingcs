// See https://aka.ms/new-console-template for more information
using System.Data;
using Microsoft.Data.SqlClient;

Console.WriteLine("Hello, World!");
string connectionString = "server=localhost;Persist Security Info=False;User ID=sa;Password=<YourStrong@Passw0rd>;";
using (SqlConnection cnx = new SqlConnection(connectionString))
{
    cnx.Open();
    Console.WriteLine(cnx.State);
}