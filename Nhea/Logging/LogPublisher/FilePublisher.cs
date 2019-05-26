using Nhea.Configuration;
using System;
using System.IO;

namespace Nhea.Logging.LogPublisher
{
    public class FilePublisher : Publisher
    {
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

                string detail = String.Format("Date: {0}{7}LogLevel: {1}{7}Message: {2}{7}Source: {3}{7}Username: {4}{7}{7}{5}{6}", DateTime.Now.ToString(), Level.ToString(), Message, Source, UserName, exceptionDetail, exceptionData, Environment.NewLine);

                string logfilename = Nhea.Helper.IOHelper.ToSafeFileName(String.Format(Settings.Log.FileName, DateTime.Now));

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
                sw.WriteLine(detail);
                sw.WriteLine("--------------------------------------");
                sw.WriteLine(Environment.NewLine);
                sw.Close();

                return base.Publish();
            }
            catch (Exception ex)
            {
            }

            return false;
        }
    }
}
