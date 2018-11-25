using Nhea.Logging;
using Nhea.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Nhea.Communication
{
    public static class MailQueue
    {
        private const string SelectCommandText = @"SET ROWCOUNT @PackageSize; SELECT TOP 100 Id, [From], [To], Cc, Bcc, Subject, Body, Priority, CreateDate, HasAttachment FROM nhea_MailQueue WHERE IsReadyToSend = 1 AND MailProviderId = @MailProviderId";
        private const string NullableSelectCommandText = @"SET ROWCOUNT @PackageSize; SELECT TOP 100 Id, [From], [To], Cc, Bcc, Subject, Body, Priority, CreateDate, HasAttachment FROM nhea_MailQueue WHERE IsReadyToSend = 1 AND MailProviderId IS NULL";

        private const string DeleteCommandText = @"DELETE FROM nhea_MailQueue WHERE Id = @MailQueueId";
        private const string InsertCommandText = @"INSERT INTO nhea_MailQueue(Id, MailProviderId, [From], [To], Cc, Bcc, Subject, Body, Priority, IsReadyToSend, HasAttachment) VALUES(@Id, @MailProviderId, @From, @To, @Cc, @Bcc, @Subject, @Body, @PriorityDate, @IsReadyToSend, @HasAttachment);";

        private const string InsertAttachmentCommandText = @"INSERT INTO [nhea_MailQueueAttachment]([MailQueueId],[AttachmentName],[AttachmentData]) VALUES(@MailQueueId, @AttachmentName,@AttachmentData)";
        private const string SelectAttachmentsCommandText = "SELECT [AttachmentName],[AttachmentData] FROM [nhea_MailQueueAttachment] WHERE MailQueueId = @MailQueueId";

        public static bool Add(string from, string toRecipient, string subject, string body)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string subject, string body, MailQueueAttachment attachment)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, GetDateByPriority(Priority.Medium), new List<MailQueueAttachment> { attachment });
        }

        public static bool Add(string from, string toRecipient, string subject, string body, List<MailQueueAttachment> attachments)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, GetDateByPriority(Priority.Medium), attachments);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string subject, string body)
        {
            return Add(from, toRecipient, ccRecipients, String.Empty, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<MailQueueAttachment> attachments)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(Priority.Medium), attachments);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, Priority priority)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(priority), null);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, DateTime priorityDate, List<MailQueueAttachment> attachments)
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

        public delegate void MailQueueingEventHandler(Mail mail);

        public delegate void MailQueuedEventHandler(Mail mail, bool result);

        public static event MailQueueingEventHandler MailQueueing;

        public static event MailQueuedEventHandler MailQueued;

        public static bool Add(Mail mail)
        {
            var result = false;

            try
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                using (SqlCommand cmd = new SqlCommand(InsertCommandText, sqlConnection))
                {
                    Guid id = Guid.NewGuid();

                    cmd.Connection.Open();

                    bool hasAttachment = false;

                    if (mail.Attachments != null && mail.Attachments.Any())
                    {
                        hasAttachment = true;

                        foreach (var mailQueueAttachment in mail.Attachments)
                        {
                            SqlCommand attachmentCommand = new SqlCommand(InsertAttachmentCommandText, cmd.Connection);
                            attachmentCommand.Parameters.Add(new SqlParameter("@MailQueueId", id));
                            attachmentCommand.Parameters.Add(new SqlParameter("@AttachmentName", mailQueueAttachment.Name));
                            attachmentCommand.Parameters.Add(new SqlParameter("@AttachmentData", mailQueueAttachment.Data));
                            attachmentCommand.ExecuteNonQuery();
                        }
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

                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@From", mail.From));
                    cmd.Parameters.Add(new SqlParameter("@To", mail.ToRecipient));
                    cmd.Parameters.Add(new SqlParameter("@Cc", mail.CcRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Bcc", mail.BccRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Subject", mail.Subject));
                    cmd.Parameters.Add(new SqlParameter("@Body", mail.Body));
                    cmd.Parameters.Add(new SqlParameter("@PriorityDate", mail.Priority));
                    cmd.Parameters.Add(new SqlParameter("@MailProviderId", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsReadyToSend", true));
                    cmd.Parameters.Add(new SqlParameter("@HasAttachment", hasAttachment));

                    if (!String.IsNullOrEmpty(mail.ToRecipient))
                    {
                        int? mailProviderId = MailProvider.Find(mail.ToRecipient);
                        if (mailProviderId.HasValue)
                        {
                            cmd.Parameters["@MailProviderId"].Value = mailProviderId.Value;
                        }
                    }

                    cmd.ExecuteNonQuery();
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
            if (address != null)
            {
                if (!String.IsNullOrEmpty(address) && address.Contains(","))
                {
                    address = address.Replace(',', ';');
                }
            }
            else
            {
                address = String.Empty;
            }

            return address;
        }

        public static List<Mail> Fetch()
        {
            return Fetch(null);
        }

        public static List<Mail> Fetch(MailProvider mailProvider)
        {
            List<Mail> mailList = new List<Mail>();

            int mailProviderId = 0;
            int packageSize = 25;
            string cmdText = NullableSelectCommandText;

            if (mailProvider != null)
            {
                mailProviderId = mailProvider.Id;
                cmdText = SelectCommandText;
                packageSize = mailProvider.PackageSize;
            }

            using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
            using (SqlCommand cmd = new SqlCommand(cmdText, sqlConnection))
            {
                cmd.Connection.Open();

                if (mailProvider != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@MailProviderId", mailProviderId));
                }

                cmd.Parameters.Add(new SqlParameter("@PackageSize", packageSize));

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Mail mail = new Mail();

                    mail.Id = reader.GetGuid(0);

                    try
                    {
                        mail.From = reader.GetString(1);
                        mail.ToRecipient = reader.GetString(2);
                        mail.CcRecipients = reader.GetString(3);
                        mail.BccRecipients = reader.GetString(4);
                        mail.Subject = reader.GetString(5);
                        mail.Body = reader.GetString(6);
                        mail.Priority = reader.GetDateTime(7);
                        mail.CreateDate = reader.GetDateTime(8);
                        mail.HasAttachment = reader.GetBoolean(9);

                        if (mail.HasAttachment)
                        {
                            using (SqlConnection attachmentConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                            using (SqlCommand attachmentCommand = new SqlCommand(SelectAttachmentsCommandText, attachmentConnection))
                            {
                                attachmentConnection.Open();

                                attachmentCommand.Parameters.Add(new SqlParameter("@MailQueueId", mail.Id));

                                SqlDataReader attachmentReader = attachmentCommand.ExecuteReader();

                                while (attachmentReader.Read())
                                {
                                    string path = attachmentReader.GetString(0);

                                    var attachment = new MailQueueAttachment
                                    {
                                        Name = attachmentReader.GetString(0),
                                        Data = (byte[])attachmentReader["AttachmentData"]
                                    };

                                    mail.Attachments.Add(attachment);
                                }

                                attachmentConnection.Close();
                            }
                        }

                        mailList.Add(mail);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex, false);

                        mail.SetError();
                    }
                }

                cmd.Connection.Close();
            }

            return mailList;
        }

        private static DateTime GetDateByPriority(Priority priority)
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
