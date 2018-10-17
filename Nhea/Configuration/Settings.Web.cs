using Nhea.Configuration.GenericConfigSection.WebSection;

namespace Nhea.Configuration
{
    public static partial class Settings
    {
        /// <summary>
        /// Nhea Data Configuration Section Helper Class
        /// </summary>
        public static partial class Web
        {
            private static WebConfigSection config = ConfigFactory.GetConfigurationSection<WebConfigSection>("nhea/web");

            public static bool EnableXssSecurity
            {
                get
                {
                    return config.EnableXssSecurity;
                }
            }

            public static bool EnableLocalization
            {
                get
                {
                    return config.EnableLocalization;
                }
            }

            public static bool UnderMaintenance
            {
                get
                {
                    return config.UnderMaintenance;
                }
            }

            public static string MaintenancePage
            {
                get
                {
                    return config.MaintenancePage;
                }
            }

            internal static string AuthenticationRepository
            {
                get
                {
                    return config.AuthenticationRepository;
                }
            }

            public static bool EnableRenderMeasurement
            {
                get
                {
                    return config.EnableRenderMeasurement;
                }
            }

            public static int RenderMeasurementBaseTime
            {
                get
                {
                    return config.RenderMeasurementBaseTime;
                }
            }

            public static bool EnableIpAuthorization
            {
                get
                {
                    return config.EnableIpAuthorization;
                }
            }

            public static string IpAuthorizationList
            {
                get
                {
                    return config.IpAuthorizationList;
                }
            }

            public static bool SecurePasswords
            {
                get
                {
                    return config.SecurePasswords;
                }
            }

            public static string LocalizationCallbackUrl
            {
                get
                {
                    return config.LocalizationCallbackUrl;
                }
            }

            public static string AuthenticationTable
            {
                get
                {
                    return config.AuthenticationTable;
                }
            }

            public static string GoogleRecaptchaSiteKey
            {
                get
                {
                    return config.GoogleRecaptchaSiteKey;
                }
            }

            public static string GoogleRecaptchaSecretKey
            {
                get
                {
                    return config.GoogleRecaptchaSecretKey;
                }
            }
        }
    }
}
