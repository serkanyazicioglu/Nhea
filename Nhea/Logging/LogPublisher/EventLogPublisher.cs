using System;
using System.Diagnostics;
using System.Text;

namespace Nhea.Logging.LogPublisher
{
    public class EventLogPublisher : Publisher
    {
        #region Publisher Members

        public override string Message { get; set; }
        public override Exception Exception { get; set; }
        public override LogLevel LogLevel { get; set; }

        private EventLogEntryType EventLogEntryLogLevel
        {
            get
            {
                switch (LogLevel)
                {
                    case LogLevel.Error:
                        return EventLogEntryType.Error;
                    case LogLevel.Info:
                        return EventLogEntryType.Information;
                    case LogLevel.Debug:
                    case LogLevel.Warning:
                        return EventLogEntryType.Warning;
                    default:
                        return EventLogEntryType.Information;
                }
            }
        }

        private const string EventLogSourceName = "Nhea.Framework";
        private const string EventLogName = "Nhea.Framework";
        private const string DefaultEventLogSourceName = ".NET Runtime";
        private const string DefaultEventLogName = "Application";

        private const string DateTimeString = "Date Time\t:\t";
        private const string MessageString = "Message\t\t:\t";
        private const string SourceString = "Source\t\t:\t";
        private const string UserNameString = "User Name\t:\t";
        private const string ExceptionDetailString = "Exception Detail\t:\t";
        private const string ExceptionDataString = "Exception Data\t:\t";

        #endregion

        #region Methods

        private string GenerateLogText()
        {
            StringBuilder errorText = new StringBuilder();

            string exceptionDetail;
            string exceptionData;

            ExceptionDetailBuilder.Build(Exception, out exceptionDetail, out exceptionData);

            errorText.Append(DateTimeString);
            errorText.Append(DateTime.Now.ToString());

            if (!String.IsNullOrEmpty(this.Message))
            {
                errorText.Append(Environment.NewLine);
                errorText.Append(MessageString);
                errorText.Append(this.Message);
            }

            if (!String.IsNullOrEmpty(this.Source))
            {
                errorText.Append(Environment.NewLine);
                errorText.Append(SourceString);
                errorText.Append(this.Source);
            }

            if (!String.IsNullOrEmpty(this.UserName))
            {
                errorText.Append(Environment.NewLine);
                errorText.Append(UserNameString);
                errorText.Append(this.UserName);
            }

            if (!String.IsNullOrEmpty(exceptionDetail))
            {
                errorText.Append(Environment.NewLine);
                errorText.Append(ExceptionDetailString);
                errorText.Append(exceptionDetail);
            }

            if (!String.IsNullOrEmpty(exceptionData))
            {
                errorText.Append(Environment.NewLine);
                errorText.Append(ExceptionDataString);
                errorText.Append(exceptionData);
            }

            return errorText.ToString();
        }

        #endregion

        public override bool Publish()
        {
            try
            {
                EventLog eventLog = new EventLog();

                eventLog.Source = EventLogSourceName;
                
                try
                {
                    if (!EventLog.SourceExists(EventLogSourceName))
                    {
                        EventLog.CreateEventSource(EventLogSourceName, EventLogName);
                    }
                }
                catch
                {
                    eventLog.Source = DefaultEventLogSourceName;
                }

                eventLog.WriteEntry(GenerateLogText(), EventLogEntryLogLevel);

                return base.Publish();
            }
            catch
            {
                return false;
            }
        }
    }
}