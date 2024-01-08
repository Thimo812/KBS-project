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
        Astrology,
        Astronomie,
        Bordspellen,
        Buitensport,
        Dansen,
        Dieren,
        DIY_Huisdecoratie,
        DIY_Projecten,
        Duurzaamheid,
        Fietsen,
        Films_en_TV,
        Fitness,
        Fotografie,
        Gaming,
        Gezondheid_en_welzijn,
        Geschiedenis,
        Houtsnijden,
        Koken_en_bakken,
        Koffie,
        Kunst_en_creativiteit,
        Lezen,
        Mode_en_stijl,
        Motorrijden,
        Muziek,
        Reizen,
        Schrijven_en_poëzie,
        Sciencefiction,
        Skydiven,
        Sport,
        Talen,
        Technologie,
        TheaterKunst,
        Thee,
        Tuinieren,
        Verzamelen,
        Vechtsporten,
        Vissen,
        Vrijwilligers,
        Wetenschap,
        Wijnproeven
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

    public static Interest getEnumFromString(string interestString)
    {
        interestString = interestString.Replace(' ', '_');

        Interest interest;

        if (Enum.TryParse(interestString, out interest)) return interest;
        
        throw new InvalidCastException(interestString);
    }
}
