using Microsoft.Extensions.Options;
using Nhea.Configuration;
using System;
using System.Collections.Generic;

namespace Nhea.Communication
{
    public class MailService : IMailService
    {
        private readonly NheaConfigurationSettings _nheaConfigurationSettings;
        public MailService(IOptions<NheaConfigurationSettings> nheaConfigurationSettings)
        {
            _nheaConfigurationSettings = nheaConfigurationSettings.Value;

            if (!String.IsNullOrEmpty(_nheaConfigurationSettings.CommunicationConnectionString))
            {
                MailQueue.ConnectionString = _nheaConfigurationSettings.CommunicationConnectionString;
            }
            else if (!String.IsNullOrEmpty(_nheaConfigurationSettings.DataConnectionString))
            {
                MailQueue.ConnectionString = _nheaConfigurationSettings.DataConnectionString;
            }
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
