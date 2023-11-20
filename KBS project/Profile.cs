using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace KBS_project
{
	public class Profile
	{
		public string UserName { get; set; }
		public string FirstName { get; set; }
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

		public Profile(string userName, string firstName, string lastName, DateTime birthDate, Gender gender, 
			SexualPreference sexualPreference, string postalCode, string country,string adress, string city)
		{
			UserName = userName;
			FirstName = firstName;
			LastName = lastName;
			BirthDate = birthDate;
			Gender = gender;
			SexualPreference = sexualPreference;
			PostalCode = postalCode;
			Country = country;
			Adress = adress;
			City = city;
		}
	}
}