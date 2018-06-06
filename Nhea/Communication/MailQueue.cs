using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Nhea.Utils;
using System.Xml;
using System.Net.Mail;
using Nhea.Logging;

namespace Nhea.Communication
{
    public static class MailQueue
    {
        private const string SelectCommandText = @"SET ROWCOUNT @PackageSize; SELECT TOP 100 Id, [From], [To], Cc, Bcc, Subject, Body, Priority, CreateDate, HasAttachment FROM nhea_MailQueue WHERE IsReadyToSend = 1 AND MailProviderId = @MailProviderId";
        private const string NullableSelectCommandText = @"SET ROWCOUNT @PackageSize; SELECT TOP 100 Id, [From], [To], Cc, Bcc, Subject, Body, Priority, CreateDate, HasAttachment FROM nhea_MailQueue WHERE IsReadyToSend = 1 AND MailProviderId IS NULL";

        private const string DeleteCommandText = @"DELETE FROM nhea_MailQueue WHERE Id = @MailQueueId";
        private const string InsertCommandText = @"INSERT INTO nhea_MailQueue(Id, MailProviderId, [From], [To], Cc, Bcc, Subject, Body, Priority, IsReadyToSend, HasAttachment) VALUES(@Id, @MailProviderId, @From, @To, @Cc, @Bcc, @Subject, @Body, @PriorityDate, @IsReadyToSend, @HasAttachment);";

        private const string UpdateStatusCommandText = @"UPDATE nhea_MailQueue SET IsReadyToSend = {0} WHERE Id = @MailQueueId";
        private const string InsertAttachmentCommandText = @"INSERT INTO [nhea_MailQueueAttachment]([MailQueueId],[Path]) VALUES(@MailQueueId, @Path)";
        private const string SelectAttachmentsCommandText = "SELECT [Path] FROM [nhea_MailQueueAttachment] WHERE MailQueueId = @MailQueueId";
        private const string DeleteAttachmentsCommandText = @"DELETE FROM nhea_MailQueueAttachment WHERE MailQueueId = @MailQueueId";

        private static readonly string MoveToHistoryCommandText = @"INSERT INTO nhea_MailQueueHistory
             ([Id],[From],[To],[Cc],[Bcc],[Subject],[Body],[MailProviderId],[Priority],[HasAttachment],[CreateDate],[Status])
             VALUES 
             (@MailQueueId,@From,@To,@Cc,@Bcc,@Subject,@Body,@MailProviderId,@Priority,@HasAttachment,@CreateDate,@Status);" + DeleteCommandText;

        public static bool Add(string from, string toRecipient, string subject, string body)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string subject, string body, List<string> attachmentPaths)
        {
            return Add(from, toRecipient, String.Empty, String.Empty, subject, body, GetDateByPriority(Priority.Medium), attachmentPaths);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string subject, string body)
        {
            return Add(from, toRecipient, ccRecipients, String.Empty, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(Priority.Medium), null);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, List<string> attachmentPaths)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(Priority.Medium), attachmentPaths);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, Priority priority)
        {
            return Add(from, toRecipient, ccRecipients, bccRecipients, subject, body, GetDateByPriority(priority), null);
        }

        public static bool Add(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body, DateTime priorityDate, List<string> attachmentPaths)
        {
            try
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                using (SqlCommand cmd = new SqlCommand(InsertCommandText, sqlConnection))
                {
                    Guid id = Guid.NewGuid();

                    cmd.Connection.Open();

                    bool hasAttachment = false;

                    if (attachmentPaths != null)
                    {
                        hasAttachment = true;

                        foreach (string attachmentPath in attachmentPaths)
                        {
                            SqlCommand attachmentCommand = new SqlCommand(InsertAttachmentCommandText, cmd.Connection);
                            attachmentCommand.Parameters.Add(new SqlParameter("@MailQueueId", id));
                            attachmentCommand.Parameters.Add(new SqlParameter("@Path", attachmentPath));
                            attachmentCommand.ExecuteNonQuery();
                        }
                    }

                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@From", from));
                    cmd.Parameters.Add(new SqlParameter("@To", toRecipient));
                    cmd.Parameters.Add(new SqlParameter("@Cc", ccRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Bcc", bccRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Subject", subject));
                    cmd.Parameters.Add(new SqlParameter("@Body", body));
                    cmd.Parameters.Add(new SqlParameter("@PriorityDate", priorityDate));
                    cmd.Parameters.Add(new SqlParameter("@MailProviderId", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsReadyToSend", true));
                    cmd.Parameters.Add(new SqlParameter("@HasAttachment", hasAttachment));

                    if (!String.IsNullOrEmpty(toRecipient))
                    {
                        int? mailProviderId = MailProvider.Find(toRecipient);
                        if (mailProviderId.HasValue)
                        {
                            cmd.Parameters["@MailProviderId"].Value = mailProviderId.Value;
                        }
                    }

                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, false);

                return false;
            }
        }

        internal static void Delete(Mail mail)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand(DeleteCommandText, sqlConnection))
                {
                    ExecuteDeleteQuery(mail.Id, cmd);
                }

                if (mail.HasAttachment)
                {
                    using (SqlCommand cmd = new SqlCommand(DeleteAttachmentsCommandText, sqlConnection))
                    {
                        ExecuteDeleteQuery(mail.Id, cmd);
                    }
                }

                sqlConnection.Close();
            }
        }

        internal static void MoveToHistory(Mail mail, MailStatus status)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand(MoveToHistoryCommandText, sqlConnection))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@MailQueueId", mail.Id));
                    cmd.Parameters.Add(new SqlParameter("@From", mail.From));
                    cmd.Parameters.Add(new SqlParameter("@To", mail.ToRecipient));
                    cmd.Parameters.Add(new SqlParameter("@Cc", mail.CcRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Bcc", mail.BccRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Subject", mail.Subject));
                    cmd.Parameters.Add(new SqlParameter("@Body", mail.Body));
                    cmd.Parameters.Add(new SqlParameter("@Priority", mail.Priority));
                    cmd.Parameters.Add(new SqlParameter("@CreateDate", mail.CreateDate));
                    cmd.Parameters.Add(new SqlParameter("@HasAttachment", mail.HasAttachment));
                    cmd.Parameters.Add(new SqlParameter("@Status", (int)status));

                    if (mail.MailProviderId.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("@MailProviderId", mail.MailProviderId.Value));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@MailProviderId", DBNull.Value));
                    }

                    cmd.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }
        }

        internal static void SetError(Guid mailQueueId)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
            using (SqlCommand setStatusCommand = new SqlCommand(String.Format(UpdateStatusCommandText, "0"), sqlConnection))
            {
                sqlConnection.Open();
                setStatusCommand.Parameters.Add(new SqlParameter("@MailQueueId", mailQueueId));
                setStatusCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        internal static List<Mail> Fetch()
        {
            return Fetch(null);
        }

        internal static List<Mail> Fetch(MailProvider mailProvider)
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

                                    Attachment attachment = new Attachment(path);
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

                        SetError(mail.Id);
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

        private static void ExecuteDeleteQuery(Guid mailQueueId, SqlCommand cmd)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@MailQueueId", mailQueueId));
            cmd.ExecuteNonQuery();
        }
    }
}
