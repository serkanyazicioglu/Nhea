using Microsoft.Extensions.Logging;
using Nhea.Configuration.GenericConfigSection.Communication;
using Nhea.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nhea.Configuration
{
    public class NheaConfigurationSettings
    {
        public EnvironmentType EnvironmentType { get; set; }

        public string DataConnectionString { get; set; }

        public string CommunicationConnectionString { get; set; }

        public PublishTypes PublishType { get; set; }

        public static IEnumerable<SmtpElement> SmtpSettings { get; set; }

        /// <summary>
        /// Gets default log level
        /// </summary>
        public LogLevel DefaultLogLevel { get; set; }

        /// <summary>
        /// Gets default directory
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Gets log file name
        /// </summary>
        public string FileName { get; set; }

        public string MailFrom { get; set; }

        /// <summary>
        /// Gets default MailList
        /// </summary>
        public string MailList { get; set; }

        public bool AutoInform { get; set; }

        public string InformSubject { get; set; }
    }
}
