using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            public static PublishType PublishType
            {
                get
                {
                    return config.PublishType;
                }
            }

            /// <summary>
            /// Gets default log level
            /// </summary>
            public static LogLevel LogLevel
            {
                get
                {
                    return config.LogLevel;
                }
            }

            /// <summary>
            /// Gets default FilePath
            /// </summary>
            public static string FilePath
            {
                get
                {
                    return config.FilePath;
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
