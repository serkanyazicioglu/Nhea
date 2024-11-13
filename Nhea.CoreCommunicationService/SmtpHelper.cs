using Nhea.Configuration.GenericConfigSection.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Nhea.CoreCommunicationService
{
    /// <summary>
    /// Manages e-mail sending processes.
    /// </summary>
    public sealed class SmtpHelper
    {
        public static SmtpElement SendMail(string from, string toRecipients, string subject, string body)
        {
            return SendMail(from, toRecipients, string.Empty, string.Empty, subject, body, false, null);
        }

        public static SmtpElement SendMail(string from, string toRecipients, string ccRecipients, string subject, string body)
        {
            return SendMail(from, toRecipients, ccRecipients, string.Empty, subject, body, false, null);
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
                from = Nhea.Text.StringHelper.TrimText(from, true, Text.TextCaseMode.lowercase);

                smtpElement = Nhea.Configuration.Settings.Communication.SmtpSettings.Where(smtpElementQuery => smtpElementQuery.From == from).FirstOrDefault();
            }
            else
            {
                smtpElement = Nhea.Configuration.Settings.Communication.SmtpSettings.Where(smtpElementQuery => smtpElementQuery.IsDefault == true).FirstOrDefault();
            }

            if (smtpElement != null)
            {
                var mailMessage = MailMessageBuilder.Build(smtpElement, toRecipients, ccRecipients, bccRecipients, subject, body, isHighPriority, attachments);

                if (string.IsNullOrEmpty(smtpElement.SmtpLibrary) || smtpElement.SmtpLibrary == "default")
                {
                    using (var smtpClient = SmtpClientBuilder.Build(smtpElement))
                    {
                        smtpClient.Send(mailMessage);
                    }
                }
                else if (smtpElement.SmtpLibrary == "mailkit")
                {
                    using (var smtpClient = SmtpClientBuilder.BuildForMailKit(smtpElement))
                    {
                        try
                        {
                            smtpClient.Send(mailMessage.ConvertToMimeMessage());
                        }
                        finally
                        {
                            if (smtpClient.IsConnected)
                            {
                                smtpClient.Disconnect(true);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Smtp library not found! Library: " + smtpElement.SmtpLibrary);
                }

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