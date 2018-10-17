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
                    connectionStringName = Settings.Data.ConnectionName;
                    break;
                case ConnectionSource.Communication:
                    connectionStringName = Settings.Communication.ConnectionName;
                    break;
                case ConnectionSource.Log:
                    connectionStringName = Settings.Log.ConnectionName;
                    break;
            }

            return System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}
