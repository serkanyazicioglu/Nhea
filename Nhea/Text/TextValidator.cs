using System;
using System.Text.RegularExpressions;

namespace Nhea.Text
{
    internal static class TextValidator
    {
        internal const string AlphabeticPattern = @"^\D*[a-zA-Z]?$";

        internal const string NumericPattern = @"^[0-9]+$";

        internal const string SingleEmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        internal const string MultipleEmailPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*(\s*[,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*\s*";

        internal static bool IsAlphabetic(string text)
        {
            return ValidateText(text, AlphabeticPattern);
        }

        internal static bool IsNumeric(string text)
        {
            return ValidateText(text, NumericPattern);
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

            if (Nhea.Text.StringHelper.ReplaceNonInvariantCharacters(text) != text)
            {
                return false;
            }

            if (!isMultiple)
            {
                return ValidateText(text, SingleEmailPattern);
            }
            else
            {
                return ValidateText(text, MultipleEmailPattern);
            }
        }

        private static bool ValidateText(string text, string pattern)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(text);

            return match.Success;
        }
    }
}
