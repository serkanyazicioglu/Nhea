﻿using Microsoft.Extensions.Logging;
using Nhea.Configuration;
using System;
using System.Net;
using System.Security.Principal;

namespace Nhea.Logging.LogPublisher
{
    public abstract class Publisher : IPublisher
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
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = System.Environment.UserName + "/" + System.Environment.UserDomainName;

                        if (System.Environment.UserDomainName != System.Environment.MachineName)
                        {
                            userName += "/" + System.Environment.MachineName;
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
                if (Settings.CurrentCommunicationConfigurationSettings == null)
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
