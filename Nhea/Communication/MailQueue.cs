using Nhea.Logging;
using Nhea.Utils;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Nhea.Communication
{
    public static class MailQueue
    {
       
        private const string InsertCommandText = @"INSERT INTO nhea_MailQueue(MailProviderId, [From], [To], Cc, Bcc, Subject, Body, Priority, IsReadyToSend, HasAttachment) output INSERTED.Id VALUES(@MailProviderId, @From, @To, @Cc, @Bcc, @Subject, @Body, @PriorityDate, @IsReadyToSend, @HasAttachment);";

        private const string InsertAttachmentCommandText = @"INSERT INTO [nhea_MailQueueAttachment]([MailQueueId],[AttachmentName],[AttachmentData]) VALUES(@MailQueueId, @AttachmentName,@AttachmentData)";

        private const string UpdateStatusCommandText = @"UPDATE nhea_MailQueue SET IsReadyToSend = {0} WHERE Id = @MailQueueId";

        public static bool Add(string from, string toRecipient, string subject, string body)
        {
            return Add(from, toRecipient, string.Empty, string.Empty, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string subject, string body, MailQueueAttachment attachment, string listUnsubscribe = null, string plainText = null)
        {
            return Add(from, toRecipient, string.Empty, string.Empty, subject, body, GetDateByPriority(Priority.Medium), new List<MailQueueAttachment> { attachment }, listUnsubscribe: listUnsubscribe, plainText: plainText);
        }

        public static bool Add(string from, string toRecipient, string subject, string body, List<MailQueueAttachment> attachments, string listUnsubscribe = null, string plainText = null)
        {
            return Add(from, toRecipient, string.Empty, string.Empty, subject, body, GetDateByPriority(Priority.Medium), attachments, listUnsubscribe: listUnsubscribe, plainText: plainText);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string subject, string body)
        {
            return Add(from, toRecipient, ccRecipients, string.Empty, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, string listUnsubscribe = null, string plainText = null, bool isBulkEmail = false, bool unsubscribeOneClick = false)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(Priority.Medium), null, listUnsubscribe: listUnsubscribe, plainText: plainText, isBulkEmail: isBulkEmail, unsubscribeOneClick: unsubscribeOneClick);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<MailQueueAttachment> attachments, string listUnsubscribe = null, string plainText = null)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(Priority.Medium), attachments, listUnsubscribe: listUnsubscribe, plainText: plainText);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, Priority priority, string listUnsubscribe = null, string plainText = null)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(priority), null, listUnsubscribe: listUnsubscribe, plainText: plainText);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, DateTime priorityDate, List<MailQueueAttachment> attachments,
            string listUnsubscribe = null,
            string plainText = null,
            bool isBulkEmail = false,
            bool unsubscribeOneClick = false
            )
        {
            return Add(new Mail
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
            });
        }

        public delegate void MailQueueingEventHandler(Mail mail);

        public delegate void MailQueuedEventHandler(Mail mail, bool result);

        public static event MailQueueingEventHandler MailQueueing;

        public static event MailQueuedEventHandler MailQueued;

        public const string NheaMailingStarter = "|NHEA_MAILING|";

        public static bool Add(Mail mail)
        {
            var result = false;

            try
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                using (SqlCommand cmd = new(InsertCommandText, sqlConnection))
                {
                    cmd.Connection.Open();                     
                    bool hasAttachment = false;
                    
                    if (mail.Attachments != null && mail.Attachments.Count > 0)
                    {
                        hasAttachment = true;
                    }

                    mail.From = PrepareMailAddress(mail.From);
                    mail.ToRecipient = PrepareMailAddress(mail.ToRecipient);
                    mail.CcRecipients = PrepareMailAddress(mail.CcRecipients);
                    mail.BccRecipients = PrepareMailAddress(mail.BccRecipients);

                    if (MailQueueing != null)
                    {
                        var subs = MailQueueing.GetInvocationList();
                        foreach (MailQueueingEventHandler sub in subs)
                        {
                            sub.Invoke(mail);
                        }
                    }

                    string body = NheaMailingStarter;

                    MailParameters mailParameters = new()
                    {
                        Version = "2",
                        Body = mail.Body,
                        ListUnsubscribe = mail.ListUnsubscribe,
                        PlainText = mail.PlainText,
                        IsBulkEmail = mail.IsBulkEmail,
                        UnsubscribeOneClick = mail.UnsubscribeOneClick
                    };

                    body += System.Text.Json.JsonSerializer.Serialize(mailParameters);

                    cmd.Parameters.Add(new SqlParameter("@From", mail.From));
                    cmd.Parameters.Add(new SqlParameter("@To", mail.ToRecipient));
                    cmd.Parameters.Add(new SqlParameter("@Cc", mail.CcRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Bcc", mail.BccRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Subject", mail.Subject));
                    cmd.Parameters.Add(new SqlParameter("@Body", body));
                    cmd.Parameters.Add(new SqlParameter("@PriorityDate", mail.Priority));
                    cmd.Parameters.Add(new SqlParameter("@MailProviderId", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsReadyToSend", !hasAttachment));
                    cmd.Parameters.Add(new SqlParameter("@HasAttachment", hasAttachment));

                    if (!string.IsNullOrEmpty(mail.ToRecipient))
                    {
                        int? mailProviderId = MailProvider.Find(mail.ToRecipient);
                        if (mailProviderId.HasValue)
                        {
                            cmd.Parameters["@MailProviderId"].Value = mailProviderId.Value;
                        }
                    }

                    Guid id = (Guid)cmd.ExecuteScalar();

                    if (hasAttachment)
                    {
                        foreach (var mailQueueAttachment in mail.Attachments)
                        {
                            using (SqlCommand attachmentCommand = new(InsertAttachmentCommandText, cmd.Connection))
                            {
                                attachmentCommand.Parameters.Add(new SqlParameter("@MailQueueId", id));
                                attachmentCommand.Parameters.Add(new SqlParameter("@AttachmentName", mailQueueAttachment.Name));
                                attachmentCommand.Parameters.Add(new SqlParameter("@AttachmentData", mailQueueAttachment.Data));
                                attachmentCommand.ExecuteNonQuery();
                            }
                        }

                        using (SqlCommand setStatusCommand = new(string.Format(UpdateStatusCommandText, "1"), cmd.Connection))
                        {
                            setStatusCommand.Parameters.Add(new SqlParameter("@MailQueueId", id));
                            setStatusCommand.ExecuteNonQuery();
                        }
                    }

                    cmd.Connection.Close();

                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex, false);
            }

            if (MailQueued != null)
            {
                var subs = MailQueued.GetInvocationList();
                foreach (MailQueuedEventHandler sub in subs)
                {
                    sub.Invoke(mail, result);
                }
            }

            return result;
        }

        private static string PrepareMailAddress(string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                return MailMessageBuilder.ParseRecipients(address).ToString().Replace(",", ";");
            }

            return string.Empty;
        }

        

        internal static DateTime GetDateByPriority(Priority priority)
        {
            DateTime priorityDate = DateTime.Now;
            switch (priority)
            {
                case Priority.High:
                    priorityDate = Convert.ToDateTime("2000.01.01");
                    break;
                case Priority.Medium:
                    priorityDate = Convert.ToDateTime("2002.01.01");
                    break;
                case Priority.Low:
                    priorityDate = DateTime.Now;
                    break;
            }
            return priorityDate;
        }
    }
}
