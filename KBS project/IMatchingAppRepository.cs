using KBS_project.Enums;
using KBS_project.Enums.FilterOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBS_project
{
	public interface IMatchingAppRepository
	{
		public List<Profile> GetProfiles();

		public List<string> GetProfiles(LocationFilter location, int minimumAge, int maximumAge,
			List<Interest> includedHobbys, List<Interest> excludedHobbys, List<Diet> includedDiets, List<Diet> excludedDiets);

		public Profile GetProfile(string userName);

		public void SaveProfile( Profile profile);

		public void StoreImages(Profile profile);

		public void SaveMatchingQuiz(List<int> answers);
        bool IsValidUsername(string username);

    }
}
