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
using Renci.SshNet;
using System.Diagnostics;
using System.Data;

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
            return new DateTime(byear, today.Month, today.Day).ToString("yyyy-MM-dd");
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

        public List<string> GetProfiles()
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
                        while (reader.Read())
                        {
                            profiles.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return profiles;
        }

        public List<string> GetProfiles(Profile profile)
        {
            List<string> profiles = new List<string>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT * FROM Profiel WHERE Gebruikersnaam != @userName";

                if (!profile.GetPreferredGender().Equals(PreferredGender.Both))
                {
                    sql += " AND Geslacht = @gender";
                }

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userName", profile.UserName);
                    command.Parameters.AddWithValue("gender", (Gender)profile.GetPreferredGender());

                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            profiles.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return profiles;
        }

        public List<string> GetProfiles(Profile profile, LocationFilter location, int minimumAge, int maximumAge,
            List<int> includedHobbies, List<int> excludedHobbies, List<Diet> includedDiets, List<Diet> excludedDiets, bool likebutt, bool matchbutt)
        {
            List<string> results = new List<string>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = new StringBuilder("SELECT DISTINCT Profiel.Gebruikersnaam FROM Profiel ");
                if (likebutt)
                {
                    sql.Append("JOIN MatchingDB.dbo.[Like] ON Profiel.Gebruikersnaam = [Like].Gebruiker1 OR Profiel.Gebruikersnaam = [Like].Gebruiker2 "
                    + $"WHERE (Gebruiker1 = '{profile.UserName}' OR Gebruiker2 = '{profile.UserName}') "
                    + "AND (Gebruiker1Liked = 'true' OR Gebruiker2Liked = 'true') ");
                }
                else if (matchbutt)
                {
                    sql.Append("JOIN MatchingDB.dbo.[Like] ON Profiel.Gebruikersnaam = [Like].Gebruiker1 OR Profiel.Gebruikersnaam = [Like].Gebruiker2 "
                    + $"WHERE (Gebruiker1 = '{profile.UserName}' OR Gebruiker2 = '{profile.UserName}') "
                    + "AND (Gebruiker1Liked = 'true' AND Gebruiker2Liked = 'true') ");
                }
                else
                {
                    sql.Append($"WHERE 1 = 1 ");
                }


                // Location
                if (location != LocationFilter.Global)
                {
                    sql.Append(location switch
                    {
                        LocationFilter.City => $"AND Woonplaats = '{profile.City}' AND Land = '{profile.Country}' ",
                        LocationFilter.Country => $"AND Land = '{profile.Country}' ",
                        _ => ""
                    });
                }
                sql.Append(minimumAge != 0 ? $"AND Geboortedatum <= '{AgeToDate(minimumAge)}' " : "");
                sql.Append(maximumAge != 0 ? $"AND Geboortedatum >= '{AgeToDate(maximumAge)}' " : "");

                sql.Append(includedHobbies.Count > 0
                    ? $"AND Gebruikersnaam IN (SELECT ProfielGebruikersnaam FROM Hobbies WHERE id IN ({string.Join(",", includedHobbies)})) "
                    : "");

                sql.Append(excludedHobbies.Count > 0
                    ? $"AND Gebruikersnaam NOT IN (SELECT ProfielGebruikersnaam FROM Hobbies WHERE id IN ({string.Join(",", excludedHobbies)})) "
                    : "");

                sql.Append(includedDiets.Count > 0
                    ? $"AND ({string.Join(" OR ", includedDiets.Select(inclDiet => $"Dieet = '{(int)inclDiet}'"))})"
                    : "");

                sql.Append(string.Join("", excludedDiets.Select(exclDiet => $"AND (Dieet IS NULL OR NOT Dieet = '{(int)exclDiet}')")));

                sql.Append(profile.GetPreferredGender() != PreferredGender.Both
                    ? $" AND (Geslacht = '{(int)profile.GetPreferredGender()}')"
                    : "");

                sql.Append($" AND Gebruikersnaam != '{profile.UserName}'");

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                {
                    command.ExecuteNonQuery();

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
                diet = (Diet)reader.GetInt16(12);
            }
            catch (SqlNullValueException ex) { diet = null; }

            try
            {
                vaccinated = reader.GetBoolean(13);
            }
            catch (SqlNullValueException ex) { vaccinated = null; }

            return new Profile(userName, firstName, infix, lastName, birthDate, gender, pref, city, postalCode, country, GetImages(userName), GetHobbies(userName), GetMatchingQuiz(userName), description, degree, school, workplace, diet, vaccinated);

        }




        public void SaveProfile(Profile profile)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Profiel (Gebruikersnaam, Naam, Achternaam, Tussenvoegsels, Geboortedatum, Seksuele_preferentie, Geslacht, Woonplaats, Land, Postcode) " +
                        "VALUES (@Gebruikersnaam, @Naam, @Achternaam, @Tussenvoegsels, @Geboortedatum, @Sekspref, @Geslacht, @Woonplaats, @land, @postcode)", connection))
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
            }
            StoreImages(profile);
        }

        public List<Interest> GetHobbies(string userName)
        {
            List<Interest> results = new List<Interest>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("Select ID FROM Hobbies WHERE ProfielGebruikersnaam = @userName", connection))
                {
                    command.Parameters.AddWithValue("userName", userName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
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
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                    "UPDATE profiel SET Gebruikersnaam = @Gebruikersnaam, Naam = @Naam, Achternaam = @Achternaam, Tussenvoegsels = @Tussenvoegsels," +
                    " Geboortedatum = @Geboortedatum, Seksuele_preferentie = @Sekspref, Geslacht = @Geslacht, Woonplaats = @Woonplaats, Land = @Land, Postcode = @Postcode," +
                    " Beschrijving = @Beschrijving, Opleiding = @Opleiding, School = @School, Werkplek = @Werkplek, Dieet = @Dieet WHERE Gebruikersnaam = @Gebruikersnaam", connection))
                {
                    command.Parameters.AddWithValue("Gebruikersnaam", profile.UserName);
                    command.Parameters.AddWithValue("Naam", profile.FirstName);
                    command.Parameters.AddWithValue("Achternaam", profile.LastName);
                    command.Parameters.AddWithValue("Tussenvoegsels", profile.Infix);
                    command.Parameters.AddWithValue("Geboortedatum", $"{profile.BirthDate:yyyy-MM-dd}");
                    command.Parameters.AddWithValue("Sekspref", profile.SexualPreference);
                    command.Parameters.AddWithValue("Geslacht", profile.Gender);
                    command.Parameters.AddWithValue("Woonplaats", profile.City);
                    command.Parameters.AddWithValue("Land", profile.Country);
                    command.Parameters.AddWithValue("Postcode", profile.PostalCode);
                    command.Parameters.AddWithValue("Beschrijving", profile.Description == null ? String.Empty : profile.Description);
                    command.Parameters.AddWithValue("Opleiding", profile.Degree == null ? String.Empty : profile.Degree);
                    command.Parameters.AddWithValue("School", profile.School == null ? String.Empty : profile.School);
                    command.Parameters.AddWithValue("Werkplek", profile.WorkPlace == null ? String.Empty : profile.WorkPlace);
                    command.Parameters.AddWithValue("Dieet", profile.Diet == null ? String.Empty : profile.Diet);
                    command.ExecuteNonQuery();
                }

                UpdateHobbies(profile);
                UpdateImages(profile);
            }
        }

        public void UpdateHobbies(Profile profile)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM Hobbies WHERE ProfielGebruikersnaam = @userName", connection))
                {
                    command.Parameters.AddWithValue("userName", profile.UserName);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("INSERT INTO Hobbies (ID, Hobby, ProfielGebruikersnaam) VALUES (@ID, @Hobby, @userName)", connection))
                {
                    foreach (var item in profile.Interests)
                    {
                        command.Parameters.AddWithValue("ID", item);
                        command.Parameters.AddWithValue("Hobby", nameof(item));
                        command.Parameters.AddWithValue("userName", profile.UserName);

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }
            }
        }

        public void UpdateImages(Profile profile)
        {
            if (!ValidateUserName(profile.UserName)) throw new InvalidUserNameException();

            CheckPhotoAlbum(profile);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "DELETE FROM Foto WHERE FotoAlbumID = (SELECT ID FROM FotoAlbum WHERE ProfielGebruikersnaam = @userName)";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userName", profile.UserName);
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
                connection.Open();
                foreach (var image in profile.Images)
                {
                    var sql = $"INSERT INTO Foto(FotoData, FotoAlbumID) VALUES (@imageData, (SELECT ID FROM FotoAlbum WHERE ProfielGebruikersnaam = @userName))";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("imageData", image);
                        command.Parameters.AddWithValue("userName", profile.UserName);
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }


        public List<byte[]> GetImages(string userName)
        {
            List<byte[]> images = new List<byte[]>();

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

                            images.Add(imageData);
                        }
                    }
                }
                connection.Close();
            }

            return images;
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

        public void SaveMatchingQuiz(List<int> answers, Profile profile)
        {
            int quizID;

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

                sql = "SELECT MAX(ID) FROM MatchingQuiz";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        quizID = reader.GetInt32(0);
                    }
                }

                sql = "UPDATE Profiel SET QuizID = @quizID WHERE Gebruikersnaam = @userName";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("quizID", quizID);
                    command.Parameters.AddWithValue("userName", profile.UserName);

                    command.ExecuteNonQuery();
                }

            }
        }

        //creates a new message request with a status of 0 (Sent)
        public void CreateMessageRequest(string sender, string receiver)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "INSERT INTO ChatVerzoek (Verzender, Ontvanger, Status) " +
                        "VALUES (@Sender, @Receiver, @Status)";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Sender", sender);
                    command.Parameters.AddWithValue("Receiver", receiver);
                    command.Parameters.AddWithValue("Status", 0);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // gets a list of every message request send to the receiver
        public List<string> GetMessageRequest(string receiver)
        {
            List<string> requests = new List<string>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT Verzender FROM ChatVerzoek WHERE Ontvanger = @Receiver";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Receiver", receiver);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            requests.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return requests;
        }

        public void UpdateMessageRequest(int status, string receiver, string sender)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "UPDATE ChatVerzoek SET Status = @Status WHERE Ontvanger = @receiver AND Verzender = @Sender";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Status", status);
                    command.Parameters.AddWithValue("Receiver", receiver);
                    command.Parameters.AddWithValue("Sender", sender);
                    command.ExecuteNonQuery();
                }
                connection.Close();
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

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT * FROM MatchingQuiz WHERE ID = (SELECT QuizID FROM Profiel WHERE Gebruikersnaam = @userName)";

                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
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
                                answers.Add(reader.GetInt16(i));
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
                            if (reader.IsDBNull(0) == false)
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

        public byte[] GetProfileImageData(string userName)
        {
            byte[] imageData;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT FotoData FROM Foto WHERE FotoAlbumID = (SELECT ID FROM FotoAlbum WHERE ProfielGebruikersnaam = @userName)";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userName", userName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            reader.Read();
                            imageData = (byte[])reader["FotoData"];
                        }
                        catch (InvalidOperationException)
                        {
                            connection.Close();
                            imageData = null;
                        }
                    }
                }
                connection.Close();
            }

            return imageData;
        }

        public List<string> GetContactNames(string userName)
        {
            List<string> activeChats = new();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT Verzender FROM ChatVerzoek WHERE Ontvanger = @userName AND Status = 1 UNION SELECT Ontvanger FROM ChatVerzoek WHERE Verzender = @userName AND Status = 1";

                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userName", userName);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            activeChats.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }

            activeChats = activeChats.OrderByDescending(x => GetLatestTimeStamp(userName, x)).ToList();

            return activeChats;
        }

        public List<Message> GetMessages(string user, string contact)
        {
            List<Message> messages = new();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT Bericht, Tijdstip, Verzender FROM Bericht WHERE (Verzender = @user AND Ontvanger = @contact) OR (Verzender = @contact AND Ontvanger = @user) ORDER BY Tijdstip";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("user", user);
                    command.Parameters.AddWithValue("contact", contact);
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string content = reader.GetString(0);
                            DateTime timeStamp = reader.GetDateTime(1);
                            bool isSender = reader.GetString(2) == user;
                            messages.Add(new Message(timeStamp, content, isSender));
                        }
                    }
                }
                connection.Close();
            }
            return messages;
        }

        public void SendMessage(Message message, string sender, string receiver)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "INSERT INTO Bericht (Verzender, Ontvanger, Tijdstip, Bericht) VALUES (@sender, @receiver, @timeStamp, @content)";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("sender", sender);
                    command.Parameters.AddWithValue("receiver", receiver);
                    command.Parameters.AddWithValue("timeStamp", message.TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss"));
                    command.Parameters.AddWithValue("content", message.Content);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

        }

        public DateTime? GetLatestTimeStamp(string user, string contact)
        {
            DateTime LatestTimeStamp;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT TOP 1 Tijdstip FROM Bericht WHERE (Verzender = @user AND Ontvanger = @contact) OR (Verzender = @contact AND Ontvanger = @user) ORDER BY Tijdstip DESC";

                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("user", user);
                    command.Parameters.AddWithValue("contact", contact);
                    try
                    {
                        command.ExecuteNonQuery();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            LatestTimeStamp = reader.GetDateTime(0);

                        }
                    }
                    catch (SqlException)
                    {
                        return DateTime.MinValue;
                    }
                    catch (InvalidOperationException ex)
                    {
                        connection.Close();
                        return DateTime.MinValue;
                    }
                    connection.Close();
                }
                return LatestTimeStamp;
            }
        }

        private (string, bool, string, bool) RetrieveLikeStatus(SqlConnection connection, string liker, string liked)
        {
            string gbr1, gbr2;
            bool isGebruiker1Liker, isGebruiker2Liker;

            var sqlCheckLikes = $"SELECT Gebruiker1, Gebruiker1Liked, Gebruiker2, Gebruiker1Liked FROM MatchingDB.dbo.[Like] WHERE (Gebruiker1 = @liker OR Gebruiker2 = @liker) AND (Gebruiker1 = @liked OR Gebruiker2 = @liked)";

            using (SqlCommand commandLikes = new SqlCommand(sqlCheckLikes, connection))
            {
                commandLikes.Parameters.AddWithValue("liker", liker);
                commandLikes.Parameters.AddWithValue("liked", liked);

                using (SqlDataReader readerLikes = commandLikes.ExecuteReader())
                {
                    readerLikes.Read();
                    if (readerLikes != null)
                    {
                        gbr1 = readerLikes.GetString(0);
                        isGebruiker1Liker = readerLikes.GetBoolean(1);
                        gbr2 = readerLikes.GetString(2);
                        isGebruiker2Liker = readerLikes.GetBoolean(3);
                        return (gbr1, isGebruiker1Liker, gbr2, isGebruiker2Liker);
                    }
                    throw new NullReferenceException();
                }
            }


        }

        public void LikeProfile(string liker, string liked)
        {
            bool isThere;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                // Check if the like record already exists
                var sqlCheckExistence = $"SELECT COUNT(*) FROM MatchingDB.dbo.[Like] WHERE (Gebruiker1 = @liker OR Gebruiker1 = @liked) AND (Gebruiker2 = @liker OR Gebruiker2 = @liked)";
                using (SqlCommand commandExistence = new SqlCommand(sqlCheckExistence, connection))
                {
                    commandExistence.Parameters.AddWithValue("liker", liker);
                    commandExistence.Parameters.AddWithValue("liked", liked);
                    isThere = (int)commandExistence.ExecuteScalar() > 0;
                }

                // Insert like record if not exists
                if (!isThere)
                {
                    var sqlInsertLike = $"INSERT INTO [Like] (Gebruiker1, Gebruiker1Liked, Gebruiker2, Gebruiker2Liked) VALUES (@liker, 'true', @liked, 'false');";
                    using (SqlCommand commandInsertLike = new SqlCommand(sqlInsertLike, connection))
                    {
                        commandInsertLike.Parameters.AddWithValue("liker", liker);
                        commandInsertLike.Parameters.AddWithValue("liked", liked);
                        commandInsertLike.ExecuteNonQuery();
                    }
                }

                var (gbr1, isGebruiker1Liked, gbr2, isGebruiker2Liked) = RetrieveLikeStatus(connection, liker, liked);
                var sqlUpdateLike = "";
                if (gbr1 == liker)
                {
                    sqlUpdateLike = $"UPDATE [Like] SET Gebruiker1Liked = 'true' WHERE Gebruiker1 = @liker AND Gebruiker2 = @liked;";
                }
                else
                {
                    sqlUpdateLike = $"UPDATE [Like] SET Gebruiker2Liked = 'true' WHERE Gebruiker1 = @liked AND Gebruiker2 = @liker;";
                }

                using (SqlCommand commandUpdateLike = new SqlCommand(sqlUpdateLike, connection))
                {
                    commandUpdateLike.Parameters.AddWithValue("liker", liker);
                    commandUpdateLike.Parameters.AddWithValue("liked", liked);
                    commandUpdateLike.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void DislikeProfile(string disliker, string disliked)
        {
            bool isThere;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                var sqlCheckExistence = $"SELECT COUNT(*) FROM MatchingDB.dbo.[Like] WHERE (Gebruiker1 = @disliker OR Gebruiker1 = @disliked) AND (Gebruiker2 = @disliker OR Gebruiker2 = @disliked)";
                using (SqlCommand commandExistence = new SqlCommand(sqlCheckExistence, connection))
                {
                    commandExistence.Parameters.AddWithValue("disliker", disliker);
                    commandExistence.Parameters.AddWithValue("disliked", disliked);
                    isThere = (int)commandExistence.ExecuteScalar() > 0;
                }

                if (!isThere)
                {
                    throw new InvalidDislikeException();
                }

                // Retrieve like status
                var (gbr1, isGebruiker1Liked, gbr2, isGebruiker2Liked) = RetrieveLikeStatus(connection, disliker, disliked);
                var sqlUpdateLike = "";
                if (gbr1 == disliker)
                {
                    sqlUpdateLike = $"UPDATE [Like] SET Gebruiker1Liked = 'false' WHERE Gebruiker1 = @disliker AND Gebruiker2 = @disliked;";
                } else
                {
                    sqlUpdateLike = $"UPDATE [Like] SET Gebruiker2Liked = 'false' WHERE Gebruiker1 = @disliked AND Gebruiker2 = @disliker;";
                }

                

                using (SqlCommand commandUpdateLike = new SqlCommand(sqlUpdateLike, connection))
                {
                    commandUpdateLike.Parameters.AddWithValue("disliker", disliker);
                    commandUpdateLike.Parameters.AddWithValue("disliked", disliked);
                    commandUpdateLike.ExecuteNonQuery();
                }
            }
        }

        public string CheckLikeStatus(string liker, string liked)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string existingLiker = null;

                // Retrieve liker who has already liked
                var sqlCheckLikes = $"SELECT Gebruiker1, Gebruiker1Liked, Gebruiker2, Gebruiker2Liked FROM MatchingDB.dbo.[Like] WHERE " +
                                    $"((Gebruiker1 = @liker AND Gebruiker2 = @liked) OR (Gebruiker1 = @liked AND Gebruiker2 = @liker)) " +
                                    $"AND (Gebruiker1Liked = 'true' OR Gebruiker2Liked = 'true')";

                using (SqlCommand commandLikes = new SqlCommand(sqlCheckLikes, connection))
                {
                    commandLikes.Parameters.AddWithValue("liker", liker);
                    commandLikes.Parameters.AddWithValue("liked", liked);

                    using (SqlDataReader readerLikes = commandLikes.ExecuteReader())
                    {
                        if (readerLikes.Read())
                        {
                            existingLiker = readerLikes.GetString(0) == liker && readerLikes.GetBoolean(1) ? liker 
                                : (readerLikes.GetString(2) == liker && readerLikes.GetBoolean(3) ? liker : liked);
                        }
                    }
                }

                connection.Close();

                return existingLiker;
            }
        }


        public (List<string>, List<bool>, List<bool>) FilterLikes(Profile profile)
        {
            List<string> profiles = new List<string>();
            List<bool> likes = new List<bool>();
            List<bool> isLiked = new List<bool>();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT Profiel.Gebruikersnaam, Gebruiker1, Gebruiker1Liked, Gebruiker2Liked FROM Profiel JOIN [Like] ON [Like].Gebruiker1 = Profiel.Gebruikersnaam OR [Like].Gebruiker2 = Profiel.Gebruikersnaam "
                    + "WHERE (Gebruiker1 = @Gebruiker OR Gebruiker2 = @Gebruiker)"
                    + "AND (Gebruiker1Liked = 'true' OR Gebruiker2Liked = 'true') AND Profiel.Gebruikersnaam != @Gebruiker";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Gebruiker", profile.UserName);
                    command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            profiles.Add(reader.GetString(0));
                            if(reader.GetString(1) == profile.UserName)
                            {
                                likes.Add(reader.GetBoolean(2));
                                isLiked.Add(reader.GetBoolean(3));
                            }
                            else
                            {
                                likes.Add(reader.GetBoolean(3));
                                isLiked.Add(reader.GetBoolean(2));
                            }
                            
                        }
                    }
                }
                connection.Close();
            }
            return (profiles, likes, isLiked);
        }

        public List<string> FilterMatch(Profile profile)
        {
            List<string> profiles = new List<string>();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var sql = "SELECT Profiel.* FROM MatchingDB.dbo.Profiel JOIN MatchingDB.dbo.[Like] ON Profiel.Gebruikersnaam = [Like].Gebruiker1 OR Profiel.Gebruikersnaam = [Like].Gebruiker2 "
                    + "WHERE (Gebruiker1 = @Gebruiker OR Gebruiker2 = @Gebruiker)"
                    + "AND (Gebruiker1Liked = 'true' AND Gebruiker2Liked = 'true') AND Profiel.Gebruikersnaam != @Gebruiker;";
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Gebruiker", profile.UserName);
                    command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            profiles.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return profiles;
        }

    }
}