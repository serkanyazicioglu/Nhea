using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Nhea.Enumeration;

namespace Nhea.Web.Helper
{
    public enum UrlScheme
    {
        [Detail("http")]
        Http,
        [Detail("https")]
        Https,
        [Detail("ftp")]
        Ftp
    }

    public static class UrlHelper
    {
        #region Variables

        internal const string RootDelimeter = "~/";

        private const string PortDelimeter = ":";

        #endregion

        /// <summary>
        /// Returns the application path by checking the environment.
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationPath()
        {
            if (!HttpContext.Current.Request.ApplicationPath.Equals("/"))
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Returns url of the current request.
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            return GetUrl(true, false, false);
        }

        /// <summary>
        /// Returns url of the current request.
        /// </summary>
        /// <param name="getScheme">Inserts scheme (http, https, ftp etc...) into the returning value.</param>
        /// <returns></returns>
        public static string GetUrl(bool getScheme)
        {
            return GetUrl(getScheme, false, false);
        }

        /// <summary>
        /// Returns url of the current request.
        /// </summary>
        /// <param name="getScheme">Inserts scheme (http, https, ftp...) into the returning value.</param>
        /// <param name="getPort">Inserts port into the returning value.</param>
        /// <returns></returns>
        public static string GetUrl(bool getScheme, bool getPort)
        {
            return GetUrl(getScheme, getPort, false);
        }

        /// <summary>
        /// Returns url of the current request.
        /// </summary>
        /// <param name="getScheme">Inserts scheme (http, https, ftp...) into the returning value.</param>
        /// <param name="getPort">Inserts port into the returning value.</param>
        /// <param name="getDirectories">Inserts sub folder names into the returning value.</param>
        /// <returns></returns>
        public static string GetUrl(bool getScheme, bool getPort, bool getDirectories)
        {
            HttpRequest request = HttpContext.Current.Request;

            string url = String.Empty;

            if (getScheme)
            {
                url = String.Concat(url, request.Url.Scheme, Uri.SchemeDelimiter);
            }

            url = String.Concat(url, HttpContext.Current.Request.Url.Host);

            if (getPort)
            {
                url = String.Concat(url, PortDelimeter, HttpContext.Current.Request.Url.Port);
            }

            string applicationPath = GetApplicationPath();

            if (!String.IsNullOrEmpty(applicationPath))
            {
                url = String.Concat(url, applicationPath);
            }

            if (getDirectories)
            {
                string directory = request.AppRelativeCurrentExecutionFilePath.Substring(0, request.AppRelativeCurrentExecutionFilePath.LastIndexOf('/') + 1).Replace("~", String.Empty);

                url = String.Concat(url, directory);
            }

            if (!url.EndsWith("/"))
            {
                url = String.Concat(url, "/");
            }

            return url;
        }

        public static string ConvertToRootUrl(string url)
        {
            url = url.Trim();

            if (!url.StartsWith(UrlHelper.RootDelimeter))
            {
                url = UrlHelper.RootDelimeter + url;
            }

            return url;
        }

        public static string ConvertToAbsoluteUrl(string url)
        {
            url = ConvertToRootUrl(url);

            return VirtualPathUtility.ToAbsolute(url);
        }
    }
}
