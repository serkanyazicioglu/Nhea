using Nhea.Configuration;

namespace Nhea.Utils
{
    internal class ConnectionStringBuilder
    {
        internal static string Build(ConnectionSource connectionSource)
        {
            string connectionStringName;

            switch (connectionSource)
            {
                default:
                case ConnectionSource.Data:
                    {
                        if (Settings.CurrentDataConfigurationSettings != null && !string.IsNullOrEmpty(Settings.CurrentDataConfigurationSettings.ConnectionString))
                        {
                            return Settings.CurrentDataConfigurationSettings.ConnectionString;
                        }

                        connectionStringName = Settings.Data.ConnectionName;
                    }
                    break;
                case ConnectionSource.Communication:
                    {
                        if (Settings.CurrentCommunicationConfigurationSettings != null && !string.IsNullOrEmpty(Settings.CurrentCommunicationConfigurationSettings.ConnectionString))
                        {
                            return Settings.CurrentCommunicationConfigurationSettings.ConnectionString;
                        }

                        connectionStringName = Settings.Communication.ConnectionName;
                    }
                    break;
                case ConnectionSource.Log:
                    {
                        if (Settings.CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(Settings.CurrentLogConfigurationSettings.ConnectionString))
                        {
                            return Settings.CurrentLogConfigurationSettings.ConnectionString;
                        }

                        connectionStringName = Settings.Log.ConnectionName;
                    }
                    break;
            }

            return System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}
