using Microsoft.Extensions.Logging;
using Nhea.Logging;

namespace Nhea.Configuration
{
    public class NheaLogConfigurationSettings
    {
        /// <summary>
        /// Sql connection string for 'Database' publish type.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Logging default publish type.
        /// </summary>
        public PublishTypes PublishType { get; set; }

        /// <summary>
        /// Gets or sets default log level.
        /// </summary>
        public LogLevel DefaultLogLevel { get; set; }

        /// <summary>
        /// Gets or sets default directory for 'File' publish type.
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets log file name for 'File' publish type.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// If true logs also send a mail.
        /// </summary>
        public bool AutoInform { get; set; }

        /// <summary>
        /// Default mail subject when auto inform is enabled.
        /// </summary>
        public string InformSubject { get; set; }

        /// <summary>
        /// Gets or sets default from e-mail address.
        /// </summary>
        public string MailFrom { get; set; }

        /// <summary>
        /// Gets or sets default addresses to be sent to.
        /// </summary>
        public string MailList { get; set; }
    }
}
