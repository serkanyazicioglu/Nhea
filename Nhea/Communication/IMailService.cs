using System;
using System.Collections.Generic;

namespace Nhea.Communication
{
    public interface IMailService
    {
        bool Add(string from, string toRecipient, string subject, string body);

        bool Add(string from, string toRecipient, string subject, string body, MailQueueAttachment attachment);

        bool Add(string from, string toRecipient, string subject, string body, List<MailQueueAttachment> attachments);

        bool Add(string from, string toRecipient, string ccRecipients, string subject, string body);

        bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body);

        bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, 
            string listUnsubscribe = null,
            string plainText = null,
            bool isBulkEmail = false,
            bool unsubscribeOneClick = false);

        bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<MailQueueAttachment> attachments);

        bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<MailQueueAttachment> attachments,
            string listUnsubscribe = null,
            string plainText = null,
            bool isBulkEmail = false,
            bool unsubscribeOneClick = false);

        bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, Priority priority);

        bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, DateTime priorityDate, List<MailQueueAttachment> attachments);

        bool Add(Mail mail);
    }
}
