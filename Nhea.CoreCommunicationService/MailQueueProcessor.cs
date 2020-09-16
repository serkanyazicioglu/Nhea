using Nhea.Communication;
using Nhea.Logging;
using Nhea.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Nhea.CoreCommunicationService
{
    public class MailQueueProcessor
    {
        private const string SelectCommandText = @"SET ROWCOUNT @PackageSize; SELECT TOP 100 Id, [From], [To], Cc, Bcc, Subject, Body, Priority, CreateDate, HasAttachment FROM nhea_MailQueue WHERE IsReadyToSend = 1 AND MailProviderId = @MailProviderId ORDER BY [Priority]";
        private const string NullableSelectCommandText = @"SET ROWCOUNT @PackageSize; SELECT TOP 100 Id, [From], [To], Cc, Bcc, Subject, Body, Priority, CreateDate, HasAttachment FROM nhea_MailQueue WHERE IsReadyToSend = 1 AND MailProviderId IS NULL ORDER BY [Priority]";
        private const string SelectAttachmentsCommandText = "SELECT [AttachmentName],[AttachmentData] FROM [nhea_MailQueueAttachment] WHERE MailQueueId = @MailQueueId";

        public static List<Mail> Fetch()
        {
            return Fetch(null);
        }

        public static List<Mail> Fetch(MailProvider mailProvider)
        {
            var mailList = new List<Mail>();

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
                    var mail = new Nhea.CoreCommunicationService.Mail();

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
    }
}
