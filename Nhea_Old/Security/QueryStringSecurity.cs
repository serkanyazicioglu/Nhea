using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace Nhea.Security
{
    public class QueryStringSecurity
    {
        private const string DefaultSalt = "nhea_hash";
        private const string Key = "&v=";

        /// <summary>
        /// Hashes the querysting part of a url and adds to the end of the url.
        /// </summary>
        /// <param name="url">Url with a querystring to be hashed.</param>
        /// <returns>Returns full url with hashed string.</returns>
        public static string HashQueryString(string url)
        {
            return HashQueryString(url, DefaultSalt);
        }

        /// <summary>
        /// Hashes the querysting part of a url and adds to the end of the url.
        /// </summary>
        /// <param name="url">Url with a querystring to be hashed.</param>
        /// <param name="salt">A unique key for using hash algorith. Must be same key within the Check method.</param>
        /// <returns>Returns full url with hashed string.</returns>
        public static string HashQueryString(string url, string salt)
        {
            string queryString;
            if (url.Contains("?"))
            {
                queryString = url.Substring(url.IndexOf("?") + 1);
            }
            else
            {
                queryString = url;
            }

            string hashedString = salt + queryString + salt;
            hashedString = hashedString.GetHashCode().ToString();

            if (url.Contains("?"))
            {
                return url.Substring(0, url.IndexOf("?") + 1) + queryString + Key + hashedString;
            }
            else
            {
                return queryString + Key + hashedString;
            }
        }

        public static bool CheckHashedQueryString(string url)
        {
            return CheckHashedQueryString(url, DefaultSalt);
        }

        public static bool CheckHashedQueryString(string url, string salt)
        {
            string queryString = url.Substring(url.IndexOf("?") + 1);
            return CheckHashedQueryString(HttpUtility.ParseQueryString(queryString), salt);
        }

        public static bool CheckHashedQueryString(NameValueCollection queryString)
        {
            return CheckHashedQueryString(queryString, DefaultSalt);
        }

        public static bool CheckHashedQueryString(NameValueCollection queryString, string salt)
        {
            try
            {
                string baseQueryString = String.Empty;
                string hashedString = String.Empty;

                string[] queryStringArray = queryString.ToString().Split(new string[] { Key }, StringSplitOptions.None);

                baseQueryString = queryStringArray[0];
                hashedString = Key + queryStringArray[1];

                if (HashQueryString(baseQueryString, salt) == baseQueryString + hashedString)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}