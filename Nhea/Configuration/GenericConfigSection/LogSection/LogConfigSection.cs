using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [ConfigurationProperty("defaultPublishType", DefaultValue = PublishType.EventLog)]
        public PublishType PublishType
        {
            get
            {
                return (PublishType)Enum.Parse(typeof(PublishType), this["defaultPublishType"].ToString());
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

        [ConfigurationProperty("filePath", DefaultValue = "ErrorLog.txt", IsRequired = false)]
        public string FilePath
        {
            get
            {
                return this["filePath"].ToString();
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
