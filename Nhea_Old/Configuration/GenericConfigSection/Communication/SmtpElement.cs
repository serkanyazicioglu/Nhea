using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Nhea.Configuration.GenericConfigSection.Communication
{
    public class SmtpElement : ConfigurationElement
    {
        [ConfigurationProperty("from", IsRequired = true, IsKey = true)]
        public string From
        {
            get
            {
                if (!String.IsNullOrEmpty(this["from"].ToString()))
                {
                    return this["from"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get
            {
                if (!String.IsNullOrEmpty(this["host"].ToString()))
                {
                    return this["host"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        [ConfigurationProperty("enableSsl", IsRequired = false)]
        public bool EnableSsl
        {
            get
            {
                if (!String.IsNullOrEmpty(this["enableSsl"].ToString()))
                {
                    return Convert.ToBoolean(this["enableSsl"]);
                }
                else
                {
                    return false;
                }
            }
        }

        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get
            {
                if (!String.IsNullOrEmpty(this["userName"].ToString()))
                {
                    return this["userName"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                if (!String.IsNullOrEmpty(this["password"].ToString()))
                {
                    return this["password"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        [ConfigurationProperty("port", IsRequired = false)]
        public int Port
        {
            get
            {
                if (!String.IsNullOrEmpty(this["port"].ToString()))
                {
                    return Convert.ToInt32(this["port"]);
                }
                else
                {
                    return 25;
                }
            }
        }

        [ConfigurationProperty("isDefault", IsRequired = false)]
        public bool IsDefault
        {
            get
            {
                if (!String.IsNullOrEmpty(this["isDefault"].ToString()))
                {
                    return Convert.ToBoolean(this["isDefault"]);
                }
                else
                {
                    return false;
                }
            }
        }

        [ConfigurationProperty("defaultToRecipients", IsRequired = false, DefaultValue = "")]
        public string DefaultToRecipients
        {
            get
            {
                return this["defaultToRecipients"].ToString();
            }
        }

        [ConfigurationProperty("defaultSubject", IsRequired = false, DefaultValue = "")]
        public string DefaultSubject
        {
            get
            {
                return this["defaultSubject"].ToString();
            }
        }

        [ConfigurationProperty("sender", IsRequired = false, DefaultValue = "")]
        public string Sender
        {
            get
            {
                return this["sender"].ToString();
            }
        }
    }
}
