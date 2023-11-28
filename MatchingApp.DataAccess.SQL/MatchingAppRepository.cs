using KBS_project;
using KBS_project.Enums;
using KBS_project.Enums.FilterOptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Common;
using static System.Net.Mime.MediaTypeNames;
namespace MatchingApp.DataAccess.SQL
{
	public class MatchingAppRepository
	{
		private SqlConnectionStringBuilder builder;
		public MatchingAppRepository()
		{
			builder = new SqlConnectionStringBuilder();
			builder.DataSource = "127.0.0.1";
			builder.UserID = "SA";
			builder.Password = "D1t1sEenSqlServertju";
			builder.InitialCatalog = "MatchingDB";
			builder.TrustServerCertificate = false;
		}

		public string AgetoDate(int age)
		{
            var today = DateTime.Today;
            var byear = today.Year - age;
			DateTime date = new DateTime(byear, today.Month, today.Day);
            var datestring = date.ToString("yyyy-MM-dd");
            return datestring;
        }

        public bool IsInDatabase(string userName)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"SELECT ProfielGebruikersnaam FROM FotoAlbum WHERE Gebruikersnaam = {userName}";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.GetString != null)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                connection.Close();
            }
            return false;
        }
		public Profile GetProfile(string userName)
		{
			using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
			{
				var sql = $"SELECT * FROM Profile WHERE Gebruikersnaam = {userName}";
				connection.Open();
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						Console.WriteLine(reader.GetString);
					}
				}
				connection.Close();
			}
			return null;
		}

		public List<string> GetProfiles()
		{
            List<string> results = new();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"SELECT DISTINCT Profiel.Gebruikersnaam FROM Profiel";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return results;
        }

        public List<string> GetProfiles(LocationFilter location, int minimumAge, int maximumAge,
            List<Interest> includedHobbys, List<Interest> excludedHobbys, List<Diet> includedDiets, List<Diet> excludedDiets)
        {
            List<string> results = new();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"SELECT DISTINCT Profiel.Gebruikersnaam FROM Profiel LEFT JOIN Hobbies ON Profiel.Gebruikersnaam=Hobbies.ProfielGebruikersnaam WHERE 1 = 1 ";
                if (location != LocationFilter.Global)
                {
                    if (location == LocationFilter.City) { sql += $"AND Woonplaats = 'Mountaintop' AND Land = 'Nederland' "; } //Moet uiteindelijk van het profiel komen boop
                    if (location == LocationFilter.Country) { sql += $"AND Land = 'Nederland' "; }

                }
                if (minimumAge != 0)
                {
                    sql += $"AND Geboortedatum <= '{AgetoDate(minimumAge)}' ";
                }
                if (maximumAge != 0)
                {
                    sql += $"AND Geboortedatum >= '{AgetoDate(maximumAge)}' ";
                }
                if (includedHobbys != null)
                {
                    foreach (var inclhobby in includedHobbys)
                    {
                        sql += $"AND Hobby = '{inclhobby}' ";
                    }
                }
                if (excludedHobbys != null)
                {
                    foreach (var exlhobby in excludedHobbys)
                    {
                        sql += $"AND NOT Hobby = '{exlhobby}' ";
                    }
                }
                if (includedDiets != null)
                {
                    foreach (var incldiet in includedDiets)
                    {
                        sql += $"AND Dieet = '{incldiet}' ";
                    }
                }
                if (excludedDiets != null)
                {
                    foreach (var exldiet in excludedDiets)
                    {
                        sql += $"AND NOT Dieet = '{exldiet}' ";
                    }
                }

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return results;
        }

        public void SaveProfile(Profile profile)
		{
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "INSERT INTO Profile (Gebruikersnaam, Naam, Achternaam, Tussenvoegsels, Geboortedatum, Seksuele preferentie, Geslacht, Woonplaats) " +
                        "VALUES (@Gebruikersnaam, @Naam, @Achternaam, @Tussenvoegsels, @Geboortedatum, @Seksuelepreferentie, @Geslacht, @Woonplaats)";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
					command.Parameters.AddWithValue("Gebruikersnaam", profile.UserName);
                    command.Parameters.AddWithValue("Naam", profile.FirstName);
                    command.Parameters.AddWithValue("Achternaam", profile.LastName);
                    command.Parameters.AddWithValue("Tussenvoegsels", profile.Infix);
                    command.Parameters.AddWithValue("Geboortedatum", $"{profile.BirthDate.Year}-{profile.BirthDate.Month}-{profile.BirthDate.Day}");
					command.Parameters.AddWithValue("seksuelepreferentie", profile.SexualPreference);
					command.Parameters.AddWithValue("Geslacht", profile.Gender);
					command.Parameters.AddWithValue("Woonplaats", profile.City);
					command.ExecuteNonQuery();
				}
				connection.Close();
            }
        }

        public void StoreFile(string filename)
        {
            if (1!=2)//Als foto album niet er is ga door anders sla over
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    var sql = $"INSERT INTO FotoAlbum(ProfielGebruikersnaam) VALUES ('gebruikersnaam')";
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery() ;
                    }
                    connection.Close();
                }
            }
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"INSERT INTO Foto(ID, FotoTitel, FotoData, FotoAlbumID) VALUES (int, {Path.GetFileName(filename)}, {File.ReadAllBytes(filename)}, int)";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine(reader.GetString);
                    }
                }
                connection.Close();
            }
        }
    }

        /*public byte[] RetrieveFile(string filename)

        {
            SqlConnection connection = new SqlConnection("Server=(local) ; Initial Catalog = FileStore ; Integrated Security = SSPI");

            SqlCommand command = new SqlCommand("SELECT * FROM MyFiles WHERE Filename=@Filename", connection);

            command.Parameters.AddWithValue("@Filename", filename); connection.Open();
            SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.SequentialAccess);
            reader.Read();
            MemoryStream memory = new MemoryStream();
            long startIndex = 0;
            const int ChunkSize = 256;
            while (true)

            {

                byte[] buffer = new byte[ChunkSize];
                long retrievedBytes = reader.GetBytes(1, startIndex, buffer, 0, ChunkSize);
                memory.Write(buffer, 0, (int)retrievedBytes);
                startIndex += retrievedBytes;
                if (retrievedBytes != ChunkSize)
                    break;
            }

            connection.Close();
            byte[] data = memory.ToArray();
            memory.Dispose();
            return data;

        }
    }*/
}
