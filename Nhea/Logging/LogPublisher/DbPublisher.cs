using Nhea.Text;
using Nhea.Utils;
using System;
using System.Data.SqlClient;

namespace Nhea.Logging.LogPublisher
{
    public class DbPublisher : Publisher
    {
        #region Publisher Properties

        public override string Message { get; set; }
        public override Exception Exception { get; set; }
        public override LogLevel LogLevel { get; set; }

        #endregion

        #region Publisher Methods

        public override bool Publish()
        {
            try
            {
                string exceptionFile;
                string exceptionDetail;
                string exceptionData;

                ExceptionDetailBuilder.Build(this.Exception, out exceptionDetail, out exceptionData, out exceptionFile);

                string insertCommandText = @"INSERT INTO nhea_Log([Source], [UserName], [Level], [Message], [ExceptionFile], [Exception], [ExceptionData]) 
                                                VALUES(@Source, @UserName, @Level, @Message, @ExceptionFile, @Exception, @ExceptionData)";

                using (SqlConnection sqlConnection = DBUtil.CreateConnection(ConnectionSource.Log))
                {
                    using (SqlCommand cmd = new SqlCommand(insertCommandText, sqlConnection))
                    {
                        cmd.Parameters.Clear();

                        #region BuildParameters

                        cmd.Parameters.Add(new SqlParameter("@Level", StringHelper.SplitText(this.LogLevel.ToString(), 50)));

                        if (this.Message == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@Message", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@Message", StringHelper.SplitText(this.Message, 4000)));
                        }

                        if (Source == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@Source", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@Source", StringHelper.SplitText(Source, 255)));
                        }

                        if (UserName == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@UserName", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@UserName", StringHelper.SplitText(UserName, 255)));
                        }

                        if (!String.IsNullOrEmpty(exceptionDetail))
                        {
                            cmd.Parameters.Add(new SqlParameter("@Exception", StringHelper.SplitText(exceptionDetail, 8000)));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@Exception", DBNull.Value));
                        }

                        if (!String.IsNullOrEmpty(exceptionFile))
                        {
                            cmd.Parameters.Add(new SqlParameter("@ExceptionFile", StringHelper.SplitText(exceptionFile, 4000)));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ExceptionFile", DBNull.Value));
                        }

                        if (!String.IsNullOrEmpty(exceptionData))
                        {
                            cmd.Parameters.Add(new SqlParameter("@ExceptionData", StringHelper.SplitText(exceptionData, 4000)));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ExceptionData", DBNull.Value));
                        }

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }

                return base.Publish();
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, PublishTypes.File, null, null, ex.Message, ex, false);
                Logger.Log(LogLevel.Error, PublishTypes.File, this.Source, this.UserName, this.Message, this.Exception, false);

                return false;
            }
        }

        #endregion
    }
}
