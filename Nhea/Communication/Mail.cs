using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Nhea.Communication
{
    public class Mail
    {
        protected internal Mail()
        {
            Attachments = new List<Attachment>();
        }

        protected internal Guid Id { get; set; }

        protected internal int? MailProviderId { get; set; }

        public string From { get; set; }

        public string ToRecipient { get; set; }

        public string CcRecipients { get; set; }

        public string BccRecipients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public List<string> AttachmentPaths { get; set; }

        public DateTime Priority { get; set; }

        protected internal DateTime CreateDate { get; set; }

        protected internal bool HasAttachment { get; set; }

        protected internal List<Attachment> Attachments { get; set; }

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

                    SmtpHelper.SendMail(From, ToRecipient, CcRecipients, BccRecipients, Subject, Body, false, Attachments);
                    MailQueue.MoveToHistory(this, MailStatus.Sent);
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

        public bool Save()
        {
            return MailQueue.Add(this);
        }

        public void Delete()
        {
            MailQueue.Delete(this);
        }

        public void SetError()
        {
            try
            {
                MailQueue.SetError(Id);
            }
            catch
            {
            }
        }
    }
}
