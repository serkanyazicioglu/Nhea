using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Nhea.Configuration.GenericConfigSection.ApplicationSection
{
    /// <summary>
    /// Nhea Configuration Connection Class
    /// </summary>
    internal class ApplicationConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("environmentType", DefaultValue = EnvironmentType.Production)]
        public EnvironmentType EnvironmentType
        {
            get
            {
                return (EnvironmentType)Enum.Parse(typeof(EnvironmentType), this["environmentType"].ToString());
            }
        }

        [ConfigurationProperty("name", DefaultValue = "")]
        public string Name
        {
            get
            {
                return this["name"].ToString();
            }
        }
    }
}
