using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;

namespace Nhea.Logging
{
    public static class ExceptionDetailBuilder
    {
        private const string FileNameString = "File Name\t:\t";
        private const string ClassNameString = "Class Name\t:\t";
        private const string MethodNameString = "Method Name\t:\t";
        private const string LineNumberString = "Line Number\t:\t";

        public static void Build(Exception exception, out string exceptionDetail, out string exceptionData)
        {
            string fileName;
            Build(exception, out exceptionDetail, out exceptionData, out fileName);
        }

        public static void Build(Exception exception, out string exceptionDetail, out string exceptionData, out string fileName)
        {
            if (exception != null)
            {
                exceptionData = String.Empty;
                fileName = String.Empty;
                bool isInnerException = false;

                string exceptionMessage = String.Empty;
                StringBuilder exceptionStackTrace = new StringBuilder();

                while (exception != null)
                {
                    if (!isInnerException)
                    {
                        if (!exception.Data.Contains("Environment"))
                        {
                            exceptionData = String.Concat(exceptionData, "Environment : " + Nhea.Configuration.Settings.Application.EnvironmentType.ToString() + Environment.NewLine);
                        }

                        if (!exception.Data.Contains("OSVersion"))
                        {
                            exceptionData = String.Concat(exceptionData, "OSVersion : " + System.Environment.OSVersion + Environment.NewLine);
                        }

                        if (!exception.Data.Contains("Framework"))
                        {
                            //var version = System.Environment.Version;
                            //var appContextFrameworkName = System.AppContext.TargetFrameworkName;
                            string frameworkName = null;

                            try
                            {
                                frameworkName = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
                            }
                            catch
                            { }

                            if (string.IsNullOrEmpty(frameworkName))
                            {
                                frameworkName = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
                            }

                            exceptionData = String.Concat(exceptionData, "Framework : " + frameworkName + Environment.NewLine);
                        }

                        if (!exception.Data.Contains("OSArchitecture"))
                        {
                            exceptionData = String.Concat(exceptionData, "OSArchitecture : " + System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString() + Environment.NewLine);
                        }

                        if (!exception.Data.Contains("ProcessArchitecture"))
                        {
                            exceptionData = String.Concat(exceptionData, "ProcessArchitecture : " + System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString() + Environment.NewLine);
                        }

                        //if (!exception.Data.Contains("Is64BitOperatingSystem"))
                        //{
                        //    exceptionData = String.Concat(exceptionData, "Is64BitOperatingSystem : " + System.Environment.Is64BitOperatingSystem.ToString() + Environment.NewLine);
                        //}

                        //if (!exception.Data.Contains("Is64BitProcess"))
                        //{
                        //    exceptionData = String.Concat(exceptionData, "Is64BitProcess : " + System.Environment.Is64BitProcess.ToString() + Environment.NewLine);
                        //}
                    }

                    if (exception.Data.Count > 0)
                    {
                        foreach (DictionaryEntry entry in exception.Data)
                        {
                            if (entry.Key != null && entry.Value != null)
                            {
                                exceptionData = String.Concat(exceptionData, entry.Key.ToString() + " : " + entry.Value.ToString() + Environment.NewLine);
                            }
                        }
                    }

                    exceptionStackTrace.Insert(0, Environment.NewLine);
                    exceptionStackTrace.Insert(0, Environment.NewLine);

                    exceptionStackTrace.Insert(0, exception.StackTrace);
                    exceptionStackTrace.Insert(0, Environment.NewLine);

                    StackTrace trace = new StackTrace(exception, true);

                    if (trace != null)
                    {
                        var frames = trace.GetFrames();

                        if (frames != null)
                        {
                            var framesList = frames.Reverse().ToList();

                            foreach (StackFrame frame in framesList)
                            {
                                string lineNumber = LineNumberString + frame.GetFileLineNumber().ToString() + Environment.NewLine;
                                exceptionStackTrace.Insert(0, lineNumber);

                                MethodBase methodBase = frame.GetMethod();

                                if (methodBase != null)
                                {
                                    string methodName = MethodNameString + methodBase.Name + Environment.NewLine;
                                    exceptionStackTrace.Insert(0, methodName);

                                    if (methodBase.DeclaringType != null)
                                    {
                                        string className = ClassNameString + methodBase.DeclaringType.FullName + Environment.NewLine;
                                        exceptionStackTrace.Insert(0, className);
                                    }
                                }

                                string file = frame.GetFileName();

                                if (!String.IsNullOrEmpty(file))
                                {
                                    fileName = file;

                                    exceptionStackTrace.Insert(0, FileNameString + file);
                                    exceptionStackTrace.Insert(0, Environment.NewLine);
                                }
                            }
                        }
                    }

                    exceptionStackTrace.Insert(0, Environment.NewLine);
                    exceptionStackTrace.Insert(0, exception.Message);

                    exception = exception.InnerException;
                    isInnerException = true;
                }

                exceptionDetail = exceptionMessage + Environment.NewLine + Environment.NewLine + exceptionStackTrace;
                exceptionDetail = exceptionDetail.Trim('\r', '\n');
            }
            else
            {
                exceptionDetail = null;
                exceptionData = null;
                fileName = null;
            }
        }
    }
}
