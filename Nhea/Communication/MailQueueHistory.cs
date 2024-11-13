using System;
using Microsoft.Data.SqlClient;
using Nhea.Utils;

namespace Nhea.Communication
{
    public class MailQueueHistory
    {
        private const string SetMailStatusCommand = "UPDATE nhea_MailQueueHistory SET Status=@Status WHERE Id=@Id";

        public static void SetMailStatus(Guid id, MailStatus status)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new(SetMailStatusCommand, sqlConnection))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@Status", (int)status));
                    cmd.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }
        }
    }
}
