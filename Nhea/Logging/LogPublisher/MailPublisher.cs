using Microsoft.Extensions.Logging;
using Nhea.Communication;
using Nhea.Configuration;
using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace Nhea.Logging.LogPublisher
{
    public class MailPublisher : Publisher
    {
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
                detail += "<b>Log Level:</b> " + Level.ToString() + HtmlNewLine;
                detail += "<b>Date:</b> " + DateTime.Now.ToString() + HtmlNewLine;
                detail += "<b>Username:</b> " + UserName + HtmlNewLine;
                
                string aspnetCoreEnvironmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (!string.IsNullOrEmpty(aspnetCoreEnvironmentVariable))
                {
                    detail += "<b>Environment :</b> " + aspnetCoreEnvironmentVariable + HtmlNewLine;
                }
                else
                {
                    detail += "<b>Environment :</b> " + Nhea.Configuration.Settings.Application.EnvironmentType.ToString() + HtmlNewLine;
                }

                detail += "<b>OSVersion :</b> " + System.Environment.OSVersion + HtmlNewLine;

                //var version = System.Environment.Version;
                //var appContextFrameworkName = System.AppContext.TargetFrameworkName;
                string frameworkName = null;

                try
                {
                    frameworkName = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
                }
                catch
                { }

                if (string.IsNullOrEmpty(frameworkName))
                {
                    frameworkName = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
                }

                detail += "<b>Framework :</b> " + frameworkName + HtmlNewLine;

                detail += "<b>OSArchitecture :</b> " + System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString() + HtmlNewLine;

                detail += "<b>ProcessArchitecture :</b> " + System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString() + HtmlNewLine;

                //detail += "Is64BitOperatingSystem : " + System.Environment.Is64BitOperatingSystem.ToString() + HtmlNewLine;
                //detail += "Is64BitProcess : " + System.Environment.Is64BitProcess.ToString() + HtmlNewLine;

                detail += "<b>Source:</b> " + Source + HtmlNewLine;

                if (!String.IsNullOrEmpty(fileName))
                {
                    detail += "<b>File Name:</b> " + fileName + HtmlNewLine;
                }

                detail += HtmlNewLine + HtmlNewLine;
                detail += exceptionData;
                detail += exceptionDetail;
                detail += "<b>----------------------------------end----------------------------------</b>";

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
                Logger.Log(LogLevel.Error, PublishTypes.File, null, null, ex.Message, ex, false);
                Logger.Log(LogLevel.Error, PublishTypes.File, this.Source, this.UserName, this.Message, this.Exception, false);

                return false;
            }
        }
    }
}
