using Nhea.Communication;
using Nhea.Configuration;
using System;

namespace Nhea.Logging.LogPublisher
{
    public class MailPublisher : Publisher
    {
        #region Publisher Properties

        public override string Message { get; set; }
        public override Exception Exception { get; set; }
        public override LogLevel LogLevel { get; set; }

        #endregion

        private const string HtmlNewLine = "<br/>";

        public override bool Publish()
        {
            try
            {
                string exceptionDetail = String.Empty;
                string exceptionData = String.Empty;
                string fileName = String.Empty;

                this.Message = Nhea.Text.HtmlHelper.ReplaceNewLineWithHtml(this.Message);

                if (this.Exception != null)
                {
                    ExceptionDetailBuilder.Build(this.Exception, out exceptionDetail, out exceptionData, out fileName);

                    exceptionDetail = String.Format("<b>Exception Detail:</b>{1}{0}{1}", Nhea.Text.HtmlHelper.ReplaceNewLineWithHtml(exceptionDetail), HtmlNewLine);
                    exceptionData = String.Format("<b>Exception Data:</b>{1}{0}{1}", Nhea.Text.HtmlHelper.ReplaceNewLineWithHtml(exceptionData), HtmlNewLine);

                    if (String.IsNullOrEmpty(this.Message))
                    {
                        this.Message = this.Exception.Message;
                    }
                }

                string detail = "<b>Message:</b> " + Message + HtmlNewLine;
                detail += "<b>Log Level:</b> " + LogLevel.ToString() + HtmlNewLine;
                detail += "<b>Date:</b> " + DateTime.Now.ToString() + HtmlNewLine;
                detail += "<b>Username:</b> " + UserName + HtmlNewLine;
                detail += "<b>Source:</b> " + Source + HtmlNewLine;

                if (!String.IsNullOrEmpty(fileName))
                {
                    detail += "<b>File Name:</b> " + fileName + HtmlNewLine;
                }

                detail += HtmlNewLine + HtmlNewLine;
                detail += exceptionData;
                detail += exceptionDetail;
                detail += "<b>----------------------------------end----------------------------------</b>";

                return PublishToMail(detail);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, PublishTypes.File, null, null, ex.Message, ex, false);
                Logger.Log(LogLevel.Error, PublishTypes.File, this.Source, this.UserName, this.Message, this.Exception, false);

                return false;
            }
        }

        private bool PublishToMail(string detail)
        {
            try
            {
                string subject = String.Empty;

                if (String.IsNullOrEmpty(Settings.Log.MailList))
                {
                    subject = Settings.Log.InformSubject;
                }
                else
                {
                    subject = this.Message;
                }

                string mailFrom = String.Empty;

                if (!String.IsNullOrEmpty(Settings.Log.MailFrom))
                {
                    mailFrom = Settings.Log.MailFrom;
                }

                MailQueue.Add(mailFrom, Settings.Log.MailList, subject, detail);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
