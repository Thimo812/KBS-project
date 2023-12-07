using KBS_project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBS_project.Enums
{
    public enum Interest
    {
        Reizen, // Traveling
        Buitensport, // OutdoorActivities
        Fitness,
        Lezen, // Reading
        Muziek, // Music
        Films_en_TV, // MoviesTVShows
        Koken_en_bakken, // CookingBaking
        Kunst_en_creativiteit, // ArtCreativity
        Vrijwilligers, // VolunteerWork
        Gaming,
        Technologie, // TechnologyProgramming
        Sport,
        Dansen, // Dancing
        Talen, // Languages
        Dieren, // Animals
        Tuinieren, // Gardening
        Mode_en_stijl, // FashionStyle
        DIY_Projecten, // DIYProjects
        Wetenschap, // ScienceTechnology
        Bordspellen, // BoardGamesCardGames
        Astrology, // AstrologySpirituality
        TheaterKunst, // TheaterPerformingArts
        Sciencefiction, // ScienceFictionFantasy
        Koffie, // CoffeeEnthusiast
        Thee, // TeaEnthusiast
        Motorrijden, // Motorcycling
        Wijnproeven, // WineBeerTasting
        Vissen, // Fishing
        Geschiedenis, // HistoryBuff
        Schrijven_en_poëzie, // WritingPoetry
        Fietsen, // Cycling
        Skydiven, // SkydivingAdrenalineJunkie
        Verzamelen, // Collecting
        DIY_Huisdecoratie, // DIYHomeDecor
        Vechtsporten, // MartialArts
        Fotografie, // Photography
        Houtsnijden, // WhittlingWoodworking
        Gezondheid_en_welzijn, // HealthWellness
        Duurzaamheid, // Sustainability
        Astronomie // AstronomyStargazing
    }
}

public static class InterestExtensions
{
    public static int count = Enum.GetNames(typeof(Interest)).Length;

    public static string GetString(int index)
    {
        Interest interest = (Interest)index;

        string result = interest.ToString();

        return result.Replace('_', ' ');
    }
}
