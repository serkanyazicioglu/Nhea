using System;
using Microsoft.Data.SqlClient;
using Nhea.Utils;

namespace Nhea.Localization
{
    public static class LocalizationManager
    {
        #region Queries

        private const string SelectTranslationWithTargetEntityIdQuery = "SELECT [Translation] FROM [Localization] WHERE [Key] = @Key AND TargetEntityId = @TargetEntityId AND LanguageId = @LanguageId";

        private const string SelectTargetEntityIdQuery = "SELECT [TargetEntityId] FROM [Localization] WHERE [Key] = @Key AND [Translation] = @Translation AND TargetEntityName = @TargetEntityName AND LanguageId = @LanguageId";

        private const string DeleteAllQuery = @"DELETE FROM [Localization] WHERE TargetEntityId = @TargetEntityId;";

        private const string DeleteWithKeyQuery = @"DELETE FROM [Localization] WHERE [Key] = @Key AND TargetEntityId IS NULL AND LanguageId = @LanguageId;";

        private const string DeleteWithTargetEntityIdQuery = @"DELETE FROM [Localization] WHERE [Key] = @Key AND TargetEntityId = @TargetEntityId AND LanguageId = @LanguageId;";

        private const string SaveQuery = @"DECLARE @Counter INT;

SET @Counter = (SELECT COUNT(*) FROM [Localization] WHERE [Key] = @Key AND TargetEntityId = @TargetEntityId AND LanguageId = @LanguageId);

IF(@Counter > 0)
BEGIN
UPDATE [Localization] SET Translation = @Translation, TargetEntityName = @TargetEntityName WHERE [Key] = @Key AND TargetEntityId = @TargetEntityId AND LanguageId = @LanguageId
END
ELSE
BEGIN
INSERT INTO [Localization]([Key],[Translation],[TargetEntityId],[LanguageId],[TargetEntityName])
VALUES (@Key,@Translation,@TargetEntityId,@LanguageId,@TargetEntityName)
END";

        private const string SaveQueryWithoutTargetEntityId = @"DECLARE @Counter INT;

SET @Counter = (SELECT COUNT(*) FROM [Localization] WHERE [Key] = @Key AND LanguageId = @LanguageId);

IF(@Counter > 0)
BEGIN
UPDATE [Localization] SET Translation = @Translation WHERE [Key] = @Key AND LanguageId = @LanguageId
END
ELSE
BEGIN
INSERT INTO [Localization]([Key],[Translation],[TargetEntityId],[LanguageId],[TargetEntityName])
VALUES (@Key,@Translation,NULL,@LanguageId,NULL)
END";

        #endregion

        public static string GetLocalization(string key, int languageId)
        {
            return CatalogManager.GetLocalization(key, languageId);
        }

        public static string GetLocalization(string key, string culture)
        {
            return CatalogManager.GetLocalization(key, culture);
        }

        public static string GetLocalization(string key, Guid targetEntityId, int languageId)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            {
                string selectQuery = SelectTranslationWithTargetEntityIdQuery;

                using (SqlCommand sqlCommand = new(selectQuery, sqlConnection))
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.Parameters.Add(new SqlParameter("@Key", key));
                    sqlCommand.Parameters.Add(new SqlParameter("@LanguageId", languageId));
                    sqlCommand.Parameters.Add(new SqlParameter("@TargetEntityId", targetEntityId));

                    sqlCommand.Connection.Open();

                    object translation = sqlCommand.ExecuteScalar();

                    sqlCommand.Connection.Close();

                    if (translation != null && !string.IsNullOrEmpty(translation.ToString()))
                    {
                        return translation.ToString();
                    }
                }
            }

            return string.Empty;
        }

        public static Guid? GetTargetEntityId(string key, string translation, string targetEntityName, int languageId)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            using (SqlCommand sqlCommand = new(SelectTargetEntityIdQuery, sqlConnection))
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.Parameters.Add(new SqlParameter("@Key", key));
                sqlCommand.Parameters.Add(new SqlParameter("@Translation", translation));
                sqlCommand.Parameters.Add(new SqlParameter("@LanguageId", languageId));
                sqlCommand.Parameters.Add(new SqlParameter("@TargetEntityName", targetEntityName));

                sqlCommand.Connection.Open();

                object targetEntityId = sqlCommand.ExecuteScalar();

                sqlCommand.Connection.Close();

                if (targetEntityId != null && !string.IsNullOrEmpty(targetEntityId.ToString()))
                {
                    return new Guid(targetEntityId.ToString());
                }
            }

            return null;
        }

        public static void SaveLocalization(string translation, string key, Guid targetEntityId, string targetEntityName, int languageId)
        {
            if (!string.IsNullOrEmpty(translation))
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection())
                using (SqlCommand sqlCommand = new(SaveQuery, sqlConnection))
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.Parameters.Add(new SqlParameter("@Translation", translation));
                    sqlCommand.Parameters.Add(new SqlParameter("@Key", key));
                    sqlCommand.Parameters.Add(new SqlParameter("@TargetEntityId", targetEntityId));
                    sqlCommand.Parameters.Add(new SqlParameter("@LanguageId", languageId));
                    sqlCommand.Parameters.Add(new SqlParameter("@TargetEntityName", targetEntityName));

                    sqlCommand.Connection.Open();

                    sqlCommand.ExecuteNonQuery();

                    sqlCommand.Connection.Close();
                }
            }
            else
            {
                DeleteLocalization(targetEntityId, key, languageId);
            }
        }

        public static void SaveLocalization(string translation, string key, int languageId)
        {
            if (!string.IsNullOrEmpty(translation))
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection())
                using (SqlCommand sqlCommand = new(SaveQueryWithoutTargetEntityId, sqlConnection))
                {
                    sqlCommand.Parameters.Clear();

                    sqlCommand.Parameters.Add(new SqlParameter("@Translation", translation));
                    sqlCommand.Parameters.Add(new SqlParameter("@Key", key));
                    sqlCommand.Parameters.Add(new SqlParameter("@LanguageId", languageId));

                    sqlCommand.Connection.Open();

                    sqlCommand.ExecuteNonQuery();

                    sqlCommand.Connection.Close();
                }
            }
            else
            {
                DeleteLocalization(key, languageId);
            }
        }

        public static void DeleteLocalization(Guid targetEntityId)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            using (SqlCommand sqlCommand = new(DeleteAllQuery, sqlConnection))
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.Parameters.Add(new SqlParameter("@TargetEntityId", targetEntityId));

                sqlCommand.Connection.Open();

                sqlCommand.ExecuteNonQuery();

                sqlCommand.Connection.Close();
            }
        }

        public static void DeleteLocalization(string key, int languageId)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            using (SqlCommand sqlCommand = new(DeleteWithKeyQuery, sqlConnection))
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.Parameters.Add(new SqlParameter("@Key", key));
                sqlCommand.Parameters.Add(new SqlParameter("@LanguageId", languageId));

                sqlCommand.Connection.Open();

                sqlCommand.ExecuteNonQuery();

                sqlCommand.Connection.Close();
            }
        }

        public static void DeleteLocalization(Guid targetEntityId, string key, int languageId)
        {
            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            using (SqlCommand sqlCommand = new(DeleteWithTargetEntityIdQuery, sqlConnection))
            {
                sqlCommand.Parameters.Clear();

                sqlCommand.Parameters.Add(new SqlParameter("@TargetEntityId", targetEntityId));
                sqlCommand.Parameters.Add(new SqlParameter("@Key", key));
                sqlCommand.Parameters.Add(new SqlParameter("@LanguageId", languageId));

                sqlCommand.Connection.Open();

                sqlCommand.ExecuteNonQuery();

                sqlCommand.Connection.Close();
            }
        }
    }
}
