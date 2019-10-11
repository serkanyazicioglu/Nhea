using Nhea.Configuration.GenericConfigSection.ApplicationSection;
using System;

namespace Nhea.Configuration
{
    public enum EnvironmentType
    {
        Development,
        Integration,
        Uat,
        Staging,
        Production
    }

    public static partial class Settings
    {
        /// <summary>
        /// Nhea Data Configuration Section Helper Class
        /// </summary>
        public static class Application
        {
            private static ApplicationConfigSection config = ConfigFactory.GetConfigurationSection<ApplicationConfigSection>("nhea/application");

            public static EnvironmentType EnvironmentType
            {
                get
                {
                    string aspnetCoreEnvironmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    try
                    {
                        if (!string.IsNullOrEmpty(aspnetCoreEnvironmentVariable))
                        {
                            return Nhea.Enumeration.EnumHelper.GetEnum<EnvironmentType>(aspnetCoreEnvironmentVariable);
                        }
                    }
                    catch
                    {
                    }

                    return config.EnvironmentType;
                }
            }
        }
    }
}
