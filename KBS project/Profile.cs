using System.Globalization;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using KBS_project.Enums;

namespace KBS_project
{
    public class Profile
	{
        
        public AnswerManager Answers { get; } = new AnswerManager();
        


        public string UserName { get; set; }
		public string FirstName { get; set; }

        public string Infix {  get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public Gender Gender { get; set; }
		public SexualPreference SexualPreference { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string School { get; set; }
		public string degree { get; set; }
		public string WorkPlace { get; set; }
		public string Diet { get; set; }
		public string Description { get; set; }
		public List<Interest> Interests { get; set; }
		public List<string> Images { get; set; }
        public List<int> QuizAnswers {  get; set; }

        

        public Profile(string userName, string firstName, string infix, string lastName, DateTime birthDate, Gender gender, 
			SexualPreference sexualPreference, string postalCode, string country, string city, List<string> images)
		{
			UserName = userName;
			FirstName = firstName;
            Infix = infix;
			LastName = lastName;
			BirthDate = birthDate;
			Gender = gender;
			SexualPreference = sexualPreference;
			PostalCode = postalCode;
			Country = country;
			City = city;
            Images = images;
		}

        public IEnumerable<string> GetQuestions()
        {
            return Answers.GetQuestions();
        }

        public int GetMatchingNumber(Profile otherProfile)
        {
            int matchingCount = 0;
            int totalQuestions = 0;

            // Iterate through each saved answer in the AnswerManager
            foreach (var question in otherProfile.Answers.GetQuestions())
            {
                totalQuestions++;

                // antwoord voor de vraag
                var userAnswer = this.Answers.GetAnswer(question);
                // antwoord voor de vraag andere profiel
                var otherProfileAnswer = otherProfile.Answers.GetAnswer(question);

                // Compare answers and increment matchingCount if they are the same
                if (userAnswer != null && otherProfileAnswer != null && userAnswer == otherProfileAnswer)
                {
                    matchingCount++;
                }
            }

            // Calculate the matching percentage
            double matchingPercentage = (double)matchingCount / totalQuestions * 100;

            return (int)matchingPercentage;
        }
    
	}
}