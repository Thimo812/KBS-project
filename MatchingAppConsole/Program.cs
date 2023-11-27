using KBS_project;
using KBS_project.Enums;
using MatchingApp.DataAccess.SQL;

MatchingAppRepository matchingAppRepository = new MatchingAppRepository();

/*matchingAppRepository.GetProfiles(0, 18, 30, null, null, null, null);
*/
matchingAppRepository.UploadPic();
