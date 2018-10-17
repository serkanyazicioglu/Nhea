using Nhea.Configuration.GenericConfigSection.WebSection;

namespace Nhea.Configuration
{
    /// <summary>
    /// Nhea Configuration Section Helper Class
    /// </summary>
    public static partial class Settings
    {
        public static partial class Web
        {
            /// <summary>
            /// Nhea Exception Configuration Section Helper Class
            /// </summary>
            public static class Exception
            {
                private static ExceptionConfigSection config = ConfigFactory.GetConfigurationSection<ExceptionConfigSection>("nhea/exception");

                /// <summary>
                /// Gets Enabled status
                /// </summary>
                public static bool Enabled
                {
                    get
                    {
                        return (bool)config.Enabled;
                    }
                }

                /// <summary>
                /// Gets default error page
                /// </summary>
                public static string ErrorPage
                {
                    get
                    {
                        return config.ErrorPage;
                    }
                }

                /// <summary>
                /// Gets Enabled status
                /// </summary>
                public static bool EnableLogging
                {
                    get
                    {
                        return (bool)config.EnableLogging;
                    }
                }
            }
        }
    }
}