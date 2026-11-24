using Microsoft.Extensions.Logging;
using Nhea.Configuration;
using System;
using System.Diagnostics;

namespace Nhea.Logging.LogPublisher
{
    public abstract class Publisher : IPublisher
    {
        private static readonly Lazy<DateTime?> processStartTime = new(() =>
        {
            try
            {
                using (var process = Process.GetCurrentProcess())
                {
                    return process.StartTime;
                }
            }
            catch
            {
                return null;
            }
        });

        protected static DateTime? ProcessStartTime => processStartTime.Value;

        protected static string ProcessUptime
        {
            get
            {
                if (!ProcessStartTime.HasValue)
                {
                    return null;
                }

                var uptime = DateTime.Now - ProcessStartTime.Value;

                return string.Format("{0}d {1:hh\\:mm\\:ss}", (int)uptime.TotalDays, uptime);
            }
        }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public LogLevel Level { get; set; }

        public bool AutoInform { get; set; }

        private string userName;
        public string UserName
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = System.Environment.UserDomainName + "\\" + System.Environment.UserName;

                        if (System.Environment.UserDomainName != System.Environment.MachineName)
                        {
                            userName += "\\" + System.Environment.MachineName;
                        }

                        if (Settings.CurrentLogConfigurationSettings != null && !string.IsNullOrEmpty(Settings.CurrentLogConfigurationSettings.FriendlyName))
                        {
                            userName += " (" + Settings.CurrentLogConfigurationSettings.FriendlyName + ")";
                        }
                    }

                    return userName;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                userName = value;
            }
        }

        private string source;
        public virtual string Source
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                {
                    source = string.Empty;

                    try
                    {
                        var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

                        if (!string.IsNullOrEmpty(assemblyName))
                        {
                            source = assemblyName;
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

                        if (!string.IsNullOrEmpty(assemblyName))
                        {
                            source += ", " + assemblyName;
                        }

                        if (this.Exception != null && assemblyName != this.Exception.Source)
                        {
                            source += ", " + this.Exception.Source;
                        }
                    }
                    catch
                    {
                    }

                    source = source.Trim().Trim(',');
                }

                return source;
            }
            set
            {
                source = value;
            }
        }

        public virtual bool Publish()
        {
            if (this.AutoInform)
            {
                if (Settings.CurrentCommunicationConfigurationSettings == null && Settings.CurrentLogConfigurationSettings != null)
                {
                    Settings.CurrentCommunicationConfigurationSettings = new NheaCommunicationConfigurationSettings { ConnectionString = Settings.CurrentLogConfigurationSettings.ConnectionString };
                }

                Publisher publisher = PublisherFactory.CreatePublisher(PublishTypes.Email);
                publisher.Message = this.Message;
                publisher.Level = this.Level;
                publisher.Source = this.Source;
                publisher.UserName = this.UserName;
                publisher.Exception = this.Exception;
                publisher.Publish();
            }

            return true;
        }
    }
}
