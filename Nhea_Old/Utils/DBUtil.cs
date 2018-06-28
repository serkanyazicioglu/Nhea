using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using Nhea.Configuration.GenericConfigSection;
using Nhea.Configuration;
using Nhea.Configuration.GenericConfigSection.DataSection;

namespace Nhea.Utils
{
    public static class DBUtil
    {
        public static SqlConnection CreateConnection()
        {
            return CreateConnection(ConnectionSource.Data);
        }

        public static SqlConnection CreateConnection(ConnectionSource connectionSource)
        {
            string connectionString = ConnectionStringBuilder.Build(connectionSource);

            connectionString = ClearConnectionString(connectionString);

            return new SqlConnection(connectionString);
        }

        public static string ClearConnectionString(string connectionString)
        {
            if (!String.IsNullOrEmpty(connectionString))
            {
                if (connectionString.StartsWith("metadata"))
                {
                    int dataSourceIndex = connectionString.ToLower().LastIndexOf("data source");

                    connectionString = connectionString.Substring(dataSourceIndex).Trim('"');
                }
            }

            return connectionString;
        }
    }
}
