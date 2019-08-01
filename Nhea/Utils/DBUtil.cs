using System;
using System.Data.SqlClient;

namespace Nhea.Utils
{
    public static class DBUtil
    {
        public static SqlConnection CreateConnection()
        {
            return CreateConnection(ConnectionSource.Data);
        }

        public static string CreateConnectionString(ConnectionSource connectionSource)
        {
            string connectionString = ConnectionStringBuilder.Build(connectionSource);

            return ClearConnectionString(connectionString);
        }

        public static SqlConnection CreateConnection(ConnectionSource connectionSource)
        {
            return new SqlConnection(CreateConnectionString(connectionSource));
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
