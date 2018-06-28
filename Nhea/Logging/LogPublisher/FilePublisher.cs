using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using Nhea.Configuration.GenericConfigSection.LogSection;
using Nhea.Configuration;

namespace Nhea.Logging.LogPublisher
{
    public class FilePublisher : Publisher
    {
        #region Publisher Properties

        public override string Message { get; set; }
        public override Exception Exception { get; set; }
        public override LogLevel LogLevel { get; set; }

        #endregion

        private const string FilePathTitle = "Nhea.Logging.FilePath";

        public override bool Publish()
        {
            try
            {
                string exceptionDetail;
                string exceptionData;

                ExceptionDetailBuilder.Build(this.Exception, out exceptionDetail, out exceptionData);

                if (exceptionDetail == null)
                {
                    exceptionDetail = String.Empty;
                }

                if (exceptionData == null)
                {
                    exceptionData = String.Empty;
                }

                string detail = String.Format("Date: {0}{7}LogLevel: {1}{7}Message: {2}{7}Source: {3}{7}Username: {4}{7}{7}Exception Detail:{7}{5}{7}Exception Data:{7}{6}", DateTime.Now.ToString(), LogLevel.ToString(), Message, Source, UserName, exceptionDetail, exceptionData, Environment.NewLine);

                base.Publish();
                
                return PublishToFile(detail);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool PublishToFile(string data)
        {
            if (String.IsNullOrEmpty(Settings.Log.FilePath))
            {
                throw new Exception("Could not initalize FilePath parameter, please check your configurations.");
            }

            string logfilename = "NheaLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            try
            {
                if (!Directory.Exists(Settings.Log.FilePath))
                {
                    Directory.CreateDirectory(Settings.Log.FilePath);
                }

                FileStream file;
                if (!File.Exists(Settings.Log.FilePath + logfilename))
                {
                    file = new FileStream(Settings.Log.FilePath + logfilename, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(data);
                    sw.WriteLine("--------------------------------------");
                    sw.WriteLine(Environment.NewLine);
                    sw.Close();
                }
                else
                {
                    file = new FileStream(Settings.Log.FilePath + logfilename, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(data);
                    sw.WriteLine("--------------------------------------");
                    sw.WriteLine(Environment.NewLine);
                    sw.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
