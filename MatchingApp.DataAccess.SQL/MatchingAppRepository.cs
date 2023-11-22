using KBS_project;
using KBS_project.Enums;
using KBS_project.Enums.FilterOptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingApp.DataAccess.SQL
{
	public class MatchingAppRepository : IMatchingAppRepository
	{
		private SqlConnectionStringBuilder builder;
		public MatchingAppRepository()
		{
			builder = new SqlConnectionStringBuilder();
			builder.DataSource = "127.0.0.1";
			builder.UserID = "SA";
			builder.Password = "D1t1sEenSqlServertju";
			builder.InitialCatalog = "testDB";
			builder.TrustServerCertificate = false;
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

		public List<Profile> GetProfiles()
		{
			throw new NotImplementedException();
		}

		public List<Profile> GetProfiles(LocationFilter location, int minimumAge, int maximumAge, 
			List<Interest> includedHobbys, List<Interest> excludedHobbys, List<Diet> includedDiets, List<Diet> excludedDiets)
		{
			throw new NotImplementedException();
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
	}
}
