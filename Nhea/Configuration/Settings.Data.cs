using Nhea.Configuration.GenericConfigSection.DataSection;

namespace Nhea.Configuration
{
    public static partial class Settings
    {
        internal static NheaDataConfigurationSettings CurrentDataConfigurationSettings { get; set; }

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
                    if (CurrentDataConfigurationSettings != null && !string.IsNullOrEmpty(CurrentDataConfigurationSettings.ConnectionString))
                    {
                        return CurrentDataConfigurationSettings.ConnectionString;
                    }

                    return config.ConnectionName;
                }
            }
        }
    }
}
