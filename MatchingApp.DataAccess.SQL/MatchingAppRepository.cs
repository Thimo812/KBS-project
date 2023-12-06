using KBS_project;
using KBS_project.Enums;
using KBS_project.Enums.FilterOptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using KBS_project.Exceptions;

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
			builder.InitialCatalog = "MatchingDB";
			builder.TrustServerCertificate = false;
		}

		public string AgeToDate(int age)
		{
            var today = DateTime.Today;
            var byear = today.Year - age;
			DateTime date = new DateTime(byear, today.Month, today.Day);
            var dateString = date.ToString("yyyy-MM-dd");
            return dateString;
        }
		public Profile GetProfile(string userName)
		{
			Profile profile;

			using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
			{
				var sql = $"SELECT * FROM Profiel WHERE Gebruikersnaam = @Username";
				connection.Open();
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
                    command.Parameters.AddWithValue("Username", userName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
					{
						reader.Read();
						string firstName = reader.GetString(1);
						string lastName = reader.GetString(2);
						string infix = reader.GetString(3);
						DateTime birthDate = reader.GetDateTime(4);
						SexualPreference pref = (SexualPreference) int.Parse(reader.GetString(5));
						Gender gender = (Gender) int.Parse((reader.GetString(6)));
						string city = reader.GetString(7);
                        string country = reader.GetString(14);
                        string postalCode = reader.GetString(15);



						profile = new(userName, firstName, infix, lastName, birthDate, gender, pref, city, country, postalCode, new List<string>());
					}
				}
				connection.Close();
			}
			return profile;
		}

		public List<Profile> GetProfiles()
		{
            List<Profile> profiles = new List<Profile>();

			using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT * FROM Profiel";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            string userName = reader.GetString(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string infix = reader.GetString(3);
                            DateTime birthDate = reader.GetDateTime(4);
                            SexualPreference pref = (SexualPreference)int.Parse(reader.GetString(5));
                            Gender gender = (Gender)int.Parse((reader.GetString(6)));
                            string city = reader.GetString(7);
                            string country = reader.GetString(14);
                            string postalCode = reader.GetString(15);

                            profiles.Add(new Profile(userName, firstName, infix, lastName, birthDate, gender, pref, city, country, postalCode, new List<string>()));
                        }
                    }
                }
                connection.Close();
            }
            return profiles;
		}

        public List<string> GetProfiles(LocationFilter location, int minimumAge, int maximumAge,
    List<string> includedHobbys, List<string> excludedHobbys, List<Diet> includedDiets, List<Diet> excludedDiets)
        {
            List<string> results = new();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"SELECT DISTINCT Profiel.Gebruikersnaam FROM Profiel WHERE 1 = 1 ";

                if (location != LocationFilter.Global)
                {
                    if (location == LocationFilter.City) { sql += $"AND Woonplaats = 'Mountaintop' AND Land = 'Nederland' "; } //Moet uiteindelijk van het profiel komen boop
                    if (location == LocationFilter.Country) { sql += $"AND Land = 'Nederland' "; }
                }
                if (minimumAge != 0)
                {
                    sql += $"AND Geboortedatum <= '{AgeToDate(minimumAge)}' ";
                }
                if (maximumAge != 0)
                {
                    sql += $"AND Geboortedatum >= '{AgeToDate(maximumAge)}' ";
                }

                if (includedHobbys.Count > 0)
                {
                    sql += $"AND Profiel.Gebruikersnaam IN (SELECT ProfielGebruikersnaam FROM Hobbies WHERE Hobby IN ({string.Join(",", includedHobbys.Select(h => $"'{h}'"))})) ";
                }

                if (excludedHobbys.Count > 0)
                {
                    sql += $"AND Profiel.Gebruikersnaam NOT IN (SELECT ProfielGebruikersnaam FROM Hobbies WHERE Hobby IN ({string.Join(",", excludedHobbys.Select(h => $"'{h}'"))})) ";
                }
                if (includedDiets.Count > 0)
                {
                    foreach (var inclDiet in includedDiets)
                    {
                        sql += $"AND Dieet = '{(int)inclDiet}' ";
                    }
                }
                if (excludedDiets.Count > 0)
                {
                    foreach (var exclDiet in excludedDiets)
                    {
                        sql += $"AND NOT Dieet = '{(int)exclDiet}' ";
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
                var sql = "INSERT INTO Profiel (Gebruikersnaam, Naam, Achternaam, Tussenvoegsels, Geboortedatum, Seksuele_preferentie, Geslacht, Woonplaats, Land, Postcode) " +
                        "VALUES (@Gebruikersnaam, @Naam, @Achternaam, @Tussenvoegsels, @Geboortedatum, @Sekspref, @Geslacht, @Woonplaats, @land, @postcode)";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
					command.Parameters.AddWithValue("Gebruikersnaam", profile.UserName);
                    command.Parameters.AddWithValue("Naam", profile.FirstName);
                    command.Parameters.AddWithValue("Achternaam", profile.LastName);
                    command.Parameters.AddWithValue("Tussenvoegsels", profile.Infix);
                    command.Parameters.AddWithValue("Geboortedatum", $"{profile.BirthDate.Year}-{profile.BirthDate.Month}-{profile.BirthDate.Day}");
					command.Parameters.AddWithValue("sekspref", profile.SexualPreference);
					command.Parameters.AddWithValue("Geslacht", profile.Gender);
					command.Parameters.AddWithValue("Woonplaats", profile.City);
                    command.Parameters.AddWithValue("land", profile.Country);
                    command.Parameters.AddWithValue("postcode", profile.PostalCode);
                    command.ExecuteNonQuery();
				}
				connection.Close();
            }

            StoreImages(profile);
        }

        public void StoreImages(Profile profile)
        {
            if (!ValidateUserName(profile.UserName)) throw new InvalidUserNameException();

            CheckPhotoAlbum(profile);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                foreach (var image in profile.Images)
                {
                    byte[] imageData = File.ReadAllBytes(image);
                    var sql = $"INSERT INTO Foto(FotoData, FotoAlbumID) VALUES (@imageData, (SELECT ID FROM FotoAlbum WHERE ProfielGebruikersnaam = @userName))";
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("imageData", imageData);
                        command.Parameters.AddWithValue("userName", profile.UserName);
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }


        public List<Image> RetrieveImages(Profile profile)
        {
            List<Image> images = new List<Image>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                var sql = $"SELECT FotoData FROM Foto WHERE FotoAlbumID = (SELECT ID FROM FotoAlbum WHERE ProfielGebruikersnaam = @gebruikersnaam)";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("gebruikersnaam", profile.UserName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[] imageData = (byte[])reader["FotoData"];

                            images.Add(ByteArrayToImage(imageData));
                        }
                    }
                }
            }

            return images;
        }

        static Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                return Image.FromStream(stream);
            }
        }

        private void CheckPhotoAlbum(Profile profile)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"SELECT COUNT(*) as amount FROM FotoAlbum WHERE ProfielGebruikersnaam = @Gebruikersnaam";

                int amount;

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Gebruikersnaam", profile.UserName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        amount = (int)reader["amount"];
                    }
                }

                if (amount == 0)
                {
                    var insertAlbumQuery = $"INSERT INTO FotoAlbum (ProfielGebruikersnaam) VALUES (@Gebruikersnaam)";

                    using (SqlCommand command = new SqlCommand(insertAlbumQuery, connection))
                    {
                        command.Parameters.AddWithValue("Gebruikersnaam", profile.UserName);
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        public bool ValidateUserName(string userName)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT COUNT(*) as amount FROM Profiel WHERE Gebruikersnaam = @userName";

                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userName", userName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        if (reader.GetInt32(0) == 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public Dictionary<int, string> GetHobbies()
        {
            Dictionary<int, string> hobbies = new();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT DISTINCT Hobby FROM Hobbies";

                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);

                            hobbies.Add(i, name);
                            i++;
                        }
                    }
                }
            }
            return hobbies;
        }

        public List<Diet> GetDiets()
        {
            List<Diet> diets = new();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT DISTINCT Dieet FROM Profiel";

                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(reader.IsDBNull(0) == false)
                            {
                                int diet = reader.GetInt16(reader.GetOrdinal("Dieet"));
                                diets.Add((Diet)diet);
                            }
                        }
                    }
                }
            }
            return diets;
        }
    }
}
