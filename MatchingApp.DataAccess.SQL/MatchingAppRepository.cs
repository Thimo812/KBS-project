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
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"SELECT DISTINCT Profiel.Gebruikersnaam FROM Profiel LEFT JOIN Hobbies ON Profiel.Gebruikersnaam=Hobbies.ProfielGebruikersnaam WHERE 1 = 1 ";
                if (location != 0)
                {
					sql += $"AND Woonplaats = '{location}' "; 
				}
                if (minimumAge != null)
                {
                    sql += $"AND Geboortedatum <= '{AgetoDate(minimumAge)}' "; 
				} 
				if (maximumAge != null)
                {
                    sql += $"AND Geboortedatum >= '{AgetoDate(maximumAge)}' ";
                }
                if (includedHobbys != null)
                {
                    foreach (var inclhobby in includedHobbys)
                    {
                        sql += $"AND Hobby = {inclhobby} ";
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
                            Console.WriteLine(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return null;
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
