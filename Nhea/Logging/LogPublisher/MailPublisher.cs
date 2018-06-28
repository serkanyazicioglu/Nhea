using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nhea.Communication;
using Nhea.Configuration.GenericConfigSection;
using Nhea.Configuration.GenericConfigSection.LogSection;
using Nhea.Configuration;

namespace Nhea.Logging.LogPublisher
{
    public class MailPublisher : Publisher
    {
        #region Publisher Properties

        public override string Message { get; set; }
        public override Exception Exception { get; set; }
        public override LogLevel LogLevel { get; set; }

        #endregion

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

                    exceptionDetail = String.Format("<b>Exception Detail:</b>{1}{0}{1}", Nhea.Text.HtmlHelper.ReplaceNewLineWithHtml(exceptionDetail), "<br/>");
                    exceptionData = String.Format("<b>Exception Detail:</b>{1}{0}{1}", Nhea.Text.HtmlHelper.ReplaceNewLineWithHtml(exceptionData), "<br/>");

                    if (String.IsNullOrEmpty(this.Message))
                    {
                        this.Message = this.Exception.Message;
                    }
                }

                string detail = String.Format("<b>Message:</b> {3}{8}<b>LogLevel:</b> {2}{8}<b>FileName:</b> {0}{8}<b>Date:</b> {1}{8}<b>Source:</b> {4}{8}<b>Username:</b> {5}{8}{8}{7}{6}<b>----------------------------------end----------------------------------</b>", fileName, DateTime.Now.ToString(), LogLevel.ToString(), Message, Source, UserName, exceptionDetail, exceptionData, "<br/>");

                return PublishToMail(detail);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, PublishType.File, null, null, ex.Message, ex, false);
                Logger.Log(LogLevel.Error, PublishType.File, this.Source, this.UserName, this.Message, this.Exception, false);

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

                MailQueue.Add(String.Empty, Settings.Log.MailList, subject, detail);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
