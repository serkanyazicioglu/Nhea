using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Nhea.Text.Password;

namespace Nhea.Text
{
    public static class StringHelper
    {
        /// <summary>
        /// Replaces all non-invariant characters to the invariant characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceNonInvariantCharacters(string text)
        {
            return CharacterReplace.ReplaceNonInvariantCharacters(text);
        }

        /// <summary>
        /// Replaces all numeric characters from the given text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveNumericCharacters(string text)
        {
            return CharacterReplace.RemoveNumericCharacters(text);
        }

        /// <summary>
        /// Removes the last character of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveLastCharacter(string text)
        {
            return CharacterReplace.RemoveLastCharacter(text);
        }

        /// <summary>
        /// Trims and changes casing of the given text with current culture.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textCase"></param>
        /// <returns></returns>
        public static string ChangeCase(string text, TextCaseMode textCase)
        {
            return TextCase.ChangeCase(text, textCase, CultureInfo.CurrentCulture, false);
        }

        /// <summary>
        /// Trims and changes casing of the given text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textCase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ChangeCase(string text, TextCaseMode textCase, CultureInfo culture)
        {
            return TextCase.ChangeCase(text, textCase, culture, false);
        }

        /// <summary>
        /// Trims and capitalizes the given text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textCase"></param>
        /// <returns></returns>
        public static string ChangeCase(string text, bool capitalizeAfterNonAlphaNumericChars)
        {
            return TextCase.ChangeCase(text, TextCaseMode.CapitalizeEachWord, CultureInfo.CurrentCulture, capitalizeAfterNonAlphaNumericChars);
        }

        /// <summary>
        /// Trims and capitalizes the given text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textCase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ChangeCase(string text, bool capitalizeAfterNonNumericChars, CultureInfo culture)
        {
            return TextCase.ChangeCase(text, TextCaseMode.CapitalizeEachWord, culture, capitalizeAfterNonNumericChars);
        }

        /// <summary>
        /// Splits the given the text with the specified length.
        /// </summary>
        /// <param name="text">String to be shortened.</param>
        /// <param name="length">Length of the new string.</param>
        /// <returns></returns>
        public static string SplitText(string text, int length)
        {
            return CharacterReplace.SplitText(text, length, "...");
        }

        /// <summary>
        /// Splits the given the text with the specified length.
        /// </summary>
        /// <param name="text">String to be shortened.</param>
        /// <param name="length">Length of the new string.</param>
        /// <param name="end">Inserts to the end of result text. Default is '...'.</param>
        /// <returns></returns>
        public static string SplitText(string text, int length, string end)
        {
            return CharacterReplace.SplitText(text, length, end);
        }

        /// <summary>
        /// Validates text if it contains only numbers.
        /// </summary>
        /// <param name="text">String to be validated.</param>
        /// <returns></returns>
        public static bool IsNumeric(string text)
        {
            return TextValidator.IsNumeric(text);
        }

        /// <summary>
        /// Validates text if it contains only letters.
        /// </summary>
        /// <param name="text">String to be validated.</param>
        /// <returns></returns>
        public static bool IsAlphabetic(string text)
        {
            return TextValidator.IsAlphabetic(text);
        }

        /// <summary>
        /// Validates text if it is in a valid e-mail format.
        /// </summary>
        /// <param name="text">String to be validated.</param>
        /// <returns></returns>
        public static bool IsEmail(string text)
        {
            return TextValidator.IsEmail(text);
        }

        /// <summary>
        /// Validates text if it is in a valid e-mail format.
        /// </summary>
        /// <param name="text">String to be validated.</param>
        /// <param name="isMultiple">When true e-mail can be ',' or ';' seperated.</param>
        /// <returns></returns>
        public static bool IsEmail(string text, bool isMultiple)
        {
            return TextValidator.IsEmail(text, isMultiple);
        }

        public static string GeneratePassword(int passwordLength)
        {
            return GeneratePassword(passwordLength, "QWERTYUIOPASDFGHJKLZXCVBNM1234567890", null, 1)[0];
        }

        public static string GeneratePassword(int passwordLength, string charSet)
        {
            return GeneratePassword(passwordLength, charSet, null, 1)[0];
        }

        public static List<string> GeneratePassword(int passwordLength, string charSet, int totalCount)
        {
            return GeneratePassword(passwordLength, charSet, null, totalCount);
        }

        public static List<string> GeneratePassword(int passwordLength, string charSet, List<ConstantChar> constantChars, int totalCount)
        {
            PasswordGenerator passwordGenerator = new PasswordGenerator();
            passwordGenerator.CharSet = charSet;
            passwordGenerator.ConstantChars = constantChars;
            passwordGenerator.PasswordLength = passwordLength;
            passwordGenerator.TotalCount = totalCount;

            List<string> passwordList = new List<string>();

            while (passwordList.Count < totalCount)
            {
                string password = passwordGenerator.CreatePassword();

                if (!passwordList.Contains(password))
                {
                    passwordList.Add(password);
                }
            }

            return passwordList;
        }

        public static string TrimText(string text)
        {
            return TrimText(text, false, TextCaseMode.None);
        }

        public static string TrimText(string text, bool removeAllWhitespaces)
        {
            return TrimText(text, removeAllWhitespaces, TextCaseMode.None);
        }

        public static string TrimText(string text, bool removeAllWhitespaces, TextCaseMode textCase)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Trim().Trim(Environment.NewLine.ToCharArray()).Replace("﻿", string.Empty);

                if (removeAllWhitespaces)
                {
                    //text = text.Replace(" ", string.Empty);
                    text = Regex.Replace(text, @"\s+", string.Empty);
                }

                if (textCase != TextCaseMode.None)
                {
                    text = ChangeCase(text, textCase);
                }
            }

            return text;
        }
    }
}
