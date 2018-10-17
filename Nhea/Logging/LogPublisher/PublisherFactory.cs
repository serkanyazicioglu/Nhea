namespace Nhea.Logging.LogPublisher
{
    internal static class PublisherFactory
    {
        internal static Publisher CreatePublisher(PublishTypes publishType)
        {
            switch (publishType)
            {
                default:
                case PublishTypes.Database:
                    {
                        return new DbPublisher();
                    }
                case PublishTypes.File:
                    {
                        return new FilePublisher();
                    }
                case PublishTypes.Email:
                    {
                        return new MailPublisher();
                    }
            }
        }
    }
}
