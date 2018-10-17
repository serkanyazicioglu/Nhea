using System;
using System.Configuration;

namespace Nhea.Configuration.GenericConfigSection.WebSection
{
    /// <summary>
    /// Nhea Configuration Logging Handler Class
    /// </summary>
    public class WebConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("enableXssSecurity", DefaultValue = true)]
        public bool EnableXssSecurity
        {
            get
            {
                return Convert.ToBoolean(this["enableXssSecurity"]);
            }
        }

        [ConfigurationProperty("enableLocalization", DefaultValue = false)]
        public bool EnableLocalization
        {
            get
            {
                return Convert.ToBoolean(this["enableLocalization"]);
            }
        }

        [ConfigurationProperty("underMaintenance", DefaultValue = false)]
        public bool UnderMaintenance
        {
            get
            {
                return Convert.ToBoolean(this["underMaintenance"]);
            }
        }

        [ConfigurationProperty("maintenancePage", IsRequired = false)]
        public string MaintenancePage
        {
            get
            {
                return this["maintenancePage"].ToString();
            }
        }

        [ConfigurationProperty("authenticationRepository", IsRequired = false)]
        public string AuthenticationRepository
        {
            get
            {
                return this["authenticationRepository"].ToString();
            }
        }

        [ConfigurationProperty("enableRenderMeasurement", DefaultValue = false, IsRequired = false)]
        public bool EnableRenderMeasurement
        {
            get
            {
                return (bool)this["enableRenderMeasurement"];
            }
        }

        [ConfigurationProperty("renderMeasurementBaseTime", IsRequired = false)]
        public int RenderMeasurementBaseTime
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this["renderMeasurementBaseTime"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
        }

        [ConfigurationProperty("enableIpAuthorization", DefaultValue = false, IsRequired = false)]
        public bool EnableIpAuthorization
        {
            get
            {
                return (bool)this["enableIpAuthorization"];
            }
        }

        [ConfigurationProperty("securePasswords", DefaultValue = false, IsRequired = false)]
        public bool SecurePasswords
        {
            get
            {
                return (bool)this["securePasswords"];
            }
        }

        [ConfigurationProperty("ipAuthorizationList", IsRequired = false)]
        public string IpAuthorizationList
        {
            get
            {
                return this["ipAuthorizationList"].ToString();
            }
        }

        [ConfigurationProperty("localizationCallbackUrl", IsRequired = false)]
        public string LocalizationCallbackUrl
        {
            get
            {
                return this["localizationCallbackUrl"].ToString();
            }
        }

        [ConfigurationProperty("authenticationTable", IsRequired = false)]
        public string AuthenticationTable
        {
            get
            {
                if (this["authenticationTable"] != null && !String.IsNullOrEmpty(this["authenticationTable"].ToString()))
                {
                    return this["authenticationTable"].ToString();
                }
                else
                {
                    return "Member";
                }
            }
        }

        [ConfigurationProperty("googleRecaptchaSiteKey", IsRequired = false)]
        public string GoogleRecaptchaSiteKey
        {
            get
            {
                return this["googleRecaptchaSiteKey"].ToString();
            }
        }

        [ConfigurationProperty("googleRecaptchaSecretKey", IsRequired = false)]
        public string GoogleRecaptchaSecretKey
        {
            get
            {
                return this["googleRecaptchaSecretKey"].ToString();
            }
        }
    }
}
