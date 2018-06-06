using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;
using Nhea.Logging;
using System.Web;
using System.Configuration;

namespace Nhea.Web.Helper
{
    public static class RequestHelper
    {
        public static string PostHttpRequest(string url)
        {
            return PostHttpRequest(url, String.Empty, null, null, true, null, HttpRequestMethod.GET, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, WebHeaderCollection requestHeaders)
        {
            return PostHttpRequest(url, String.Empty, null, null, true, requestHeaders, HttpRequestMethod.GET, System.Text.Encoding.ASCII);
        }


        public static string PostHttpRequest(string url, WebHeaderCollection requestHeaders, bool byPassSsl)
        {
            return PostHttpRequest(url, String.Empty, null, null, byPassSsl, requestHeaders, HttpRequestMethod.GET, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, HttpRequestMethod httpRequestMethod)
        {
            return PostHttpRequest(url, String.Empty, "application/x-www-form-urlencoded", null, true, null, httpRequestMethod, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, string postData)
        {
            return PostHttpRequest(url, postData, "application/x-www-form-urlencoded", null, true, null, HttpRequestMethod.POST, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, string postData, string contentType)
        {
            return PostHttpRequest(url, postData, contentType, null, true, null, HttpRequestMethod.POST, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, string postData, string contentType, string userAgent)
        {
            return PostHttpRequest(url, postData, contentType, userAgent, true, null, HttpRequestMethod.POST, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, string postData, bool byPassSsl)
        {
            return PostHttpRequest(url, postData, "application/x-www-form-urlencoded", null, byPassSsl, null, HttpRequestMethod.POST, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, string postData, bool byPassSsl, WebHeaderCollection requestHeaders)
        {
            return PostHttpRequest(url, postData, "application/x-www-form-urlencoded", null, byPassSsl, requestHeaders, HttpRequestMethod.POST, System.Text.Encoding.ASCII);
        }

        public static string PostHttpRequest(string url, string postData, bool byPassSsl, WebHeaderCollection requestHeaders, HttpRequestMethod httpRequestMethod, System.Text.Encoding encoding)
        {
            return PostHttpRequest(url, postData, "application/x-www-form-urlencoded", null, byPassSsl, requestHeaders, httpRequestMethod, encoding);
        }

        public static string PostHttpRequest(string url, string postData, string contentType, bool byPassSsl, WebHeaderCollection requestHeaders, HttpRequestMethod httpRequestMethod, System.Text.Encoding encoding)
        {
            return PostHttpRequest(url, postData, contentType, null, byPassSsl, requestHeaders, httpRequestMethod, encoding);
        }

        public static string PostHttpRequest(string url, string postData, string contentType, string userAgent, bool byPassSsl, WebHeaderCollection requestHeaders, HttpRequestMethod httpRequestMethod, System.Text.Encoding encoding)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = httpRequestMethod.ToString();
            //request.Proxy = null;

            if (!String.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }

            if (byPassSsl && url.StartsWith(UrlScheme.Https.ToString()))
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }

            if (requestHeaders != null)
            {
                request.Headers.Add(requestHeaders);
            }

            if (httpRequestMethod == HttpRequestMethod.POST)
            {
                request.ContentType = contentType;
                request.ContentLength = postData.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    StreamWriter streamWriter = new StreamWriter(requestStream, encoding);
                    streamWriter.Write(postData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();

                StreamReader responseReader = new StreamReader(responseStream);
                return responseReader.ReadToEnd();
            }
        }

        public static string PostJsonRequest(string url, string postData)
        {
            return PostJsonRequest(url, postData, null);
        }

        public static string PostJsonRequest(string url, string postData, WebHeaderCollection requestHeaders)
        {
            postData = postData.Replace(Environment.NewLine, String.Empty);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Proxy = null;

            if (requestHeaders != null)
            {
                request.Headers.Add(requestHeaders);
            }

            request.ContentType = "application/json";

            byte[] bdata = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = bdata.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bdata, 0, bdata.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();

                StreamReader responseReader = new StreamReader(responseStream);
                return responseReader.ReadToEnd();
            }
        }

        public static string PostSocketRequest(string host, int port, string postData)
        {
            TcpClient tcpClient;
            NetworkStream networkStream;
            StreamReader streamReader;
            StreamWriter streamWriter;

            try
            {
                tcpClient = new TcpClient(host, port);
                networkStream = tcpClient.GetStream();
                streamReader = new StreamReader(networkStream);
                streamWriter = new StreamWriter(networkStream);
                streamWriter.WriteLine(postData);
                streamWriter.Flush();
                string result = streamReader.ReadLine();

                streamReader.Close();
                streamReader.Dispose();
                streamWriter.Close();
                streamWriter.Dispose();
                networkStream.Close();
                networkStream.Dispose();

                return result;
            }
            finally
            {
                streamReader = null;
                streamWriter = null;
                networkStream = null;
            }
        }

        public static void PostSocketRequest(string host, int port, byte[] postData)
        {
            Socket m_socWorker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(host);
            System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, port);
            m_socWorker.Connect(remoteEP);

            m_socWorker.Send(postData);


        }

        public static string GetCurrentIp()
        {
            string ip;

            ip = HttpContext.Current.Request["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request["REMOTE_ADDR"];
            }

            return ip;
        }
    }
}
