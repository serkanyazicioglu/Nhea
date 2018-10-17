using System;

namespace Nhea.Text
{
    public static class XmlHelper
    {
        /// <summary>
        /// Removes characters that are invalid for xml encoding.
        /// </summary>
        /// <param name="text">Text to be encoded.</param>
        /// <returns>Text with invalid xml characters removed.</returns>
        public static string CleanInvalidXmlChars(string text)
        {
            //From xml spec valid chars:
            //#x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]    
            //any Unicode character, excluding the surrogate blocks, FFFE, and FFFF.

            if (!String.IsNullOrEmpty(text))
            {
                string pattern = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
                text = System.Text.RegularExpressions.Regex.Replace(text, pattern, String.Empty);
            }

            return text;
        }
    }
}
