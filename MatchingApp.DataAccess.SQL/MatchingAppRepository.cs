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
using System.Data.SqlTypes;

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
                        profile = ProfileFromQuery(reader);
                    }
				}
				connection.Close();
			}
			return profile;
		}

		public List<Profile> GetProfiles()
		{
            List<string> profiles = new List<string>();

			using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT * FROM Profiel";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            profiles.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return profiles;
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
                    sql += $"AND Geboortedatum <= '{AgeToDate(minimumAge)}' "; 
				} 
				if (maximumAge != 0)
                {
                    sql += $"AND Geboortedatum >= '{AgeToDate(maximumAge)}' ";
                }
                if (includedHobbys.Count > 0)
                {
                    sql += "AND (";

                    bool isFirstHobby = true;

                    foreach (var inclhobby in includedHobbys)
                    {
                        if (!isFirstHobby)
                        {
                            sql += " OR ";
                        }

                        sql += $"Hobby = '{inclhobby}'";
                        isFirstHobby = false;
                    }

                    sql += ")";
                }
                if (excludedHobbys.Count > 0)
                {
                    sql += "AND NOT (";

                    bool isFirstHobby = true;

                    foreach (var exclHobby in excludedHobbys)
                    {
                        if (!isFirstHobby)
                        {
                            sql += " OR ";
                        }

                        sql += $"Hobby = '{exclHobby}'";
                        isFirstHobby = false;
                    }

                    sql += ")";
                }
                if (includedDiets.Count > 0)
                {
                    sql += "AND (";

                    bool isFirstDiet = true;

                    foreach (var inclDiet in includedDiets)
                    {
                        if (!isFirstDiet)
                        {
                            sql += " OR ";
                        }

                        sql += $"Dieet = '{inclDiet}'";
                        isFirstDiet = false;
                    }

                    sql += ")";
                }
                if (excludedDiets.Count > 0)
                {
                    sql += "AND NOT (";

                    bool isFirstDiet = true;

                    foreach (var exclDiet in excludedDiets)
                    {
                        if (!isFirstDiet)
                        {
                            sql += " OR ";
                        }

                        sql += $"Dieet = '{exclDiet}'";
                        isFirstDiet = false;
                    }

                    sql += ")";
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

        private Profile ProfileFromQuery(SqlDataReader reader)
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
            string description;
            string degree;
            string school;
            string workplace;
            Diet? diet;
            bool? vaccinated;

            try
            {
                description = reader.GetString(8);
            }
            catch (SqlNullValueException ex) { description = null; }

            try
            {
                degree = reader.GetString(9);
            }
            catch (SqlNullValueException ex) { degree = null; }

            try
            {
                school = reader.GetString(10);
            }
            catch (SqlNullValueException ex) { school = null; }

            try
            {
                workplace = reader.GetString(11);
            }
            catch (SqlNullValueException ex) { workplace = null; }

            try
            {
                diet = (Diet) reader.GetInt16(12);
            }
            catch (SqlNullValueException ex) { diet = null; }

            try
            {
                vaccinated = reader.GetBoolean(13);
            }
            catch (SqlNullValueException ex) { vaccinated = null; }

            return new Profile(userName, firstName, infix, lastName, birthDate, gender, pref, city, postalCode, country, new List<string>(), GetHobbies(userName), GetMatchingQuiz(userName), description, degree, school, workplace, diet, vaccinated);

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

        public List<Interest> GetHobbies(string userName)
        {
            List<Interest> results = new List<Interest>();

            using(SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "Select ID FROM Hobbies WHERE ProfielGebruikersnaam = @userName";

                connection.Open();

                using(SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userName", userName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader =  command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add((Interest)reader.GetInt32(0));
                        }
                    }
                }
            }
            return results;
        }

        
        public void UpdateProfile(Profile profile) 
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "UPDATE profiel SET Gebruikersnaam = @Gebruikersnaam, Naam = @Naam, Achternaam = @Achternaam, Tussenvoegsels = @Tussenvoegsels," +
                    " Geboortedatum = @Geboortedatum, Seksuele_preferentie = @Sekspref, Geslacht = @Geslacht, Woonplaats = @Woonplaats, Land = @Land, Postcode = @Postcode," +
                    " Beschrijving = @Beschrijving, Opleiding = @Opleiding, School = @School, Werkplek = @Werkplek, Dieet = @Dieet WHERE Gebruikersnaam = @Gebruikersnaam";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Gebruikersnaam", profile.UserName);
                    command.Parameters.AddWithValue("Naam", profile.FirstName);
                    command.Parameters.AddWithValue("Achternaam", profile.LastName);
                    command.Parameters.AddWithValue("Tussenvoegsels", profile.Infix);
                    command.Parameters.AddWithValue("Geboortedatum", $"{profile.BirthDate.Year}-{profile.BirthDate.Month}-{profile.BirthDate.Day}");
                    command.Parameters.AddWithValue("Sekspref", profile.SexualPreference);
                    command.Parameters.AddWithValue("Geslacht", profile.Gender);
                    command.Parameters.AddWithValue("Woonplaats", profile.City);
                    command.Parameters.AddWithValue("Land", profile.Country);
                    command.Parameters.AddWithValue("Postcode", profile.PostalCode);
                    command.Parameters.AddWithValue("Beschrijving", profile.Description);
                    command.Parameters.AddWithValue("Opleiding", profile.degree);
                    command.Parameters.AddWithValue("School", profile.School);
                    command.Parameters.AddWithValue("Werkplek", profile.WorkPlace);
                    command.Parameters.AddWithValue("Dieet", profile.Diet);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
                
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


        public List<Image> RetrieveImages(string userName)
        {
            List<Image> images = new List<Image>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                var sql = $"SELECT FotoData FROM Foto WHERE FotoAlbumID = (SELECT ID FROM FotoAlbum WHERE ProfielGebruikersnaam = @gebruikersnaam)";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("gebruikersnaam", userName);

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

        public void SaveMatchingQuiz(List<int> answers)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = $"SELECT count(*) FROM MatchingQuiz WHERE ";
                for (int i = 1; i <= 13; i++)
                {
                    sql = sql + $"Vraag{i} = @vraag{i} AND ";
                }
                sql = sql + "1 = 1";

                int amount;

                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    for (int i = 0; i < answers.Count; i++)
                    {
                        command.Parameters.AddWithValue($"vraag{i + 1}", answers[i]);
                    }
                    command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        amount = reader.GetInt32(0);
                    }
                }

                if (amount > 0)
                {
                    connection.Close();
                    return;
                }

                sql = $"INSERT INTO MatchingQuiz (Vraag1, Vraag2, Vraag3, Vraag4, Vraag5, Vraag6, Vraag7, Vraag8, Vraag9, Vraag10, Vraag11, Vraag12, Vraag13) VALUES (@Vraag1, @Vraag2, @Vraag3, @Vraag4, @Vraag5, @Vraag6, @Vraag7, @Vraag8, @Vraag9, @Vraag10, @Vraag11, @Vraag12, @Vraag13)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    for (int i = 0; i < answers.Count; i++)
                    {
                        command.Parameters.AddWithValue($"Vraag{i + 1}", answers[i]);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }
        public bool ValidateUserName(string userName)
        {
            List<string> profiles = GetProfiles();
            return profiles.Any(x => x == userName);
        }

        public List<int> GetMatchingQuiz(string userName)
        {
            List<int> answers = new List<int>();

            using(SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT * FROM MatchingQuiz WHERE ID = (SELECT QuizID FROM Profiel WHERE Gebruikersnaam = @userName)";

                connection.Open();

                using(SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userName", userName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        for (int i = 0; i < 13; i++)
                        {
                            try
                            {
                                answers.Add(reader.GetInt32(i));
                            }
                            catch (InvalidOperationException)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return answers;
        }
    }
}
