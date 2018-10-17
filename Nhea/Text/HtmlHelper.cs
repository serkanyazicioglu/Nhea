using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Nhea.Text
{
    public static class HtmlHelper
    {
        public static string ClearHtml(string content)
        {
            return ClearHtml(content, false, true);
        }

        /// <summary>
        /// Removes all unnecessary Html tags including comments from the given content with returning plain text. Also cleans Office elements.
        /// </summary>
        /// <param name="content">Html content to be cleaned</param>
        /// <returns></returns>
        public static string ClearHtml(string content, bool clearStyles, bool clearTurkishCharacters)
        {
            if (String.IsNullOrEmpty(content))
            {
                return String.Empty;
            }

            content = WebUtility.HtmlDecode(content);

            content = Regex.Replace(content, "<!--(.*?)-->", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            content = Regex.Replace(content, "<font[^>]*>", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "</font>", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            content = Regex.Replace(content, "<o:p>", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "</o:p>", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, " style=\"\"", String.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<?xml:namespace prefix = o ns = \"urn:schemas-microsoft-com:office:office\" />", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<meta name=\"ProgId\" content=\"Word.Document\">", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<meta name=\"Generator\" content=\"Microsoft Word 12\">", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<meta name=\"Originator\" content=\"Microsoft Word 12\">", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, " class=MsoNormal", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, " v:shapes=\"[^>]*\"", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (clearStyles)
            {
                content = Regex.Replace(content, " style='[^>]*'", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            //if (cleanTags)
            {
                //content = Regex.Replace(content, " style='[^>]*'", String.Empty, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);   
            }

            return content;
        }

        public static string CreatePermaUrl(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                text = text.Trim();
                text = Nhea.Text.StringHelper.ChangeCase(text, Nhea.Text.TextCaseMode.lowercase);
                text = Nhea.Text.StringHelper.ReplaceNonInvariantCharacters(text);

                string[] badChars = new string[] { ",", ".", ":", ";", "?", "!", "(", ")", "[", "]", "{", "}", "\"", "@", "#", "$", "½", "₺", "€", "'", "^", "&", "/", "\\", "%", "*", "=" };

                foreach (var charc in badChars)
                {
                    if (text.Contains(charc))
                    {
                        text = text.Replace(charc, String.Empty);
                    }
                }

                text = text.Replace(" - ", "-");
                text = text.Replace(' ', '-');

                while (text.Contains("--"))
                {
                    text = text.Replace("--", "-");
                }

                return text;
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// Removes all Html tags and converts all Html characters with returning plain text.
        /// </summary>
        /// <param name="content">Html content to be converted.</param>
        /// <returns></returns>
        public static string GetPlainText(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return String.Empty;
            }

            string result;

            try
            {
                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = content.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);

                // Remove repeating spaces because browsers ignore them
                result = Regex.Replace(result, @"( )+", " ");

                if (result.EndsWith("</body></head>"))
                {
                    result = result.Remove(result.Length - ("</head>".Length));
                }

                // Remove the header (prepare first by clearing attributes)
                result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
                //result = Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);

                // Clear HTML
                result = ClearHtml(result);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\t)", "\t\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\t)( )+(\r)", "\t\r", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(\r)( )+(\t)", "\r\t", RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = Regex.Replace(result, "(\r)(\t)+", "\r\t", RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                return result;
            }
            catch
            {
                return content;
            }
        }

        ///// <summary>
        ///// Replaces all Html characters to text.
        ///// </summary>
        ///// <param name="content">Html content to be converted.</param>
        ///// <returns></returns>
        //public static string ReplaceSpecialCharacters(string content)
        //{
        //    return ReplaceSpecialCharacters(content, true);
        //}

        ///// <summary>
        ///// Replaces all special Html characters to symbols.
        ///// </summary>
        ///// <param name="content">Html content to be converted.</param>
        ///// <param name="replaceUnidentifiedCharacters">If true replaces all unidentified characters with String.Empty.</param>
        ///// <returns></returns>
        //public static string ReplaceSpecialCharacters(string content, bool replaceUnidentifiedCharacters)
        //{
        //    if (String.IsNullOrEmpty(content))
        //    {
        //        return String.Empty;
        //    }

        //    content = Regex.Replace(content, @"&nbsp;", " ", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&bull;", " * ", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&lsaquo;", "<", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&rsaquo;", ">", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&trade;", "(tm)", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&frasl;", "/", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&ldquo;", @"""", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&rdquo;", @"""", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&lt;", "<", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&gt;", ">", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&copy;", "(c)", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&reg;", "(r)", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&amp;", "&", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&quot;", "\"", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&hellip;", "...", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&#133;", "...");
        //    content = Regex.Replace(content, @"&#32;", " ");
        //    content = Regex.Replace(content, @"&#33;", "!");
        //    content = Regex.Replace(content, @"&#34;", "\"");
        //    content = Regex.Replace(content, @"&#35;", "#");
        //    content = Regex.Replace(content, @"&#36;", "$");
        //    content = Regex.Replace(content, @"&#37;", "%");
        //    content = Regex.Replace(content, @"&#38;", "&");
        //    content = Regex.Replace(content, @"&#39;", "'");
        //    content = Regex.Replace(content, @"&#039;", "'");
        //    content = Regex.Replace(content, @"&rsquo;", "'", RegexOptions.IgnoreCase);
        //    content = Regex.Replace(content, @"&#146;", "’");
        //    content = Regex.Replace(content, @"&#40;", "(");
        //    content = Regex.Replace(content, @"&#41;", ")");
        //    content = Regex.Replace(content, @"&#42;", "*");
        //    content = Regex.Replace(content, @"&#43;", "+");
        //    content = Regex.Replace(content, @"&#44;", ",");
        //    content = Regex.Replace(content, @"&#45;", "-");
        //    content = Regex.Replace(content, @"&#47;", "/");

        //    //Turkish chars
        //    content = Regex.Replace(content, @"&#231;", "ç");
        //    content = Regex.Replace(content, @"&ccedil;", "ç");
        //    content = Regex.Replace(content, @"&#199;", "Ç");
        //    content = Regex.Replace(content, @"&Ccedil;", "Ç");

        //    content = Regex.Replace(content, @"&#252; ", "ü");
        //    content = Regex.Replace(content, @"&uuml;", "ü");
        //    content = Regex.Replace(content, @"&#220;", "Ü");
        //    content = Regex.Replace(content, @"&Uuml;", "Ü");

        //    content = Regex.Replace(content, @"&#246;", "ö");
        //    content = Regex.Replace(content, @"&ouml;", "ö");
        //    content = Regex.Replace(content, @"&#214;", "Ö");
        //    content = Regex.Replace(content, @"&Ouml;", "Ö");

        //    content = Regex.Replace(content, @"&#350;", "Ş");
        //    content = Regex.Replace(content, @"&#351;", "ş");

        //    content = Regex.Replace(content, @"&#304;", "İ");
        //    content = Regex.Replace(content, @"&#305;", "ı");

        //    content = Regex.Replace(content, @"&#286;", "Ğ");
        //    content = Regex.Replace(content, @"&#287;", "ğ");


        //    if (replaceUnidentifiedCharacters)
        //    {
        //        // Remove all others.
        //        content = Regex.Replace(content, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);
        //    }

        //    return content;
        //}

        public static string ReplaceNewLineWithHtml(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                content = content.Replace(Environment.NewLine, "<br/>");
            }

            return content;
        }
    }
}
