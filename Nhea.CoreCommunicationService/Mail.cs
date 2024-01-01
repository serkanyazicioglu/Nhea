using Nhea.Communication;
using Nhea.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Nhea.CoreCommunicationService
{
    public class Mail : Nhea.Communication.Mail
    {
        private const string UpdateStatusCommandText = @"UPDATE nhea_MailQueue SET IsReadyToSend = {0} WHERE Id = @MailQueueId";
        private const string DeleteCommandText = @"DELETE FROM nhea_MailQueue WHERE Id = @MailQueueId";
        private static readonly string MoveToHistoryCommandText = @"INSERT INTO nhea_MailQueueHistory
             ([Id],[From],[To],[Cc],[Bcc],[Subject],[Body],[MailProviderId],[Priority],[HasAttachment],[CreateDate],[Status])
             VALUES 
             (@MailQueueId,@From,@To,@Cc,@Bcc,@Subject,@Body,@MailProviderId,@Priority,@HasAttachment,@CreateDate,@Status);" + DeleteCommandText;
        private const string DeleteAttachmentsCommandText = @"DELETE FROM nhea_MailQueueAttachment WHERE MailQueueId = @MailQueueId";

        public void Send()
        {
            int tryCount = 0;

            while (true)
            {
                try
                {
                    var attachments = new List<System.Net.Mail.Attachment>();

                    if (HasAttachment)
                    {
                        attachments.AddRange(Attachments.Select(attachment => new System.Net.Mail.Attachment(new MemoryStream(attachment.Data), attachment.Name)));
                    }

                    var smtpElement = SmtpHelper.SendMail(From, ToRecipient, CcRecipients, BccRecipients, Subject, Body, false, attachments);

                    if (!smtpElement.DisableHistoryLogging)
                    {
                        if (string.IsNullOrEmpty(this.From))
                        {
                            this.From = smtpElement.From;
                        }

                        MoveToHistory(MailStatus.Sent);
                    }
                    else
                    {
                        DeleteFromQueue();
                    }

                    return;
                }
                catch
                {
                    if (tryCount < 10)
                    {
                        tryCount++;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        internal void SetError()
        {
            try
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                using (SqlCommand setStatusCommand = new SqlCommand(String.Format(UpdateStatusCommandText, "0"), sqlConnection))
                {
                    sqlConnection.Open();
                    setStatusCommand.Parameters.Add(new SqlParameter("@MailQueueId", Id));
                    setStatusCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
            catch
            {
            }
        }

        private void MoveToHistory(MailStatus status)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand(MoveToHistoryCommandText, sqlConnection))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@MailQueueId", Id));
                    cmd.Parameters.Add(new SqlParameter("@From", From));
                    cmd.Parameters.Add(new SqlParameter("@To", ToRecipient));
                    cmd.Parameters.Add(new SqlParameter("@Cc", CcRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Bcc", BccRecipients));
                    cmd.Parameters.Add(new SqlParameter("@Subject", Subject));
                    cmd.Parameters.Add(new SqlParameter("@Body", Body));
                    cmd.Parameters.Add(new SqlParameter("@Priority", Priority));
                    cmd.Parameters.Add(new SqlParameter("@CreateDate", CreateDate));
                    cmd.Parameters.Add(new SqlParameter("@HasAttachment", HasAttachment));
                    cmd.Parameters.Add(new SqlParameter("@Status", (int)status));

                    if (MailProviderId.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("@MailProviderId", MailProviderId.Value));
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

        private void DeleteFromQueue()
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand(DeleteCommandText, sqlConnection))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@MailQueueId", Id));
                    cmd.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }
        }
    }
}
