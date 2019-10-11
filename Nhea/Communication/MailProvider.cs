using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nhea.Utils;

namespace Nhea.Communication
{
    public class MailProvider
    {
        private const string SelectCommandText = "SELECT Id, [Name], Url, [Description], PackageSize, StandbyPeriod FROM nhea_MailProvider";

        private static Dictionary<int, MailProvider> providers;
        public static Dictionary<int, MailProvider> Providers
        {
            get
            {
                if (providers == null)
                {
                    providers = new Dictionary<int, MailProvider>();
                    Refresh();
                }

                return providers;
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public int PackageSize { get; set; }

        public int StandbyPeriod { get; set; }

        public static void Refresh()
        {
            Providers.Clear();

            try
            {
                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Communication))
                {
                    using (SqlCommand cmd = new SqlCommand(SelectCommandText, sqlConnection))
                    {
                        cmd.Connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            MailProvider provider = new MailProvider();

                            provider.Id = reader.GetInt32(0);
                            provider.Name = reader.GetString(1);
                            provider.Url = reader.GetString(2);
                            provider.Description = reader.GetString(3);
                            provider.PackageSize = reader.GetInt32(4);
                            provider.StandbyPeriod = reader.GetInt32(5);

                            Providers.Add(provider.Id, provider);
                        }

                        cmd.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                providers = null;
                throw ex;
            }
        }

        /// <summary>
        /// This method return MailProviderId
        /// </summary>
        /// <param name="mail">Email address</param>
        /// <returns>in MailProviderId</returns>
        public static int? Find(string mail)
        {
            string[] mailPart = mail.Trim().Trim(';').Trim(',').Split(new char[] { '@' });

            if (mailPart.Length == 2)
            {
                foreach (KeyValuePair<int, MailProvider> item in Providers)
                {
                    if (item.Value.Url.Contains(mailPart[1]))
                    {
                        return item.Value.Id;
                    }
                }
            }

            return null;
        }
    }
}
