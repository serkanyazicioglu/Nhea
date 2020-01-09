﻿using System;
using System.Configuration;

namespace Nhea.Configuration.GenericConfigSection.Communication
{
    public class SmtpElement : ConfigurationElement
    {
        private string from = null;

        [ConfigurationProperty("from", IsRequired = true, IsKey = true)]
        public string From
        {
            get
            {
                if (!string.IsNullOrEmpty(from))
                {
                    return from;
                }

                if (!String.IsNullOrEmpty(this["from"].ToString()))
                {
                    return this["from"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                from = value;
            }
        }

        private string host = null;

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get
            {
                if (!string.IsNullOrEmpty(host))
                {
                    return host;
                }

                if (!String.IsNullOrEmpty(this["host"].ToString()))
                {
                    return this["host"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                host = value;
            }
        }

        private bool? enableSsl = null;

        [ConfigurationProperty("enableSsl", IsRequired = false)]
        public bool EnableSsl
        {
            get
            {
                if (enableSsl.HasValue)
                {
                    return enableSsl.Value;
                }

                if (!String.IsNullOrEmpty(this["enableSsl"].ToString()))
                {
                    return Convert.ToBoolean(this["enableSsl"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                enableSsl = value;
            }
        }

        private string userName = null;

        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    return userName;
                }

                if (!String.IsNullOrEmpty(this["userName"].ToString()))
                {
                    return this["userName"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                userName = value;
            }
        }

        private string password = null;

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                if (!string.IsNullOrEmpty(password))
                {
                    return password;
                }

                if (!String.IsNullOrEmpty(this["password"].ToString()))
                {
                    return this["password"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                password = value;
            }
        }

        private int? port = null;

        [ConfigurationProperty("port", IsRequired = false)]
        public int Port
        {
            get
            {
                if (port.HasValue)
                {
                    return port.Value;
                }

                if (!String.IsNullOrEmpty(this["port"].ToString()))
                {
                    return Convert.ToInt32(this["port"]);
                }
                else
                {
                    return 25;
                }
            }
            set
            {
                port = value;
            }
        }

        private bool? isDefault = null;

        [ConfigurationProperty("isDefault", IsRequired = false)]
        public bool IsDefault
        {
            get
            {
                if (isDefault.HasValue)
                {
                    return isDefault.Value;
                }

                if (!String.IsNullOrEmpty(this["isDefault"].ToString()))
                {
                    return Convert.ToBoolean(this["isDefault"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                isDefault = value;
            }
        }

        private string defaultToRecipients = null;

        [ConfigurationProperty("defaultToRecipients", IsRequired = false, DefaultValue = "")]
        public string DefaultToRecipients
        {
            get
            {
                if (!string.IsNullOrEmpty(defaultToRecipients))
                {
                    return defaultToRecipients;
                }

                return this["defaultToRecipients"].ToString();
            }
            set
            {
                defaultToRecipients = value;
            }
        }

        private string defaultSubject = null;

        [ConfigurationProperty("defaultSubject", IsRequired = false, DefaultValue = "")]
        public string DefaultSubject
        {
            get
            {
                if (!string.IsNullOrEmpty(defaultSubject))
                {
                    return defaultSubject;
                }

                return this["defaultSubject"].ToString();
            }
            set
            {
                defaultSubject = value;
            }
        }

        private string sender = null;

        [ConfigurationProperty("sender", IsRequired = false, DefaultValue = "")]
        public string Sender
        {
            get
            {
                if (!string.IsNullOrEmpty(sender))
                {
                    return sender;
                }

                return this["sender"].ToString();
            }
            set
            {
                sender = value;
            }
        }

        private bool? autoGeneratePlainText = null;

        [ConfigurationProperty("autoGeneratePlainText", IsRequired = false)]
        public bool AutoGeneratePlainText
        {
            get
            {
                if (autoGeneratePlainText.HasValue)
                {
                    return autoGeneratePlainText.Value;
                }

                if (!String.IsNullOrEmpty(this["autoGeneratePlainText"].ToString()))
                {
                    return Convert.ToBoolean(this["autoGeneratePlainText"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                autoGeneratePlainText = value;
            }
        }

        private bool? disableLogging = null;

        [ConfigurationProperty("disableLogging", IsRequired = false)]
        public bool DisableLogging
        {
            get
            {
                if (disableLogging.HasValue)
                {
                    return disableLogging.Value;
                }

                if (!String.IsNullOrEmpty(this["disableLogging"].ToString()))
                {
                    return Convert.ToBoolean(this["disableLogging"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                disableLogging = value;
            }
        }

    }
}
