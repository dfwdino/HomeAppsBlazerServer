using System.Globalization;

namespace HomeAppsBlazerServer.Components
{
    public static class MyExtensions
    {
        public static string ToTileCase(this string word)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(word);
        }
    }
}
