using KBS_project;
using KBS_project.Enums;
using KBS_project.Enums.FilterOptions;
using KBS_project.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingAppTest
{
    [TestClass]
    public class RegistrationFieldsExtensionsTest
    {
        [TestMethod]
        public void MinimumLength_ReturnsCorrectValue()
        {
            var fields = new List<RegistrationFields>
            {
                RegistrationFields.UserName,
                RegistrationFields.FirstName,
                RegistrationFields.LastName,
            };

            foreach (var field in fields)
            {
                int result = field.MinimumLength();

                switch (field)
                {
                    case RegistrationFields.UserName:
                        Assert.AreEqual(3, result);
                        break;
                    case RegistrationFields.FirstName:
                        Assert.AreEqual(2, result);
                        break;
                    case RegistrationFields.LastName:
                        Assert.AreEqual(1, result);
                        break;
                    default:
                        Assert.Fail($"Unexpected field: {field}");
                        break;
                }
            }
        }

        [TestMethod]
        public void MaximumLength_ReturnsCorrectValue()
        {
            var fields = new List<RegistrationFields>
            {
                RegistrationFields.UserName,
                RegistrationFields.FirstName,
                RegistrationFields.LastName,
                RegistrationFields.Infix
            };

            foreach (var field in fields)
            {
                int result = field.MaximumLength();

                switch (field)
                {
                    case RegistrationFields.UserName:
                    case RegistrationFields.FirstName:
                        Assert.AreEqual(15, result);
                        break;
                    case RegistrationFields.LastName:
                        Assert.AreEqual(1, result);
                        break;
                    case RegistrationFields.Infix:
                        Assert.AreEqual(10, result);
                        break;
                    default:
                        Assert.Fail($"Unexpected field: {field}");
                        break;
                }
            }
        }

        [TestMethod]
        public void AllowedCharacters_ReturnsCorrectPattern()
        {
            var fields = Enum.GetValues(typeof(RegistrationFields)).Cast<RegistrationFields>();

            foreach (var field in fields)
            {
                string result = field.AllowedCharacters();

                switch (field)
                {
                    case RegistrationFields.PostalCode:
                    case RegistrationFields.UserName:
                        Assert.AreEqual("^[a-zA-Z0-9]+$", result);
                        break;
                    default:
                        Assert.AreEqual("^[a-zA-Z]+$", result);
                        break;
                }
            }
        }
        [TestMethod]
        public void Validate_InvalidInput_ThrowsInvalidFieldException()
        {
            var invalidInput = "Invalid!@#$";
            var field = RegistrationFields.FirstName;
            var repo = new MockMatchingAppRepository();

            Assert.ThrowsException<InvalidFieldException>(() =>
            {
                RegistrationFieldsExtensions.Validate(invalidInput, field, repo);
            });
        }

        [TestMethod]
        public void Validate_InfixWithEmptyInput_ReturnsInput()
        {
            var emptyInput = string.Empty;
            var field = RegistrationFields.Infix;
            var repo = new MockMatchingAppRepository();

            var result = RegistrationFieldsExtensions.Validate(emptyInput, field, repo);

            Assert.AreEqual(emptyInput, result);
        }

        [TestMethod]
        public void ValidateBirthDate_ValidInput_ReturnsInput()
        {
            var validBirthDate = DateTime.Today.AddYears(-20);

            var result = RegistrationFieldsExtensions.Validate(validBirthDate);

            Assert.AreEqual(validBirthDate, result);
        }

        [TestMethod]
        public void ValidateBirthDate_NullInput_ThrowsInvalidFieldException()
        {
            DateTime? nullInput = null;

            Assert.ThrowsException<InvalidFieldException>(() =>
            {
                RegistrationFieldsExtensions.Validate(nullInput);
            });
        }

        [TestMethod]
        public void ValidateBirthDate_UnderageInput_ThrowsInvalidFieldException()
        {
            var underageBirthDate = DateTime.Today.AddYears(-17);

            Assert.ThrowsException<InvalidFieldException>(() =>
            {
                RegistrationFieldsExtensions.Validate(underageBirthDate);
            });
        }

        [TestMethod]
        public void ValidateGender_ValidInput_ReturnsGender()
        {
            var validRadioButtons = new List<bool?> { false, true, false };

            var result = RegistrationFieldsExtensions.ValidateGender(validRadioButtons);

            Assert.AreEqual(Gender.Female, result);
        }

        [TestMethod]
        public void ValidateGender_InvalidInput_ThrowsInvalidFieldException()
        {
            var invalidRadioButtons = new List<bool?> { false, false, false };

            Assert.ThrowsException<InvalidFieldException>(() =>
            {
                RegistrationFieldsExtensions.ValidateGender(invalidRadioButtons);
            });
        }

        [TestMethod]
        public void ValidateSexuality_ValidInput_ReturnsSexualPreference()
        {
            var validRadioButtons = new List<bool?> { true, false, false };

            var result = RegistrationFieldsExtensions.ValidateSexuality(validRadioButtons);

            Assert.AreEqual(SexualPreference.Hetero, result);
        }

        [TestMethod]
        public void ValidateSexuality_InvalidInput_ThrowsInvalidFieldException()
        {
            var invalidRadioButtons = new List<bool?> { false, false, false };

            Assert.ThrowsException<InvalidFieldException>(() =>
            {
                RegistrationFieldsExtensions.ValidateSexuality(invalidRadioButtons);
            });
        }
    }

    public class MockMatchingAppRepository : IMatchingAppRepository
    {
        private List<string> profiles = new List<string>();

        public void AddProfile(string username)
        {
            profiles.Add(username);
        }

        public Profile GetProfile(string userName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetProfiles()
        {
            return profiles;
        }

        public List<string> GetProfiles(Profile profile, LocationFilter location, int minimumAge, int maximumAge, List<int> includedHobbys, List<int> excludedHobbys, List<Diet> includedDiets, List<Diet> excludedDiets, bool likebutt)
        {
            throw new NotImplementedException();
        }

        public void SaveMatchingQuiz(List<int> answers, Profile profile)
        {
            throw new NotImplementedException();
        }

        public void SaveProfile(Profile profile)
        {
            throw new NotImplementedException();
        }

        public void StoreImages(Profile profile)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUserName(string userName)
        {
            throw new NotImplementedException();
        }

        List<string> IMatchingAppRepository.GetProfiles()
        {
            throw new NotImplementedException();
        }
    }
}
