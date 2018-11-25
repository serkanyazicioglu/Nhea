using System;
using System.Configuration;
using Nhea.Enumeration;

namespace Nhea.Configuration.GenericConfigSection.DataSection
{
    /// <summary>
    /// Nhea Configuration Connection Class
    /// </summary>
    internal class DataConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("connectionName", IsRequired = false)]
        public string ConnectionName
        {
            get
            {
                if (!String.IsNullOrEmpty(this["connectionName"].ToString()))
                {
                    return this["connectionName"].ToString();
                }
                else
                {
                    throw new Exception("Connection string property has not been initalized!");
                }
            }
        }
    }
}
