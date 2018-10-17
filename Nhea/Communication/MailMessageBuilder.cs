using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Nhea.Configuration.GenericConfigSection.Communication;

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
            mailMessage.Subject = subject.Replace('\r', ' ').Replace('\n', ' ').Replace(Environment.NewLine, String.Empty);
            mailMessage.Body = body;

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

            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            return mailMessage;
        }

        private static MailAddressCollection ParseRecipients(string mailAddress)
        {
            mailAddress = mailAddress.Trim().Trim(',').Trim(';');

            MailAddressCollection mailAddressCollection = new MailAddressCollection();

            char[] spliters = new char[] { ',', ';' };

            string[] mailArray = mailAddress.Split(spliters, StringSplitOptions.RemoveEmptyEntries);

            if (mailArray.Length > 1)
            {
                foreach (string str in mailArray)
                {
                    mailAddressCollection.Add(str);
                }
            }
            else
            {
                mailAddressCollection.Add(mailAddress);
            }

            return mailAddressCollection;
        }
    }
}
