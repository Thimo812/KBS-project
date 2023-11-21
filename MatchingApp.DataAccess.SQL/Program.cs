using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MatchingApp.DataAccess.SQL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(); //Kan miss bet in een class kijk vid voor meer info
                builder.DataSource = "127.0.0.1";
                builder.UserID = "SA";
                builder.Password = "D1t1sEenSqlServertju";
                builder.InitialCatalog = "testDB";
                builder.TrustServerCertificate = false;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    String sql = "SELECT id, name, quantity FROM Inventory";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}