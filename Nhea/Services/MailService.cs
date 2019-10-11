using Microsoft.Extensions.Options;
using Nhea.Communication;
using Nhea.Configuration;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public class MailService : IMailService
    {
        public MailService(IOptions<NheaCommunicationConfigurationSettings> nheaConfigurationSettings)
        {
            Settings.CurrentCommunicationConfigurationSettings = nheaConfigurationSettings.Value;
        }

        public bool Add(string from, string toRecipient, string subject, string body)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), null);
        }

        public bool Add(string from, string toRecipient, string subject, string body, MailQueueAttachment attachment)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), new List<MailQueueAttachment> { attachment });
        }

        public bool Add(string from, string toRecipient, string subject, string body, List<MailQueueAttachment> attachments)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), attachments);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string subject, string body)
        {
            return Add(from, toRecipient, ccRecipients, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), null);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(Priority.Medium), null);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<MailQueueAttachment> attachments)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(Priority.Medium), attachments);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, Priority priority)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(priority), null);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, DateTime priorityDate, List<MailQueueAttachment> attachments)
        {
            var mail = new Mail
            {
                From = from,
                ToRecipient = toRecipient,
                CcRecipients = ccRecipients,
                BccRecipients = bccRecipients,
                Subject = subject,
                Body = body,
                Priority = priorityDate,
                Attachments = attachments
            };

            return Add(mail);
        }

        public bool Add(Mail mail)
        {
            return MailQueue.Add(mail);
        }
    }
}
