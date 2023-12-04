using KBS_project;
using KBS_project.Enums;
using MatchingApp.DataAccess.SQL;

var repo = new MatchingAppRepository();

Profile profile = repo.GetProfile("Thimo812");

Console.WriteLine(profile);



