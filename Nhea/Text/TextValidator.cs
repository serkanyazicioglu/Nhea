using System.Text.RegularExpressions;

namespace Nhea.Text
{
    internal static class TextValidator
    {
        private static readonly Regex AlphabeticRegex = new(@"^\D*[a-zA-Z]?$", RegexOptions.Compiled);

        private static readonly Regex NumericRegex = new(@"^[0-9]+$", RegexOptions.Compiled);

        private static readonly Regex SingleEmailRegex = new(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);

        private static readonly Regex MultipleEmailRegex = new(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*(\s*[,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*\s*", RegexOptions.Compiled);

        internal static bool IsAlphabetic(string text)
        {
            return ValidateText(text, AlphabeticRegex);
        }

        internal static bool IsNumeric(string text)
        {
            return ValidateText(text, NumericRegex);
        }

        internal static bool IsEmail(string text)
        {
            return IsEmail(text, false);
        }

        internal static bool IsEmail(string text, bool isMultiple)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            text = text.Trim();

            if (text.Contains(' '))
            {
                return false;
            }

            if (!isMultiple)
            {
                return ValidateText(text, SingleEmailRegex);
            }
            else
            {
                return ValidateText(text, MultipleEmailRegex);
            }
        }

        private static bool ValidateText(string text, Regex regex)
        {
            Match match = regex.Match(text);

            return match.Success;
        }
    }
}
