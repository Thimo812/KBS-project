using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KBS_project.Enums
{
    public enum RegistrationFields
    {
        UserName,
        FirstName,
        LastName,
        Infix,
        City,
        Country,
        PostalCode,
        Gender,
        Sexuality,
        BirthDate
    }

    public static class RegistrationFieldsExtensions
    {
        public static int MinimumLength(this RegistrationFields field)
        {
            return field switch
            {
                RegistrationFields.UserName => 3,
                RegistrationFields.FirstName => 2,
                RegistrationFields.LastName => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(field)),
            };
        }

        public static int MaximumLength(this RegistrationFields field)
        {
            return field switch
            {
                RegistrationFields.UserName => 15,
                RegistrationFields.FirstName => 15,
                RegistrationFields.LastName => 1,
                RegistrationFields.Infix => 10,
                _ => throw new ArgumentOutOfRangeException(nameof(field)),
            };
        }

        public static string AllowedCharacters(this RegistrationFields field)
        {
            return field switch
            {
                RegistrationFields.PostalCode => "^[a-zA-Z0-9]+$",
                RegistrationFields.UserName => "^[a-zA-Z0-9]+$",
                _ => "^[a-zA-Z]+$",
            } ;
        }

        public static string Validate(string input, RegistrationFields field, IMatchingAppRepository repo)
        {
            var pattern = new Regex(AllowedCharacters(field));

            if (field.Equals(RegistrationFields.Infix) && input == String.Empty)
            {
                return input;
            }

            if (field.Equals(RegistrationFields.UserName) && repo.GetProfiles().Where(x => x.UserName == input).Count() > 0)
            {
                throw new FieldEmptyException(field);
            }

            if (!pattern.IsMatch(input))
            {
                throw new FieldEmptyException(field);
            }

            return input;

        }

        public static DateTime Validate(DateTime? input)
        {
            DateTime minimumBirthDate = DateTime.Today.AddYears(-18);

            if (input == null || minimumBirthDate.CompareTo(input) <= 0)
            {
                throw new FieldEmptyException(RegistrationFields.BirthDate);
            }
            return (DateTime)input;
        }

        public static Gender ValidateGender(List<bool?> radioButtons)
        {
            for (int i = 0; i < radioButtons.Count; i++)
            {
                if ((bool)radioButtons[i]) return (Gender)i;
            }
            throw new FieldEmptyException(RegistrationFields.Gender);
        }

        public static SexualPreference ValidateSexuality(List<bool?> radioButtons)
        {
            for (int i = 0; i < radioButtons.Count; i++)
            {
                if ((bool)radioButtons[i]) return (SexualPreference)i;
            }
            throw new FieldEmptyException(RegistrationFields.Sexuality);
        }
    }
}
