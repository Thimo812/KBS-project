using KBS_project;
using KBS_project.Enums;

namespace MatchingTest
{
    [TestFixture]
    public class ProfileTests
    {
        [Test]
        public void GetMatchingNumber_Should_Return_Matching_Percentage()
        {
            // Arrange
            var profile1 = new Profile("User1", "John", "Doe", DateTime.Now, Gender.Male, SexualPreference.Men,
                "12345", "Country1", "Address1", "City1");

            var profile2 = new Profile("User2", "Jane", "Doe", DateTime.Now, Gender.Female, SexualPreference.Men,
                "54321", "Country2", "Address2", "City2");

            // Mocking the answers for the profiles
            profile1.Answers.StoreAnswer("Question1", "Answer1");
            profile1.Answers.StoreAnswer("Question2", "Answer2");

            profile2.Answers.StoreAnswer("Question1", "Answer1");
            profile2.Answers.StoreAnswer("Question2", "Answer3");

            // Act
            int matchingPercentage = profile1.GetMatchingNumber(profile2);

            // Assert
            Assert.AreEqual(50, matchingPercentage);
        }
    }
}