using Nhea.Configuration;
using Nhea.Logging.LogPublisher;
using System;

namespace Nhea.Logging
{
    public static class Logger
    {
        /// <summary>
        /// Creates new log. In case of failure logs error to event log. Default publish type is Database
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="message">Loggers additional notes.</param>
        public static bool Log(Exception ex)
        {
            return LogCore(LogLevel.Error, Settings.Log.PublishType, null, null, null, ex, Settings.Log.AutoInform);
        }

        public static bool Log(Exception ex, bool autoInform)
        {
            return LogCore(LogLevel.Error, Settings.Log.PublishType, null, null, null, ex, autoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log. Default publish type is Database
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="message">Loggers additional notes.</param>
        public static bool Log(Exception ex, string message)
        {
            return LogCore(LogLevel.Error, Settings.Log.PublishType, null, null, message, ex, Settings.Log.AutoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log. Default publish type is Database
        /// </summary>
        /// <param name="message">Loggers additional notes.</param>
        public static bool Log(string message)
        {
            return LogCore(Settings.Log.LogLevel, Settings.Log.PublishType, null, null, message, null, Settings.Log.AutoInform);
        }

        public static bool Log(string message, bool autoInform)
        {
            return LogCore(Settings.Log.LogLevel, Settings.Log.PublishType, null, null, message, null, autoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log. Default publish type is Database
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="message">Loggers additional notes.</param>
        public static bool Log(LogLevel logLevel, string message)
        {
            return LogCore(logLevel, Settings.Log.PublishType, null, null, message, null, Settings.Log.AutoInform);
        }

        public static bool Log(LogLevel logLevel, string message, bool autoInform)
        {
            return LogCore(logLevel, Settings.Log.PublishType, null, null, message, null, autoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log.
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="publishType">Publishing type of log.</param>
        /// <param name="message">Loggers additional notes.</param>
        public static bool Log(LogLevel logLevel, PublishTypes publishType, string message)
        {
            return LogCore(logLevel, publishType, null, null, message, null, Settings.Log.AutoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log.
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="publishType">Publishing type of log.</param>
        /// <param name="exception">Exception of the log if caused by any type of error.</param>
        public static bool Log(LogLevel logLevel, PublishTypes publishType, Exception exception)
        {
            return LogCore(logLevel, publishType, null, null, null, exception, Settings.Log.AutoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log.
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="publishType">Publishing type of log.</param>
        /// <param name="userName">Current application user's name.</param>
        /// <param name="message">Loggers additional notes.</param>
        public static bool Log(LogLevel logLevel, PublishTypes publishType, string userName, string message)
        {
            return LogCore(logLevel, publishType, null, userName, message, null, Settings.Log.AutoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log.
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="publishType">Publishing type of log.</param>
        /// <param name="userName">Current application user's name.</param>
        /// <param name="message">Loggers additional notes.</param>
        /// <param name="exception">Exception of the log if caused by any type of error.</param>
        public static bool Log(LogLevel logLevel, PublishTypes publishType, string userName, string message, Exception exception)
        {
            return LogCore(logLevel, publishType, exception.Source, userName, message, exception, Settings.Log.AutoInform);
        }

        /// <summary>
        /// Creates new log. In case of failure logs error to event log.
        /// </summary>
        /// <param name="logLevel">Priority of log.</param>
        /// <param name="publishType">Publishing type of log.</param>
        /// <param name="source">Source of log. This parameter can be page name, form name or assembly name.</param>
        /// <param name="userName">Current application user's name.</param>
        /// <param name="message">Loggers additional notes.</param>
        /// <param name="exception">Exception of the log if caused by any type of error.</param>
        public static bool Log(LogLevel logLevel, PublishTypes publishType, string source, string userName, string message, Exception exception)
        {
            return LogCore(logLevel, publishType, source, userName, message, exception, Settings.Log.AutoInform);
        }

        internal static bool Log(LogLevel logLevel, PublishTypes publishType, string source, string userName, string message, Exception exception, bool autoInform)
        {
            return LogCore(logLevel, publishType, source, userName, message, exception, autoInform);
        }

        public delegate void LogPublishingEventHandler(Publisher publisher);

        public delegate void LogPublishedEventHandler(Publisher publisher, bool result);

        public static event LogPublishingEventHandler LogPublishing;

        public static event LogPublishedEventHandler LogPublished;

        private static bool LogCore(LogLevel logLevel, PublishTypes publishType, string source, string userName, string message, Exception exception, bool autoInform)
        {
            Publisher publisher = PublisherFactory.CreatePublisher(publishType);

            if (LogPublishing != null)
            {
                var subs = LogPublishing.GetInvocationList();
                foreach (LogPublishingEventHandler sub in subs)
                {
                    sub.Invoke(publisher);
                }
            }

            publisher.Message = message;
            publisher.LogLevel = logLevel;
            publisher.Source = source;
            publisher.UserName = userName;
            publisher.Exception = exception;
            publisher.AutoInform = autoInform;
            var result = publisher.Publish();

            if (LogPublished != null)
            {
                var subs = LogPublished.GetInvocationList();
                foreach (LogPublishedEventHandler sub in subs)
                {
                    sub.Invoke(publisher, result);
                }
            }

            return result;
        }
    }
}
