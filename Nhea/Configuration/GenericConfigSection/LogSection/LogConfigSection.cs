using System;
using System.Configuration;
using Nhea.Logging;

namespace Nhea.Configuration.GenericConfigSection.LogSection
{
    /// <summary>
    /// Nhea Configuration Logging Handler Class
    /// </summary>
    public class LogConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("connectionName", IsRequired = false)]
        public string ConnectionName
        {
            get
            {
                if (!String.IsNullOrEmpty(this["connectionName"].ToString()))
                {
                    return this["connectionName"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        [ConfigurationProperty("defaultPublishType", DefaultValue = PublishTypes.File)]
        public PublishTypes PublishType
        {
            get
            {
                return (PublishTypes)Enum.Parse(typeof(PublishTypes), this["defaultPublishType"].ToString());
            }
        }

        [ConfigurationProperty("defaultlogLevel", DefaultValue = LogLevel.Info)]
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)Enum.Parse(typeof(LogLevel), this["defaultlogLevel"].ToString());
            }
        }

        [ConfigurationProperty("directoryPath", DefaultValue = "Logs", IsRequired = false)]
        public string DirectoryPath
        {
            get
            {
                return this["directoryPath"].ToString();
            }
        }

        [ConfigurationProperty("fileName", DefaultValue = "NheaLog-{0:dd.MM.yyyy-HH:mm}.txt", IsRequired = false)]
        public string FileName
        {
            get
            {
                return this["fileName"].ToString();
            }
        }

        [ConfigurationProperty("mailFrom", IsRequired = false, DefaultValue = "")]
        public string MailFrom
        {
            get
            {
                return this["mailFrom"].ToString();
            }
        }

        [ConfigurationProperty("mailList", IsRequired = false, DefaultValue = "")]
        public string MailList
        {
            get
            {
                return this["mailList"].ToString();
            }
        }

        [ConfigurationProperty("autoInform", IsRequired = false, DefaultValue = false)]
        public bool AutoInform
        {
            get
            {
                return Convert.ToBoolean(this["autoInform"]);
            }
        }

        [ConfigurationProperty("informSubject", IsRequired = false, DefaultValue = "")]
        public string InformSubject
        {
            get
            {
                return this["informSubject"].ToString();
            }
        }
    }
}
