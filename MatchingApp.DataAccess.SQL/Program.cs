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

    }

    public class SQLBuilder
    {
        string sql;

        public SqlConnectionStringBuilder SQLBuilder1()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "127.0.0.1";
            builder.UserID = "SA";
            builder.Password = "D1t1sEenSqlServertju";
            builder.InitialCatalog = "testDB";
            builder.TrustServerCertificate = false;
            return builder;
        }

        public void SQLEdit(string sql)
        {
            SqlConnectionStringBuilder builder = SQLBuilder1();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                sql = "SELECT id, name, quantity FROM Inventory";

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
    }
}