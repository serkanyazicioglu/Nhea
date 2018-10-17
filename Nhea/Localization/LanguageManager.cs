using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nhea.Utils;

namespace Nhea.Localization
{
    public static class LanguageManager
    {
        private const string LanguageSelectCommandText = @"SELECT [Id], [Title], [Culture], [TwoLetterIsoName], [CurrencyCode], [CurrencyShortCode], [CurrencyCodeNo]
FROM [Language]
WHERE [Language].Status = 1";
        private static object lockObject = new object();

        private static List<Language> currentLanguages;
        public static List<Language> CurrentLanguages
        {
            get
            {
                if (currentLanguages == null)
                {
                    lock (lockObject)
                    {
                        if (currentLanguages == null)
                        {
                            List<Language> catalogList = new List<Language>();

                            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
                            {
                                using (SqlCommand cmd = new SqlCommand(LanguageSelectCommandText, sqlConnection))
                                {
                                    cmd.Connection.Open();
                                    SqlDataReader reader = cmd.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        Language language = new Language();
                                        language.Id = reader.GetInt32(0);
                                        language.Title = reader.GetString(1);
                                        language.Culture = reader.GetString(2);
                                        language.TwoLetterIsoLanguageName = reader.GetString(3);
                                        language.CurrencyCode = reader.GetString(4);
                                        language.CurrencyShortCode = reader.GetString(5);
                                        language.CurrencyCodeNo = reader.GetInt32(6);

                                        catalogList.Add(language);
                                    }

                                    reader.Close();
                                    cmd.Connection.Close();
                                }

                                currentLanguages = catalogList;
                            }
                        }
                    }
                }

                return currentLanguages;
            }
            set
            {
                currentLanguages = value;
            }
        }

        public static int FindLanguageId(string culture)
        {
            string commandText = @"SELECT Id From Language WHERE Culture = @Culture OR TwoLetterIsoName = @Culture";

            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(commandText, sqlConnection))
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add(new SqlParameter("@Culture", culture));

                    cmd.Connection.Open();
                    object languageId = cmd.ExecuteScalar();
                    cmd.Connection.Close();

                    if (languageId != null)
                    {
                        return (int)languageId;
                    }
                }
            }

            return 0;
        }

        public static string FindLanguage(int id)
        {
            string commandText = @"SELECT Title From Language WHERE Id = @Id";

            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(commandText, sqlConnection))
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add(new SqlParameter("@Id", id.ToString()));

                    cmd.Connection.Open();
                    object language = cmd.ExecuteScalar();
                    cmd.Connection.Close();

                    if (language != null)
                    {
                        return language.ToString();
                    }
                }
            }

            return null;
        }

        public static string FindLanguageShortCulture(int id)
        {
            string commandText = @"SELECT TwoLetterIsoName From Language WHERE Id = @Id";

            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(commandText, sqlConnection))
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add(new SqlParameter("@Id", id.ToString()));

                    cmd.Connection.Open();
                    object culture = cmd.ExecuteScalar();
                    cmd.Connection.Close();

                    if (culture != null)
                    {
                        return culture.ToString();
                    }
                }
            }

            return null;
        }

        private const string usedLanguagesCommandText = "SELECT DISTINCT Language.Id, Language.Title FROM Language INNER JOIN Localization ON Localization.LanguageId = Language.Id WHERE Localization.TargetEntityId = @TargetEntityId";

        private const string unusedLanguagesCommandText = "SELECT DISTINCT Language.Id, Language.Title FROM Language WHERE Id NOT IN(SELECT Localization.LanguageId FROM Localization WHERE Localization.TargetEntityId = @TargetEntityId)";

        public static List<Language> GetTargetLanguages(Guid targetEntityId, bool usedLanguages)
        {
            string commandText;

            if (usedLanguages)
            {
                commandText = usedLanguagesCommandText;
            }
            else
            {
                commandText = unusedLanguagesCommandText;
            }

            List<Language> targetLanguages = new List<Language>();

            using (SqlConnection sqlConnection = DBUtil.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand(commandText, sqlConnection))
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add(new SqlParameter("@TargetEntityId", targetEntityId.ToString()));

                    cmd.Connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Language language = new Language();

                        language.Id = reader.GetInt32(0);
                        language.Title = reader.GetString(1);

                        targetLanguages.Add(language);
                    }
                }
            }

            return targetLanguages;
        }
    }
}
