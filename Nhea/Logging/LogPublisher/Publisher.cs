using System;
using System.Security.Principal;

namespace Nhea.Logging.LogPublisher
{
    public abstract class Publisher : IPublisher
    {
        #region IPublisher Properties

        public abstract string Message { get; set; }

        public abstract Exception Exception { get; set; }

        public abstract LogLevel LogLevel { get; set; }

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
                if (!String.IsNullOrEmpty(source))
                {
                    return source;
                }
                else if (this.Exception != null)
                {
                    return this.Exception.Source;
                }
                else
                {
                    return "Nhea";
                }
            }
            set
            {
                source = value;
            }
        }

        #endregion

        #region IPublisher Methods

        public virtual bool Publish()
        {
            if (this.AutoInform)
            {
                Publisher publisher = PublisherFactory.CreatePublisher(PublishTypes.Email);
                publisher.Message = this.Message;
                publisher.LogLevel = this.LogLevel;
                publisher.Source = this.Source;
                publisher.UserName = this.UserName;
                publisher.Exception = this.Exception;
                publisher.Publish();
            }

            return true;
        }

        #endregion
    }
}
