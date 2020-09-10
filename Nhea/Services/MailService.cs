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
            return AddCore(from, toRecipient, String.Empty, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), null);
        }

        public bool Add(string from, string toRecipient, string subject, string body, MailQueueAttachment attachment)
        {
            return AddCore(from, toRecipient, String.Empty, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), new List<MailQueueAttachment> { attachment });
        }

        public bool Add(string from, string toRecipient, string subject, string body, List<MailQueueAttachment> attachments)
        {
            return AddCore(from, toRecipient, String.Empty, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), attachments);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string subject, string body)
        {
            return AddCore(from, toRecipient, ccRecipients, String.Empty, subject, body, MailQueue.GetDateByPriority(Priority.Medium), null);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body)
        {
            return AddCore(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(Priority.Medium), null);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, string listUnsubscribe = null, string plainText = null, bool isBulkEmail = false, bool unsubscribeOneClick = false)
        {
            return AddCore(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(Priority.Medium), null,
                listUnsubscribe: listUnsubscribe,
                plainText: plainText,
                isBulkEmail: isBulkEmail,
                unsubscribeOneClick: unsubscribeOneClick);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<MailQueueAttachment> attachments)
        {
            return AddCore(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(Priority.Medium), attachments);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<MailQueueAttachment> attachments, string listUnsubscribe = null, string plainText = null, bool isBulkEmail = false, bool unsubscribeOneClick = false)
        {
            return AddCore(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(Priority.Medium), attachments,
                listUnsubscribe: listUnsubscribe,
                plainText: plainText,
                isBulkEmail: isBulkEmail,
                unsubscribeOneClick: unsubscribeOneClick);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, Priority priority)
        {
            return AddCore(from, toRecipient, ccRecipients, bccRecipients, subject, body, MailQueue.GetDateByPriority(priority), null);
        }

        public bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, DateTime priorityDate, List<MailQueueAttachment> attachments)
        {
            return AddCore(from, toRecipient, ccRecipients, bccRecipients, subject, body, priorityDate, attachments);
        }

        private bool AddCore(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, DateTime priorityDate, List<MailQueueAttachment> attachments,
            string listUnsubscribe = null, string plainText = null, bool isBulkEmail = false, bool unsubscribeOneClick = false)
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
                Attachments = attachments,
                ListUnsubscribe = listUnsubscribe,
                PlainText = plainText,
                IsBulkEmail = isBulkEmail,
                UnsubscribeOneClick = unsubscribeOneClick
            };

            return Add(mail);
        }

        public bool Add(Mail mail)
        {
            return MailQueue.Add(mail);
        }
    }
}
