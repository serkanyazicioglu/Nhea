using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nhea.Logging.LogPublisher
{
    internal static class PublisherFactory
    {
        internal static Publisher CreatePublisher(PublishType publishType)
        {
            switch (publishType)
            {
                default:
                case PublishType.Database:
                    {
                        return new DbPublisher();
                    }
                case PublishType.EventLog:
                    {
                        return new EventLogPublisher();
                    }
                case PublishType.File:
                    {
                        return new FilePublisher();
                    }
                case PublishType.Email:
                    {
                        return new MailPublisher();
                    }
            }
        }
    }
}
