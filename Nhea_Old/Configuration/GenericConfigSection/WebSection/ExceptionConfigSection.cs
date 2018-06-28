using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Nhea.Configuration.GenericConfigSection.WebSection
{
    /// <summary>
    /// Nhea Configuration Exception Handler Class
    /// </summary>
    public class ExceptionConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("enabled", DefaultValue = false, IsRequired = false)]
        public bool Enabled
        {
            get
            {
                if (!String.IsNullOrEmpty(this["enabled"].ToString()))
                {
                    return (bool)this["enabled"];
                }
                else
                {
                    return false;
                }
            }
        }

        [ConfigurationProperty("enableLogging", DefaultValue = false, IsRequired = false)]
        public bool EnableLogging
        {
            get
            {
                if (!String.IsNullOrEmpty(this["enableLogging"].ToString()))
                {
                    return (bool)this["enableLogging"];
                }
                else
                {
                    return false;
                }
            }
        }

        [ConfigurationProperty("errorPage", IsRequired = false)]
        public string ErrorPage
        {
            get
            {
                if (!String.IsNullOrEmpty(this["errorPage"].ToString()))
                {
                    return this["errorPage"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
        }
    }
}
