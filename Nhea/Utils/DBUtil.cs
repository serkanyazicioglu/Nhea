using Microsoft.Data.SqlClient;

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
            return new SqlConnection(ClearConnectionString(ConnectionStringBuilder.Build(connectionSource)));
        }

        private static string ClearConnectionString(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
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