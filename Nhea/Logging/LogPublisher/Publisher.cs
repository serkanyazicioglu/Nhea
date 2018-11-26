using Microsoft.Extensions.Logging;
using Nhea.Configuration;
using System;
using System.Security.Principal;

namespace Nhea.Logging.LogPublisher
{
    public abstract class Publisher : IPublisher, ILogger
    {
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
                    if (!String.IsNullOrEmpty(userName))
                    {
                        return userName;
                    }
                    else if (WindowsIdentity.GetCurrent() != null)
                    {
                        return WindowsIdentity.GetCurrent().Name;
                    }
                }
                catch
                {
                }

                return null;
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
                if (String.IsNullOrEmpty(source))
                {
                    source = String.Empty;

                    try
                    {
                        var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

                        if (!String.IsNullOrEmpty(assemblyName))
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

                        if (!String.IsNullOrEmpty(assemblyName))
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

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string msg = $"{logLevel} :: {formatter(state, exception)} :: UserName :: {this.UserName} :: {DateTime.Now}";

            this.Level = logLevel;
            this.Exception = exception;
            this.Message = "";
            this.AutoInform = Settings.Log.AutoInform;
            this.Publish();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
