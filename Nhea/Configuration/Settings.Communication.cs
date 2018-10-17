using System;
using System.Collections.Generic;
using System.Linq;
using Nhea.Configuration.GenericConfigSection.CommunicationSection;
using Nhea.Configuration.GenericConfigSection.Communication;
using Nhea.Helper;

namespace Nhea.Configuration
{
    public static partial class Settings
    {
        /// <summary>
        /// Nhea Communication Configuration Section Helper Class
        /// </summary>
        public static partial class Communication
        {
            private static CommunicationConfigSection config = ConfigFactory.GetConfigurationSection<CommunicationConfigSection>("nhea/communication");

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

            public static IEnumerable<SmtpElement> SmtpSettings
            {
                get
                {
                    return config.SmtpSettings.Cast<SmtpElement>();
                }
            }

            public static string NotificationsPublicKey
            {
                get
                {
                    return config.NotificationsPublicKey;
                }
            }

            public static string NotificationsApplicationId
            {
                get
                {
                    return config.NotificationsApplicationId;
                }
            }

            public static string OpenReportUrl
            {
                get
                {
                    return config.OpenReportUrl.Convert<string>("");
                }
            }
        }
    }
}
