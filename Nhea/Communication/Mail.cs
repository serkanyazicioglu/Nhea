using Nhea.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Nhea.Communication
{
    public class Mail
    {
        private const string UpdateStatusCommandText = @"UPDATE nhea_MailQueue SET IsReadyToSend = {0} WHERE Id = @MailQueueId";
        private const string DeleteCommandText = @"DELETE FROM nhea_MailQueue WHERE Id = @MailQueueId";
        private static readonly string MoveToHistoryCommandText = @"INSERT INTO nhea_MailQueueHistory
             ([Id],[From],[To],[Cc],[Bcc],[Subject],[Body],[MailProviderId],[Priority],[HasAttachment],[CreateDate],[Status])
             VALUES 
             (@MailQueueId,@From,@To,@Cc,@Bcc,@Subject,@Body,@MailProviderId,@Priority,@HasAttachment,@CreateDate,@Status);" + DeleteCommandText;
        private const string DeleteAttachmentsCommandText = @"DELETE FROM nhea_MailQueueAttachment WHERE MailQueueId = @MailQueueId";

        protected internal Mail()
        {
            Attachments = new List<MailQueueAttachment>();
        }

        protected internal Guid Id { get; set; }

        protected internal int? MailProviderId { get; set; }

        public string From { get; set; }

        public string ToRecipient { get; set; }

        public string CcRecipients { get; set; }

        public string BccRecipients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime Priority { get; set; }

        protected internal DateTime CreateDate { get; set; }

        protected internal bool HasAttachment { get; set; }

        protected internal List<MailQueueAttachment> Attachments { get; set; }

        public void Send()
        {
            int tryCount = 0;

            while (true)
            {
                try
                {
                    //if (!String.IsNullOrEmpty(Nhea.Configuration.Settings.Communication.OpenReportUrl))
                    //{
                    //    string openReportUrl = Nhea.Configuration.Settings.Communication.OpenReportUrl;

                    //    if (!openReportUrl.EndsWith("?"))
                    //    {
                    //        openReportUrl += "?";
                    //    }

                    //    string key = "i=" + this.Id;

                    //    string hash = Nhea.Security.QueryStringSecurity.HashQueryString(key);

                    //    string imageHtml = String.Format("<img src=\"{0}\" height=\"1\" width=\"1\"/>", openReportUrl + hash);

                    //    this.Body += imageHtml;
                    //}

                    var attachments = new List<System.Net.Mail.Attachment>();

                    if (HasAttachment)
                    {
                        foreach (var attachment in Attachments)
                        {
                            var newAttachment = new System.Net.Mail.Attachment(new MemoryStream(attachment.Data), attachment.Name);
                            attachments.Add(newAttachment);
                        }
                    }

                    SmtpHelper.SendMail(From, ToRecipient, CcRecipients, BccRecipients, Subject, Body, false, attachments);
                    MoveToHistory(MailStatus.Sent);
                    return;
                }
                catch (Exception ex)
                {
                    if (tryCount < 10)
                    {
                        tryCount++;
                    }
                    else
                    {
                        throw ex;
                    }
                }
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

        public bool Save()
        {
            return MailQueue.Add(this);
        }

        internal void Delete()
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

                if (HasAttachment)
                {
                    using (SqlCommand cmd = new SqlCommand(DeleteAttachmentsCommandText, sqlConnection))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@MailQueueId", Id));
                        cmd.ExecuteNonQuery();
                    }
                }

                sqlConnection.Close();
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
    }
}
