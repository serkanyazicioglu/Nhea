using System;
using System.Text;
using System.Globalization;

namespace Nhea.Text
{
    public enum TextCaseMode
    {
        None,
        CapitalizeEachWord,
        UPPERCASE,
        lowercase
    }

    internal static class TextCase
    {
        private static readonly char[] splitArray = new char[] { ' ' };

        private static readonly char[] capitalizeCharsSplitArray = new char[] { ' ', '?', '!', '.', '-', '&' };

        private const string CapitalizeChars = "ABCÇDEFGHIİJKLMNOÖPRSŞTUÜVYZQWXabcçdefghıijklmnoöprsştuüvyzqwx0123456789";

        internal static string ChangeCase(string text, TextCaseMode textCase, CultureInfo culture, bool capitalizeAfterNonAplhaNumericChars)
        {
            switch (textCase)
            {
                default:
                case TextCaseMode.None:
                    return text;
                case TextCaseMode.CapitalizeEachWord:
                    return Capitalize(text, culture, capitalizeAfterNonAplhaNumericChars);
                case TextCaseMode.lowercase:
                    return ToLowercase(text, culture);
                case TextCaseMode.UPPERCASE:
                    return ToUppercase(text, culture);
            }
        }

        internal static string Capitalize(string text, CultureInfo culture, bool capitalizeAfterNonAplhaNumericChars)
        {
            char[] split;

            if (capitalizeAfterNonAplhaNumericChars)
            {
                split = capitalizeCharsSplitArray;
            }
            else
            {
                split = splitArray;
            }

            text = text.ToLower(culture);

            string returnText = string.Empty;

            foreach (char ch in split)
            {
                StringBuilder capitalizedWords = new StringBuilder();

                string[] words = text.Split(new char[] { ch }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < words.Length; i++)
                {
                    for (int k = 0; k < words[i].Length; k++)
                    {
                        char letter = words[i][k];

                        if (CapitalizeChars.Contains(letter))
                        {
                            capitalizedWords.Append(words[i].Substring(k, 1).ToUpper(culture) + words[i].Substring(k + 1));
                            break;
                        }
                        else
                        {
                            capitalizedWords.Append(letter);
                        }
                    }

                    if (i < words.Length - 1)
                    {
                        capitalizedWords.Append(ch);
                    }
                }

                text = capitalizedWords.ToString();
            }

            return text;
        }

        internal static string ToUppercase(string text, CultureInfo culture)
        {
            string[] words = text.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder upperCasedWords = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                upperCasedWords.Append(words[i].ToUpper(culture));
                if (i < words.Length - 1)
                {
                    upperCasedWords.Append(" ");
                }
            }

            return upperCasedWords.ToString();
        }

        internal static string ToLowercase(string text, CultureInfo culture)
        {
            string[] words = text.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder lowerCasedWords = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                lowerCasedWords.Append(words[i].ToLower(culture));
                if (i < words.Length - 1)
                {
                    lowerCasedWords.Append(" ");
                }
            }

            return lowerCasedWords.ToString();
        }
    }
}
