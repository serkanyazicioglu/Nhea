using Nhea.Logging;
using Nhea.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Nhea.Communication
{
    public static class NotificationQueue
    {
        private const string InsertCommandText = @"INSERT INTO [dbo].[nhea_NotificationQueue] ([DeviceToken],[NotificationText],[NotificationBadge],[NotificationSound],[DeviceTypeId],[ApplicationId],[CustomItem]) VALUES(@DeviceToken,@NotificationText,@NotificationBadge,@NotificationSound,@DeviceTypeId,@ApplicationId,@CustomItem);";

        public static bool Add(DeviceType deviceType, string deviceToken, string notificationText)
        {
            return Add(deviceType, deviceToken, notificationText, 0, null, null);
        }

        public static bool Add(DeviceType deviceType, string deviceToken, string notificationText, string customItem)
        {
            return Add(deviceType, deviceToken, notificationText, 0, customItem, null);
        }

        public static bool Add(DeviceType deviceType, string deviceToken, string notificationText, int notificationBadge)
        {
            return Add(deviceType, deviceToken, notificationText, notificationBadge, null, null);
        }

        public static bool Add( DeviceType deviceType, string deviceToken, string notificationText, int notificationBadge, string customItem)
        {
            return Add(deviceType, deviceToken, notificationText, notificationBadge, customItem, null);
        }

        public static bool Add(DeviceType deviceType, string deviceToken, string notificationText, int notificationBadge, string customItem, string notificationSound)
        {
            try
            {
                if (String.IsNullOrEmpty(notificationSound))
                {
                    notificationSound = "sound.caf";
                }

                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                using (SqlCommand cmd = new SqlCommand(InsertCommandText, sqlConnection))
                {
                    cmd.Connection.Open();

                    cmd.Parameters.Add(new SqlParameter("@DeviceToken", deviceToken));
                    cmd.Parameters.Add(new SqlParameter("@NotificationText", notificationText));
                    cmd.Parameters.Add(new SqlParameter("@NotificationBadge", notificationBadge));
                    cmd.Parameters.Add(new SqlParameter("@NotificationSound", notificationSound));
                    cmd.Parameters.Add(new SqlParameter("@DeviceTypeId", (int)deviceType));
                    cmd.Parameters.Add(new SqlParameter("@ApplicationId", Nhea.Configuration.Settings.Communication.NotificationsApplicationId));

                    if (!String.IsNullOrEmpty(customItem))
                    {
                        cmd.Parameters.Add(new SqlParameter("@CustomItem", customItem));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@CustomItem", DBNull.Value));
                    }

                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, false);

                return false;
            }
        }
        
        public static bool Add(List<NotificationInfo> notificationInfoList)
        {
            try
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                {
                    SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection);

                    DataTable dataTable = new DataTable();

                    DataColumn idColumn = new DataColumn("Id", typeof(Guid));
                    dataTable.Columns.Add(idColumn);
                    sqlBulkCopy.ColumnMappings.Add("Id", "Id");

                    DataColumn deviceTokenColumn = new DataColumn("DeviceToken", typeof(string));
                    dataTable.Columns.Add(deviceTokenColumn);
                    sqlBulkCopy.ColumnMappings.Add("DeviceToken", "DeviceToken");

                    DataColumn notTextColumn = new DataColumn("NotificationText", typeof(string));
                    dataTable.Columns.Add(notTextColumn);
                    sqlBulkCopy.ColumnMappings.Add("NotificationText", "NotificationText");

                    DataColumn notBadgeColumn = new DataColumn("NotificationBadge", typeof(string));
                    dataTable.Columns.Add(notBadgeColumn);
                    sqlBulkCopy.ColumnMappings.Add("NotificationBadge", "NotificationBadge");

                    DataColumn notSoundColumn = new DataColumn("NotificationSound", typeof(string));
                    dataTable.Columns.Add(notSoundColumn);
                    sqlBulkCopy.ColumnMappings.Add("NotificationSound", "NotificationSound");

                    DataColumn deviceTypeIdColumn = new DataColumn("DeviceTypeId", typeof(int));
                    dataTable.Columns.Add(deviceTypeIdColumn);
                    sqlBulkCopy.ColumnMappings.Add("DeviceTypeId", "DeviceTypeId");

                    DataColumn applicationIdColumn = new DataColumn("ApplicationId", typeof(Guid));
                    dataTable.Columns.Add(applicationIdColumn);
                    sqlBulkCopy.ColumnMappings.Add("ApplicationId", "ApplicationId");

                    DataColumn customItemColumn = new DataColumn("CustomItem", typeof(string));
                    dataTable.Columns.Add(customItemColumn);
                    sqlBulkCopy.ColumnMappings.Add("CustomItem", "CustomItem");

                    DataColumn deviceKeyColumn = new DataColumn("DeviceKey", typeof(string));
                    dataTable.Columns.Add(deviceKeyColumn);
                    sqlBulkCopy.ColumnMappings.Add("DeviceKey", "DeviceKey");

                    DataColumn deviceAuthColumn = new DataColumn("DeviceAuth", typeof(string));
                    dataTable.Columns.Add(deviceAuthColumn);
                    sqlBulkCopy.ColumnMappings.Add("DeviceAuth", "DeviceAuth");

                    foreach (var notificationInfo in notificationInfoList)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["Id"] = Guid.NewGuid();
                        dataRow["DeviceToken"] = notificationInfo.DeviceToken;
                        dataRow["NotificationText"] = notificationInfo.NotificationText;
                        dataRow["NotificationBadge"] = notificationInfo.NotificationBadge;

                        if (String.IsNullOrEmpty(notificationInfo.NotificationSound))
                        {
                            dataRow["NotificationSound"] = "sound.caf";
                        }
                        else
                        {
                            dataRow["NotificationSound"] = notificationInfo.NotificationSound;
                        }
                        
                        dataRow["DeviceTypeId"] = notificationInfo.DeviceTypeId;
                        dataRow["ApplicationId"] = Nhea.Configuration.Settings.Communication.NotificationsApplicationId;

                        if (!String.IsNullOrEmpty(notificationInfo.CustomItem))
                        {
                            dataRow["CustomItem"] = notificationInfo.CustomItem;
                        }

                        dataRow["DeviceKey"] = notificationInfo.DeviceKey;
                        dataRow["DeviceAuth"] = notificationInfo.DeviceAuth;

                        dataTable.Rows.Add(dataRow);
                    }

                    sqlConnection.Open();
                    sqlBulkCopy.DestinationTableName = "nhea_NotificationQueue";
                    sqlBulkCopy.BulkCopyTimeout = 1800;
                    sqlBulkCopy.WriteToServer(dataTable);
                    sqlConnection.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex, false);

                return false;
            }
        }
    }
}
