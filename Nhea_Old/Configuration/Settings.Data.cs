using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            public static string DataTextField
            {
                get
                {
                    return config.DataTextField;
                }
            }

            public static string DataValueField
            {
                get
                {
                    return config.DataValueField;
                }
            }

            public static bool LogDirtyFields
            {
                get
                {
                    return config.LogDirtyFields;
                }
            }

            public static DataPreserveType DataPreserveType
            {
                get
                {
                    return config.DataPreserveType;
                }
            }
        }
    }
}
