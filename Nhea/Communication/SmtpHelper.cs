using System;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using Nhea.Configuration.GenericConfigSection.Communication;
using System.Linq;
using System.Configuration;

namespace Nhea.Communication
{
    /// <summary>
    /// Email gönderme iþlerini yönetir
    /// </summary>
    public sealed class SmtpHelper
    {
        private SmtpHelper()
        {
            //only has static methods
        }

        public static void SendMail(string from, string toRecipients, string subject, string body)
        {
            SendMail(from, toRecipients, String.Empty, String.Empty, subject, body, false, null);
        }

        public static void SendMail(string from, string toRecipients, string ccRecipients, string subject, string body)
        {
            SendMail(from, toRecipients, ccRecipients, String.Empty, subject, body, false, null);
        }

        public static void SendMail(string from, string toRecipients, string ccRecipients, string bccRecipients, string subject, string body)
        {
            SendMail(from, toRecipients, ccRecipients, bccRecipients, subject, body, false, null);
        }

        public static void SendMail(string from, string toRecipients, string ccRecipients, string bccRecipients, string subject, string body, bool isHighPriority, List<Attachment> attachments)
        {
            SmtpElement smtpElement;

            if (!String.IsNullOrEmpty(from))
            {
                from = Nhea.Text.StringHelper.ReplaceTurkishCharacters(from.ToLower().Trim());

                smtpElement = Nhea.Configuration.Settings.Communication.SmtpSettings.Where(smtpElementQuery => smtpElementQuery.From == from).First();
            }
            else
            {
                smtpElement = Nhea.Configuration.Settings.Communication.SmtpSettings.Where(smtpElementQuery => smtpElementQuery.IsDefault == true).First();
            }

            MailMessage mailMessage = MailMessageBuilder.Build(smtpElement, toRecipients, ccRecipients, bccRecipients, subject, body, isHighPriority, attachments);

            SmtpClient smtpClient = SmtpClientBuilder.Build(smtpElement);
            smtpClient.Send(mailMessage);
        }
    }
}