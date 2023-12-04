using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using KBS_project.Enums;

namespace KBS_project
{
    public class Profile
	{
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string Infix { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public Gender Gender { get; set; }
		public SexualPreference SexualPreference { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string? School { get; set; }
		public string? Degree { get; set; }
		public string? WorkPlace { get; set; }
		public Diet? Diet { get; set; }
		public string? Description { get; set; }
		public bool? Vaccinated {  get; set; }
		public List<Interest>? Interests { get; set; }
		public List<string>? Images { get; set; }
		public List<int>? QuizAnswers { get; set; }

		public Profile(string userName, string firstName, string infix, string lastName, DateTime birthDate, Gender gender, 
			SexualPreference sexualPreference, string city, string postalCode, string country, List<string> images, 
			List<Interest> interests, List<int> quizAnswers, string description, string degree, string school, string workplace, Diet? diet, bool? vaccinated) : 
			this(userName, firstName, infix, lastName, birthDate, gender, sexualPreference, city, postalCode, country, images)
		{
			Interests = interests;
			QuizAnswers = quizAnswers;
			Description = description;
			Degree = degree;
			School = school;
			WorkPlace = workplace;
			Diet = diet;
			Vaccinated = vaccinated;
		}

        public Profile(string userName, string firstName, string infix, string lastName, DateTime birthDate, Gender gender,
            SexualPreference sexualPreference, string city, string postalCode, string country, List<string> images)
        {
            UserName = userName;
            FirstName = firstName;
            Infix = infix;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            SexualPreference = sexualPreference;
            City = city;
            PostalCode = postalCode;
            Country = country;
            Images = images;
			Interests = new List<Interest>();
        }

		public int Age()
		{
			int result = 0;
			DateTime today = DateTime.Today;

			result = today.Year - BirthDate.Year;
			DateTime birthday = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
			if(birthday.CompareTo(today) > 0)
			{
				result--;
			}
			return result;
		}

        public override string ToString()
		{
			return
				$"Username: {UserName}\n" +
				$"Full name: {FirstName} {Infix} {LastName}\n" +
				$"Birth date: {BirthDate.Day}-{BirthDate.Month}-{BirthDate.Year}\n" +
				$"Gender: {Gender}\n" +
				$"Sexual preference: {SexualPreference}\n" +
				$"Adress: {PostalCode} {City}, {Country}\n" +
				$"Degree: {Degree}\n" +
				$"School: {School} \n" +
				$"workplace: {WorkPlace} \n" +
				$"Diet: {Diet}\n" +
				$"Description: {Description} \n" +
				$"Vaccinated: {Vaccinated} \n";
		}
    }
}