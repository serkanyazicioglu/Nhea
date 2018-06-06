using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Nhea.Enumeration;

namespace Nhea.Configuration.GenericConfigSection.DataSection
{
    /// <summary>
    /// Nhea Configuration Connection Class
    /// </summary>
    internal class DataConfigSection : ConfigurationSection
    {
        private const string DefaultDataValueField = "Id";

        private const string DefaultDataTextField = "Title";

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

        [ConfigurationProperty("dataTextField", IsRequired = false)]
        public string DataTextField
        {
            get
            {
                if (!String.IsNullOrEmpty(this["dataTextField"].ToString()))
                {
                    return this["dataTextField"].ToString();
                }
                else
                {
                    return DefaultDataTextField;
                }
            }
        }

        [ConfigurationProperty("dataValueField", IsRequired = false)]
        public string DataValueField
        {
            get
            {
                if (!String.IsNullOrEmpty(this["dataValueField"].ToString()))
                {
                    return this["dataValueField"].ToString();
                }
                else
                {
                    return DefaultDataValueField;
                }
            }
        }

        [ConfigurationProperty("logDirtyFields", DefaultValue = false)]
        public bool LogDirtyFields
        {
            get
            {
                return Convert.ToBoolean(this["logDirtyFields"]);
            }
        }

        [ConfigurationProperty("dataPreserveType", DefaultValue = DataPreserveType.QueryString)]
        public DataPreserveType DataPreserveType
        {
            get
            {
                return EnumHelper.GetEnum<DataPreserveType>(this["dataPreserveType"]);
            }
        }
    }

    public enum DataPreserveType
    {
        QueryString,
        Session
    }
}
