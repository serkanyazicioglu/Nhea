using System;
using System.Net.Mail;

namespace Nhea.Communication
{
    public static class MailMessageBuilder
    {
        public static string ParseBody(string body)
        {
            if (!string.IsNullOrEmpty(body))
            {
                body = body.Replace(Environment.NewLine, string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\t", " ")
                    .Replace("﻿", string.Empty);

                while (body.Contains("  "))
                {
                    body = body.Replace("  ", " ");
                }
            }

            return body;
        }

        public static MailAddressCollection ParseRecipients(string address)
        {
            address = Nhea.Text.StringHelper.TrimText(address, true, Nhea.Text.TextCaseMode.lowercase).Trim(',').Trim(';');
            address = Nhea.Text.StringHelper.ReplaceNonInvariantCharacters(address);

            MailAddressCollection mailAddressCollection = new MailAddressCollection();

            char[] spliters = new char[] { ',', ';' };

            string[] mailArray = address.Split(spliters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string str in mailArray)
            {
                mailAddressCollection.Add(str);
            }

            return mailAddressCollection;
        }
    }
}
