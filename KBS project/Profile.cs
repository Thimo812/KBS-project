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
		public string Adress { get; set; }
		public string City { get; set; }
		public string School { get; set; }
		public string degree { get; set; }
		public string WorkPlace { get; set; }
		public string Diet { get; set; }
		public string Description { get; set; }
		public List<Interest> Interests { get; set; }
		public List<string> Images { get; set; }

		public Profile(string userName, string firstName, string infix, string lastName, DateTime birthDate, Gender gender, 
			SexualPreference sexualPreference, string city)
		{
			UserName = userName;
			FirstName = firstName;
			Infix = infix;
			LastName = lastName;
			BirthDate = birthDate;
			Gender = gender;
			SexualPreference = sexualPreference;
			City = city;
		}

		public override string ToString()
		{
			return $"Username: {UserName}\nFull name: {FirstName + Infix + LastName}\nBirth date: {BirthDate.Day}-{BirthDate.Month}-{BirthDate.Year}\nGender: {Gender}\nSexual preference: {SexualPreference}";
		}
    }
}