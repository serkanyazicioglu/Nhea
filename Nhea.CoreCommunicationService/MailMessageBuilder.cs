using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Nhea.Configuration.GenericConfigSection.Communication;
using System.Net.Mime;
using Nhea.Communication;
using MimeKit;
using System.Linq;

namespace Nhea.CoreCommunicationService
{
    public static class MailMessageBuilder
    {
        public static MailMessage Build(SmtpElement smtpElement, string toRecipients, string ccRecipients, string bccRecipients, string subject, string body, bool isHighPriority, List<Attachment> attachments)
        {
            MailMessage mailMessage = new();

            if (!string.IsNullOrEmpty(smtpElement.Sender))
            {
                mailMessage.From = new MailAddress(smtpElement.From, smtpElement.Sender);
            }
            else
            {
                mailMessage.From = new MailAddress(smtpElement.From);
            }

            if (string.IsNullOrEmpty(toRecipients) && string.IsNullOrEmpty(ccRecipients) && string.IsNullOrEmpty(bccRecipients))
            {
                toRecipients = smtpElement.DefaultToRecipients;

                if (string.IsNullOrEmpty(subject))
                {
                    subject = smtpElement.DefaultSubject;
                }
            }

            if (!string.IsNullOrEmpty(toRecipients))
            {
                foreach (MailAddress address in Nhea.Communication.MailMessageBuilder.ParseRecipients(toRecipients))
                {
                    mailMessage.To.Add(address);
                }
            }

            if (!string.IsNullOrEmpty(ccRecipients))
            {
                foreach (MailAddress address in Nhea.Communication.MailMessageBuilder.ParseRecipients(ccRecipients))
                {
                    mailMessage.CC.Add(address);
                }
            }

            if (!string.IsNullOrEmpty(bccRecipients))
            {
                foreach (MailAddress address in Nhea.Communication.MailMessageBuilder.ParseRecipients(bccRecipients))
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

            if (body.StartsWith(Nhea.Communication.MailQueue.NheaMailingStarter))
            {
                body = body.Replace(Nhea.Communication.MailQueue.NheaMailingStarter, string.Empty);

                var parameters = System.Text.Json.JsonSerializer.Deserialize<MailParameters>(body);

                if (!string.IsNullOrEmpty(parameters.PlainText) || smtpElement.AutoGeneratePlainText)
                {
                    if (string.IsNullOrEmpty(parameters.PlainText))
                    {
                        parameters.PlainText = Nhea.Text.HtmlHelper.GetPlainText(parameters.Body);
                    }

                    mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(parameters.PlainText, null, MediaTypeNames.Text.Plain));
                    mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(Nhea.Communication.MailMessageBuilder.ParseBody(parameters.Body), null, MediaTypeNames.Text.Html));
                }
                else
                {
                    mailMessage.Body = Nhea.Communication.MailMessageBuilder.ParseBody(parameters.Body);
                    mailMessage.IsBodyHtml = true;
                }

                if (parameters.IsBulkEmail)
                {
                    mailMessage.Headers.Add("Precedence", "bulk");
                }

                if (!string.IsNullOrEmpty(parameters.ListUnsubscribe))
                {
                    string listUnsubscribeHeader = parameters.ListUnsubscribe.Replace(" ", string.Empty);

                    if (!listUnsubscribeHeader.StartsWith('<'))
                    {
                        listUnsubscribeHeader = "<" + listUnsubscribeHeader;
                    }

                    if (!listUnsubscribeHeader.EndsWith('>'))
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
                mailMessage.Body = Nhea.Communication.MailMessageBuilder.ParseBody(body);
                mailMessage.IsBodyHtml = true;
            }

            return mailMessage;
        }

        public static MimeMessage ConvertToMimeMessage(this MailMessage mailMessage)
        {
            var mimeMessage = new MimeKit.MimeMessage
            {
                Sender = new MimeKit.MailboxAddress(mailMessage.From.DisplayName, mailMessage.From.Address)
            };
            mimeMessage.From.Add(mimeMessage.Sender);

            foreach (var mailAddress in mailMessage.To)
            {
                mimeMessage.To.Add(new MimeKit.MailboxAddress(string.Empty, mailAddress.Address));
            }

            foreach (var mailAddress in mailMessage.CC)
            {
                mimeMessage.To.Add(new MimeKit.MailboxAddress(string.Empty, mailAddress.Address));
            }

            foreach (var mailAddress in mailMessage.Bcc)
            {
                mimeMessage.Bcc.Add(new MimeKit.MailboxAddress(string.Empty, mailAddress.Address));
            }

            mimeMessage.Subject = mailMessage.Subject;
            mimeMessage.Priority = mailMessage.Priority == MailPriority.High ? MessagePriority.Urgent : MessagePriority.Normal;

            var builder = new MimeKit.BodyBuilder();

            if (mailMessage.IsBodyHtml)
            {
                builder.HtmlBody = mailMessage.Body;
            }
            else
            {
                builder.TextBody = mailMessage.Body;
            }

            if (mailMessage.Attachments != null && mailMessage.Attachments.Any())
            {
                foreach (var attachment in mailMessage.Attachments)
                {
                    builder.Attachments.Add(attachment.Name, attachment.ContentStream);
                }
            }

            mimeMessage.Body = builder.ToMessageBody();

            if (mailMessage.Headers != null)
            {
                foreach (string mailHeaderKey in mailMessage.Headers)
                {
                    mimeMessage.Headers.Add(new Header(mailHeaderKey, mailMessage.Headers[mailHeaderKey]));
                }
            }

            return mimeMessage;
        }
    }
}