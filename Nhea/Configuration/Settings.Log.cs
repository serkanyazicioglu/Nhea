using System;
using Microsoft.Extensions.Logging;
using Nhea.Configuration.GenericConfigSection.LogSection;
using Nhea.Logging;

namespace Nhea.Configuration
{
    /// <summary>
    /// Nhea Configuration Section Helper Class
    /// </summary>
    public static partial class Settings
    {
        /// <summary>
        /// Nhea Log Configuration Section Helper Class
        /// </summary>
        public static class Log
        {
            private static LogConfigSection config = ConfigFactory.GetConfigurationSection<LogConfigSection>("nhea/log");

            public static string ConnectionName
            {
                get
                {
                    if (!String.IsNullOrEmpty(config.ConnectionName))
                    {
                        return config.ConnectionName;
                    }
                    else
                    {
                        return Settings.Data.ConnectionName;
                    }
                }
            }

            /// <summary>
            /// Gets default PublishType
            /// </summary>
            public static PublishTypes PublishType
            {
                get
                {
                    return config.PublishType;
                }
            }

            /// <summary>
            /// Gets default log level
            /// </summary>
            public static LogLevel DefaultLogLevel
            {
                get
                {
                    return config.DefaultLogLevel;
                }
            }

            /// <summary>
            /// Gets default directory
            /// </summary>
            public static string DirectoryPath
            {
                get
                {
                    return config.DirectoryPath;
                }
            }

            /// <summary>
            /// Gets log file name
            /// </summary>
            public static string FileName
            {
                get
                {
                    return config.FileName;
                }
            }

            public static string MailFrom
            {
                get
                {
                    return config.MailFrom;
                }
            }

            /// <summary>
            /// Gets default MailList
            /// </summary>
            public static string MailList
            {
                get
                {
                    return config.MailList;
                }
            }

            public static bool AutoInform
            {
                get
                {
                    return config.AutoInform;
                }
            }

            public static string InformSubject
            {
                get
                {
                    return config.InformSubject;
                }
            }
        }
    }
}
