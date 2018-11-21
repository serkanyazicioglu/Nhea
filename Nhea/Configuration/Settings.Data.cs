using Nhea.Configuration.GenericConfigSection.DataSection;

namespace Nhea.Configuration
{
    public static partial class Settings
    {
        /// <summary>
        /// Nhea Data Configuration Section Helper Class
        /// </summary>
        public static class Data
        {
            private static DataConfigSection config = ConfigFactory.GetConfigurationSection<DataConfigSection>("nhea/data");

            public static string ConnectionName
            {
                get
                {
                    return config.ConnectionName;
                }
            }
        }
    }
}
