using System.Text;

namespace BackendCRUD.ApiService.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveAccents(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string StemWord(this string word)
        {
            // Implementación básica de stemming en español
            if (word.EndsWith("ando") || word.EndsWith("iendo"))
                return word[..^4];
            if (word.EndsWith("ar") || word.EndsWith("er") || word.EndsWith("ir"))
                return word[..^2];
            if (word.EndsWith("ción"))
                return word[..^3] + "r";
            if (word.EndsWith("mente"))
                return word[..^5];
            return word;
        }
    }
}