using System.Globalization;

namespace HomeAppsBlazerServer.Components.Extensions
{
    public static class MyExtensions
    {
        public static string ToTileCase(this string word,bool skipalluppercase = false)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            word = skipalluppercase ?  word : word.ToLower();

            return textInfo.ToTitleCase(word);
        }

        public static DateTime GetNextSaturday(DateTime fromDate)
        {
            int daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)fromDate.DayOfWeek + 7) % 7;
            daysUntilSaturday = daysUntilSaturday == 0 ? 7 : daysUntilSaturday;
            return fromDate.AddDays(daysUntilSaturday);
        }
    }
}
