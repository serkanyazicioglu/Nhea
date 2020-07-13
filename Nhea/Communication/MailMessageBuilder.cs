using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Nhea.Configuration.GenericConfigSection.Communication;
using System.Net.Mime;

namespace Nhea.Communication
{
    public static class MailMessageBuilder
    {
        public static MailMessage Build(SmtpElement smtpElement, string toRecipients, string ccRecipients, string bccRecipients, string subject, string body, bool isHighPriority, List<Attachment> attachments)
        {
            MailMessage mailMessage = new MailMessage();

            if (!String.IsNullOrEmpty(smtpElement.Sender))
            {
                mailMessage.From = new MailAddress(smtpElement.From, smtpElement.Sender);
            }
            else
            {
                mailMessage.From = new MailAddress(smtpElement.From);
            }

            if (String.IsNullOrEmpty(toRecipients) && String.IsNullOrEmpty(ccRecipients) && String.IsNullOrEmpty(bccRecipients))
            {
                toRecipients = smtpElement.DefaultToRecipients;

                if (String.IsNullOrEmpty(subject))
                {
                    subject = smtpElement.DefaultSubject;
                }
            }

            if (!String.IsNullOrEmpty(toRecipients))
            {
                foreach (MailAddress address in ParseRecipients(toRecipients))
                {
                    mailMessage.To.Add(address);
                }
            }

            if (!String.IsNullOrEmpty(ccRecipients))
            {
                foreach (MailAddress address in ParseRecipients(ccRecipients))
                {
                    mailMessage.CC.Add(address);
                }
            }

            if (!String.IsNullOrEmpty(bccRecipients))
            {
                foreach (MailAddress address in ParseRecipients(bccRecipients))
                {
                    mailMessage.Bcc.Add(address);
                }
            }

            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;
            mailMessage.Subject = subject.Replace('\r', ' ').Replace('\n', ' ').Replace(Environment.NewLine, " ");
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.BodyEncoding = Encoding.UTF8;

            if (isHighPriority)
            {
                mailMessage.Priority = MailPriority.High;
            }
            else
            {
                mailMessage.Priority = MailPriority.Normal;
            }

            if (attachments != null)
            {
                foreach (Attachment attachment in attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }

            if (body.StartsWith(MailQueue.NheaMailingStarter))
            {
                body = body.Replace(MailQueue.NheaMailingStarter, String.Empty);

                var parameters = System.Text.Json.JsonSerializer.Deserialize<MailParameters>(body);

                if (!string.IsNullOrEmpty(parameters.PlainText) || smtpElement.AutoGeneratePlainText)
                {
                    if (string.IsNullOrEmpty(parameters.PlainText))
                    {
                        parameters.PlainText = Nhea.Text.HtmlHelper.GetPlainText(parameters.Body);
                    }

                    mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(parameters.PlainText, null, MediaTypeNames.Text.Plain));
                    mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(ParseBody(parameters.Body), null, MediaTypeNames.Text.Html));
                }
                else
                {
                    mailMessage.Body = ParseBody(parameters.Body);
                    mailMessage.IsBodyHtml = true;
                }

                if (parameters.IsBulkEmail)
                {
                    mailMessage.Headers.Add("Precedence", "bulk");
                }

                if (!string.IsNullOrEmpty(parameters.ListUnsubscribe))
                {
                    string listUnsubscribeHeader = parameters.ListUnsubscribe.Replace(" ", string.Empty);

                    if (!listUnsubscribeHeader.StartsWith("<"))
                    {
                        listUnsubscribeHeader = "<" + listUnsubscribeHeader;
                    }

                    if (!listUnsubscribeHeader.EndsWith(">"))
                    {
                        listUnsubscribeHeader += ">";
                    }

                    mailMessage.Headers.Add("List-Unsubscribe", listUnsubscribeHeader);

                    if (parameters.UnsubscribeOneClick)
                    {
                        mailMessage.Headers.Add("List-Unsubscribe-Post", "List-Unsubscribe=One-Click");
                    }
                }
            }
            else
            {
                mailMessage.Body = ParseBody(body);
                mailMessage.IsBodyHtml = true;
            }

            return mailMessage;
        }

        private static string ParseBody(string body)
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
