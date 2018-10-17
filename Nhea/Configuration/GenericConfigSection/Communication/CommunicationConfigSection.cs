using System;
using System.Configuration;
using Nhea.Configuration.GenericConfigSection.Communication;

namespace Nhea.Configuration.GenericConfigSection.CommunicationSection
{
    /// <summary>
    /// Nhea Configuration Connection Class
    /// </summary>
    internal class CommunicationConfigSection : ConfigurationSection
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

        [ConfigurationProperty("smtpSettings", IsRequired = false, IsDefaultCollection=true)]
        public SmtpSettingsElementCollection SmtpSettings
        {
            get
            {
                return (SmtpSettingsElementCollection)this["smtpSettings"];
            }
        }

        [ConfigurationProperty("openReportUrl", IsRequired = false, IsDefaultCollection = true)]
        public string OpenReportUrl
        {
            get
            {
                return (string)this["openReportUrl"];
            }
        }

        [ConfigurationProperty("notificationsPublicKey", IsRequired = false, IsDefaultCollection = true)]
        public string NotificationsPublicKey
        {
            get
            {
                return (string)this["notificationsPublicKey"];
            }
        }

        [ConfigurationProperty("notificationsApplicationId", IsRequired = false, IsDefaultCollection = true)]
        public string NotificationsApplicationId
        {
            get
            {
                return (string)this["notificationsApplicationId"];
            }
        }
    }
}
