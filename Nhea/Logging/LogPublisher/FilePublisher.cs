using Nhea.Configuration;
using System;
using System.IO;

namespace Nhea.Logging.LogPublisher
{
    public class FilePublisher : Publisher
    {
        #region Publisher Properties

        public override string Message { get; set; }
        public override Exception Exception { get; set; }
        public override LogLevel LogLevel { get; set; }

        #endregion

        public override bool Publish()
        {
            try
            {
                string exceptionDetail = String.Empty;
                string exceptionData = String.Empty;

                if (this.Exception != null)
                {
                    ExceptionDetailBuilder.Build(this.Exception, out exceptionDetail, out exceptionData);

                    if (!String.IsNullOrEmpty(exceptionDetail))
                    {
                        exceptionDetail = "Exception Detail:" + Environment.NewLine + exceptionDetail + Environment.NewLine;
                    }

                    if (exceptionData == null)
                    {
                        exceptionData = "Exception Data:" + Environment.NewLine + exceptionDetail + Environment.NewLine;
                    }
                }

                string detail = String.Format("Date: {0}{7}LogLevel: {1}{7}Message: {2}{7}Source: {3}{7}Username: {4}{7}{7}{5}{6}", DateTime.Now.ToString(), LogLevel.ToString(), Message, Source, UserName, exceptionDetail, exceptionData, Environment.NewLine);

                base.Publish();

                return PublishToFile(detail);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, PublishTypes.File, null, null, ex.Message, ex, false);
                Logger.Log(LogLevel.Error, PublishTypes.File, this.Source, this.UserName, this.Message, this.Exception, false);
                return false;
            }
        }

        private bool PublishToFile(string data)
        {
            string logfilename = Nhea.Helper.IOHelper.ToSafeFileName(String.Format(Settings.Log.FileName, DateTime.Now));

            try
            {
                string directoryPath = Nhea.Helper.IOHelper.ToSafeDirectoryPath(Settings.Log.DirectoryPath);

                if (!String.IsNullOrEmpty(directoryPath))
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                }

                string filePath = Path.Combine(directoryPath, logfilename);

                var fileMode = FileMode.Append;

                if (!File.Exists(filePath))
                {
                    fileMode = FileMode.CreateNew;
                }

                var file = new FileStream(filePath, fileMode, FileAccess.Write);
                StreamWriter sw = new StreamWriter(file);
                sw.WriteLine(data);
                sw.WriteLine("--------------------------------------");
                sw.WriteLine(Environment.NewLine);
                sw.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
