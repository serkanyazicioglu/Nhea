using Nhea.Configuration.GenericConfigSection.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Nhea.Communication
{
    /// <summary>
    /// Manages e-mail sending processes.
    /// </summary>
    public sealed class SmtpHelper
    {
        public static SmtpElement SendMail(string from, string toRecipients, string subject, string body)
        {
            return SendMail(from, toRecipients, String.Empty, String.Empty, subject, body, false, null);
        }

        public static SmtpElement SendMail(string from, string toRecipients, string ccRecipients, string subject, string body)
        {
            return SendMail(from, toRecipients, ccRecipients, String.Empty, subject, body, false, null);
        }

        public static SmtpElement SendMail(string from, string toRecipients, string ccRecipients, string bccRecipients, string subject, string body)
        {
            return SendMail(from, toRecipients, ccRecipients, bccRecipients, subject, body, false, null);
        }

        public static SmtpElement SendMail(string from, string toRecipients, string ccRecipients, string bccRecipients, string subject, string body, bool isHighPriority, List<Attachment> attachments)
        {
            SmtpElement smtpElement;

            if (!string.IsNullOrEmpty(from))
            {
                from = Nhea.Text.StringHelper.ReplaceNonInvariantCharacters(Nhea.Text.StringHelper.TrimText(from, true, Text.TextCaseMode.lowercase));

                smtpElement = Nhea.Configuration.Settings.Communication.SmtpSettings.Where(smtpElementQuery => smtpElementQuery.From == from).First();
            }
            else
            {
                smtpElement = Nhea.Configuration.Settings.Communication.SmtpSettings.Where(smtpElementQuery => smtpElementQuery.IsDefault == true).First();
            }

            if (smtpElement != null)
            {
                MailMessage mailMessage = MailMessageBuilder.Build(smtpElement, toRecipients, ccRecipients, bccRecipients, subject, body, isHighPriority, attachments);

                SmtpClient smtpClient = SmtpClientBuilder.Build(smtpElement);
                smtpClient.Send(mailMessage);

                return smtpElement;
            }
            else
            {
                if (!string.IsNullOrEmpty(from))
                {
                    throw new Exception("Related smtp setting could not found! From: " + from);
                }
                else
                {
                    throw new Exception("Default smtp setting could not found!");
                }
            }
        }
    }
}