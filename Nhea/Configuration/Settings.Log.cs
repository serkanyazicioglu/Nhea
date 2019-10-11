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
        internal static NheaLogConfigurationSettings CurrentLogConfigurationSettings { get; set; }

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
                    if (CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(CurrentLogConfigurationSettings.ConnectionString))
                    {
                        return CurrentLogConfigurationSettings.ConnectionString;
                    }

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
                    if (CurrentLogConfigurationSettings != null)
                    {
                        return CurrentLogConfigurationSettings.PublishType;
                    }

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
                    if (CurrentLogConfigurationSettings != null)
                    {
                        return CurrentLogConfigurationSettings.DefaultLogLevel;
                    }

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
                    if (CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(CurrentLogConfigurationSettings.DirectoryPath))
                    {
                        return CurrentLogConfigurationSettings.DirectoryPath;
                    }

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
                    if (CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(CurrentLogConfigurationSettings.FileName))
                    {
                        return CurrentLogConfigurationSettings.FileName;
                    }

                    return config.FileName;
                }
            }

            public static string MailFrom
            {
                get
                {
                    if (CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(CurrentLogConfigurationSettings.MailFrom))
                    {
                        return CurrentLogConfigurationSettings.MailFrom;
                    }

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
                    if (CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(CurrentLogConfigurationSettings.MailList))
                    {
                        return CurrentLogConfigurationSettings.MailList;
                    }

                    return config.MailList;
                }
            }

            public static bool AutoInform
            {
                get
                {
                    if (CurrentLogConfigurationSettings != null)
                    {
                        return CurrentLogConfigurationSettings.AutoInform;
                    }

                    return config.AutoInform;
                }
            }

            public static string InformSubject
            {
                get
                {
                    if (CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(CurrentLogConfigurationSettings.InformSubject))
                    {
                        return CurrentLogConfigurationSettings.InformSubject;
                    }

                    return config.InformSubject;
                }
            }
        }
    }
}
