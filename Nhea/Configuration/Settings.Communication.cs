using System.Collections.Generic;
using System.Linq;
using Nhea.Configuration.GenericConfigSection.CommunicationSection;
using Nhea.Configuration.GenericConfigSection.Communication;

namespace Nhea.Configuration
{
    public static partial class Settings
    {
        internal static NheaCommunicationConfigurationSettings CurrentCommunicationConfigurationSettings { get; set; }
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
                    if (!string.IsNullOrEmpty(config.ConnectionName))
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
                    if (CurrentCommunicationConfigurationSettings != null && CurrentCommunicationConfigurationSettings.SmtpSettings != null)
                    {
                        return CurrentCommunicationConfigurationSettings.SmtpSettings;
                    }

                    return config.SmtpSettings.Cast<SmtpElement>();
                }
            }
        }
    }
}
