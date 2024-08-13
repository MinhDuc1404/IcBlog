using System.Text.RegularExpressions;

namespace IcBlogAdmin.Pages.Helper
{
    public static class StringExtensions
    {
        public static string StripHtmlAndTruncate(this string input, int wordLimit)
        {
            string text = Regex.Replace(input, "<.*?>", string.Empty);
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > wordLimit)
            {
                text = string.Join(" ", words.Take(wordLimit)) + "...";
            }

            return text;
        }
    }
}
