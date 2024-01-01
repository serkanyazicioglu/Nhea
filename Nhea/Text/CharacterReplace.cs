using System;
using System.Net;
using System.Linq;

namespace Nhea.Text
{
    internal static class CharacterReplace
    {
        internal static string ReplaceNonInvariantCharacters(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                text = text.Replace('İ', 'I').Replace('ı', 'i')
                .Replace('Ğ', 'G').Replace('ğ', 'g')
                .Replace('Ü', 'U').Replace('ü', 'u')
                .Replace('Ş', 'S').Replace('ş', 's')
                .Replace('Ö', 'O').Replace('ö', 'o')
                .Replace('Ç', 'C').Replace('ç', 'c')

                .Replace("À", "A").Replace("à", "a")
                .Replace("Á", "A").Replace("á", "a")
                .Replace("Â", "A").Replace("â", "a")
                .Replace("Ã", "A").Replace("ã", "a")
                .Replace("Ä", "A").Replace("ä", "a")
                .Replace("Å", "A").Replace("å", "a")

                .Replace("È", "E").Replace("è", "e")
                .Replace("É", "E").Replace("é", "e")
                .Replace("Ê", "E").Replace("ê", "e")
                .Replace("Ë", "E").Replace("ë", "e")

                .Replace("Ì", "I").Replace("ì", "i")
                .Replace("Í", "I").Replace("í", "i")
                .Replace("Î", "I").Replace("î", "i")
                .Replace("Ï", "I").Replace("ï", "i")
                .Replace("¡", "I")

                .Replace("Ò", "O").Replace("ò", "o")
                .Replace("Ó", "O").Replace("ó", "o")
                .Replace("Ô", "O").Replace("ô", "o")
                .Replace("Õ", "O").Replace("õ", "o")

                .Replace("Ù", "U").Replace("ù", "u")
                .Replace("Ú", "U").Replace("ú", "u")
                .Replace("Û", "U").Replace("Û", "u")

                .Replace("Ñ", "N").Replace("ñ", "n")
                .Replace("Ý", "Y").Replace("ý", "y")
                .Replace("Ÿ", "Y").Replace("ÿ", "y")
                .Replace("Þ", "P").Replace("þ", "p")
                .Replace("ß", "B")
                .Replace("Ø", "O").Replace("ø", "o")
                .Replace("¾", "3/4")
                .Replace("Æ", "AE").Replace("æ", "ae")
                .Replace("Ð", "D").Replace("ð", "d")
                .Replace("¿", "?");

                text = Nhea.Text.XmlHelper.CleanInvalidXmlChars(text);
                text = new string(text.Where(c => char.IsLetter(c) || char.IsDigit(c) || char.IsNumber(c) || char.IsPunctuation(c) || char.IsSeparator(c) || c == ' ').ToArray());
            }

            return text;
        }

        internal static string RemoveNumericCharacters(string text)
        {
            for (int i = 0; i < 10; i++)
            {
                text = text.Replace(i.ToString(), String.Empty);
            }

            return text;
        }

        internal static string RemoveLastCharacter(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                return text.Substring(0, text.Length - 1);
            }

            return text;
        }

        internal static string SplitText(string text, int length, string end)
        {
            if (!String.IsNullOrEmpty(text))
            {
                if (text.Length > length)
                {
                    int smallLength = length - end.Length;

                    text = WebUtility.HtmlDecode(text);

                    if (smallLength > 0)
                    {
                        text = text.Substring(0, smallLength) + end;
                    }
                    else
                    {
                        text = text.Substring(0, length);
                    }
                }
            }

            return text;
        }
    }
}
