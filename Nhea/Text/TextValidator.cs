using System;
using System.Text.RegularExpressions;

namespace Nhea.Text
{
    internal static class TextValidator
    {
        private static Regex AlphabeticRegex = new Regex(@"^\D*[a-zA-Z]?$", RegexOptions.Compiled);

        private static Regex NumericRegex = new Regex(@"^[0-9]+$", RegexOptions.Compiled);

        private static Regex SingleEmailRegex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);

        private static Regex MultipleEmailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*(\s*[,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*\s*", RegexOptions.Compiled);

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
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            text = text.Trim();

            if (text.Contains(" "))
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
