using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nhea.Security
{
    public static class XssSecurity
    {
        private const string EmptyStringVBS = "\"\"";
        private const string EmptyStringJavaScript = "''";

        private static char[][] whitelistCodes = InitWhitelistCodes();

        private static char[][] InitWhitelistCodes()
        {
            char[][] allCharacters = new char[65536][];
            char[] thisChar;
            for (int i = 0; i < allCharacters.Length; i++)
            {
                if ((i >= 97 && i <= 122) ||        // a-z
                    (i >= 65 && i <= 90) ||         // A-Z
                    (i >= 48 && i <= 57) ||         // 0-9
                    i == 32 ||                      // space
                    i == 46 ||                      // .
                    i == 44 ||                      // ,
                    i == 45 ||                      // -
                    i == 95 ||                      // _
                    (i >= 256 && i <= 591) ||       // Latin,Extended-A,Latin Extended-B        
                    (i >= 880 && i <= 2047) ||      // Greek and Coptic,Cyrillic,Cyrillic Supplement,Armenian,Hebrew,Arabic,Syriac,Arabic,Supplement,Thaana,NKo
                    (i >= 2304 && i <= 6319) ||     // Devanagari,Bengali,Gurmukhi,Gujarati,Oriya,Tamil,Telugu,Kannada,Malayalam,Sinhala,Thai,Lao,Tibetan,Myanmar,eorgian,Hangul Jamo,Ethiopic,Ethiopic Supplement,Cherokee,Unified Canadian Aboriginal Syllabics,Ogham,Runic,Tagalog,Hanunoo,Buhid,Tagbanwa,Khmer,Mongolian   
                    (i >= 6400 && i <= 6687) ||     // Limbu, Tai Le, New Tai Lue, Khmer, Symbols, Buginese
                    (i >= 6912 && i <= 7039) ||     // Balinese         
                    (i >= 7680 && i <= 8191) ||     // Latin Extended Additional, Greek Extended        
                    (i >= 11264 && i <= 11743) ||   // Glagolitic, Latin Extended-C, Coptic, Georgian Supplement, Tifinagh, Ethiopic Extended    
                    (i >= 12352 && i <= 12591) ||   // Hiragana, Katakana, Bopomofo       
                    (i >= 12688 && i <= 12735) ||   // Kanbun, Bopomofo Extended        
                    (i >= 12784 && i <= 12799) ||   // Katakana, Phonetic Extensions         
                    (i >= 40960 && i <= 42191) ||   // Yi Syllables, Yi Radicals        
                    (i >= 42784 && i <= 43055) ||   // Latin Extended-D, Syloti, Nagri        
                    (i >= 43072 && i <= 43135) ||   // Phags-pa         
                    (i >= 44032 && i <= 55215) ||   // Hangul Syllables 
                    (i >= 19968 && i <= 40899))     // Mixed japanese/chinese/korean
                {
                    allCharacters[i] = null;
                }
                else
                {
                    string indexString = i.ToString();
                    int indexStringLen = indexString.Length;
                    thisChar = new char[indexStringLen];     // everything else
                    for (int j = 0; j < indexStringLen; j++)
                    {
                        thisChar[j] = indexString[j];
                    }

                    allCharacters[i] = thisChar;
                }
            }
            return allCharacters;
        }

        public static string HtmlEncode(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Use a new char array.
            int len = 0;
            int inputLength = input.Length;
            char[] returnMe = new char[inputLength * 8];
            char[] thisChar;
            int thisCharID;
            for (int i = 0; i < inputLength; i++)
            {
                thisCharID = (int)input[i];

                if (whitelistCodes[thisCharID] != null)
                {
                    // character needs to be encoded
                    thisChar = whitelistCodes[thisCharID];
                    returnMe[len++] = '&';
                    returnMe[len++] = '#';
                    for (int j = 0; j < thisChar.Length; j++)
                    {
                        returnMe[len++] = thisChar[j];
                    }
                    returnMe[len++] = ';';
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }
            return new string(returnMe, 0, len);
        }

        public static string UrlEncode(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Use a new char array.
            int len = 0;
            int inputLength = input.Length;
            int thisCharID;

            string thisChar;
            char ch;
            Encoding inputEncoding = null;

            // Use a new char array.
            char[] returnMe = new char[inputLength * 24];

            for (int i = 0; i < inputLength; i++)
            {
                thisCharID = (int)input[i];
                thisChar = input[i].ToString();
                //escaping SPACE and COMMA for URL Encoding
                if ((whitelistCodes[thisCharID] != null) || (thisCharID == 32) || (thisCharID == 44))
                {
                    // Character needs to be encoded to default UTF-8.
                    inputEncoding = Encoding.UTF8;
                    byte[] inputEncodingBytes = inputEncoding.GetBytes(thisChar);
                    int noinputEncodingBytes = inputEncodingBytes.Length;
                    for (int index = 0; index < noinputEncodingBytes; index++)
                    {
                        ch = (char)inputEncodingBytes[index];

                        // character needs to be encoded. Infact the byte cannot be greater than 256.
                        if (ch <= 256)
                        {
                            returnMe[len++] = '%';
                            string hex = ((int)ch).ToString("x").PadLeft(2, '0');
                            returnMe[len++] = hex[0];
                            returnMe[len++] = hex[1];
                        }
                    }
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }
            return new string(returnMe, 0, len);
        }

        public static string UrlEncode(string input, int codepage)
        {
            if (String.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            int len = 0;
            int thisCharID;
            int inputLength = input.Length;

            char ch;
            string thisChar;
            Encoding inputEncoding = null;

            // Use a new char array.
            char[] returnMe = new char[inputLength * 24]; // worst case length scenario            

            for (int i = 0; i < inputLength; i++)
            {
                thisCharID = (int)input[i];
                thisChar = input[i].ToString();

                //escaping SPACE and COMMA for URL Encoding
                if ((whitelistCodes[thisCharID] != null) || (thisCharID == 32) || (thisCharID == 44))
                {
                    // character needs to be encoded
                    inputEncoding = Encoding.GetEncoding(codepage);
                    byte[] inputEncodingBytes = inputEncoding.GetBytes(thisChar);
                    int noinputEncodingBytes = inputEncodingBytes.Length;
                    for (int index = 0; index < noinputEncodingBytes; index++)
                    {
                        ch = (char)inputEncodingBytes[index];

                        // character needs to be encoded. Infact the byte cannot be greater than 256.
                        if (ch <= 256)
                        {
                            returnMe[len++] = '%';
                            string hex = ((int)ch).ToString("x").PadLeft(2, '0');
                            returnMe[len++] = hex[0];
                            returnMe[len++] = hex[1];
                        }
                    }
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }

            return new string(returnMe, 0, len);
        }

        public static string JavaScriptEncode(string input)
        {
            return JavaScriptEncode(input, true);
        }

        public static string JavaScriptEncode(string input, bool flagforQuote)
        {
            // Input validation: empty or null string condition
            if (String.IsNullOrEmpty(input))
            {
                if (flagforQuote)
                {
                    return (EmptyStringJavaScript);
                }
                else
                {
                    return String.Empty;
                }
            }

            // Use a new char array.
            int len = 0;
            int inputLength = input.Length;
            char[] returnMe = new char[inputLength * 8]; // worst case length scenario
            char[] thisChar;
            char ch;
            int thisCharID;

            // First step is to start the encoding with an apostrophe if flag is true.
            if (flagforQuote)
            {
                returnMe[len++] = '\'';
            }

            for (int i = 0; i < inputLength; i++)
            {
                thisCharID = (int)input[i];
                ch = input[i];
                if (whitelistCodes[thisCharID] != null)
                {
                    // character needs to be encoded
                    thisChar = whitelistCodes[thisCharID];
                    if (thisCharID > 127)
                    {
                        returnMe[len++] = '\\';
                        returnMe[len++] = 'u';
                        string hex = ((int)ch).ToString("x").PadLeft(4, '0');
                        returnMe[len++] = hex[0];
                        returnMe[len++] = hex[1];
                        returnMe[len++] = hex[2];
                        returnMe[len++] = hex[3];
                    }
                    else
                    {
                        returnMe[len++] = '\\';
                        returnMe[len++] = 'x';
                        string hex = ((int)ch).ToString("x").PadLeft(2, '0');
                        returnMe[len++] = hex[0];
                        returnMe[len++] = hex[1];
                    }
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }

            // Last step is to end the encoding with an apostrophe if flag is true.
            if (flagforQuote)
            {
                returnMe[len++] = '\'';
            }

            return new string(returnMe, 0, len);
        }
    }
}
