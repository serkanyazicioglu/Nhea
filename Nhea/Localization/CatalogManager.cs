using Nhea.Logging;
using Nhea.Utils;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace Nhea.Localization
{
    internal static class CatalogManager
    {
        private const string LocalizationSelectCommandText = @"SELECT [Localization].[Key], [Localization].[Translation], 
[Localization].[LanguageId],
[Language].Title AS LanguageTitle, [Language].Culture, [Language].TwoLetterIsoName
FROM [Localization]
INNER JOIN [Language] ON [Language].Id = [Localization].LanguageId
WHERE [Language].Status = 1 AND [TargetEntityId] IS NULL";

        private static readonly object lockObject = new();

        private static List<Catalog> currentCatalog;
        internal static List<Catalog> CurrentCatalog
        {
            get
            {
                try
                {
                    if (currentCatalog == null)
                    {
                        lock (lockObject)
                        {
                            if (currentCatalog == null)
                            {
                                var catalogList = new List<Catalog>();

                                using (SqlConnection sqlConnection = DBUtil.CreateConnection())
                                {
                                    using (SqlCommand cmd = new(LocalizationSelectCommandText, sqlConnection))
                                    {
                                        cmd.Connection.Open();
                                        SqlDataReader reader = cmd.ExecuteReader();

                                        while (reader.Read())
                                        {
                                            catalogList.Add(new Catalog
                                            {
                                                Key = reader.GetString(0),
                                                Translation = reader.GetString(1),
                                                LanguageId = reader.GetInt32(2),
                                                LanguageTitle = reader.GetString(3),
                                                Culture = reader.GetString(4),
                                                TwoLetterIsoLanguageName = reader.GetString(5)
                                            });
                                        }

                                        reader.Close();
                                        cmd.Connection.Close();
                                    }

                                    currentCatalog = catalogList;
                                }
                            }
                        }
                    }

                    return currentCatalog;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    return null;
                }
            }
            set
            {
                currentCatalog = value;
            }
        }

        internal static string GetLocalization(string key, string culture)
        {
            try
            {
                Catalog catalog = CurrentCatalog.Where(catalogQuery => (catalogQuery.Culture == culture || catalogQuery.TwoLetterIsoLanguageName == culture) && catalogQuery.Key == key).SingleOrDefault();

                if (catalog != null)
                {
                    return catalog.Translation;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Culture", culture);
                ex.Data.Add("Key", key);
                Logger.Log(ex);
            }

            return key;
        }

        internal static string GetLocalization(string key, int languageId)
        {
            try
            {
                Catalog catalog = CurrentCatalog.Where(catalogQuery => catalogQuery.LanguageId == languageId && catalogQuery.Key == key).SingleOrDefault();

                if (catalog != null)
                {
                    return catalog.Translation;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("LanguageId", languageId.ToString());
                ex.Data.Add("Key", key);
                Logger.Log(ex);
            }

            return string.Empty;
        }
    }
}
