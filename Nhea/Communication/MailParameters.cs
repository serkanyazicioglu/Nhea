﻿namespace Nhea.Communication
{
    public class MailParameters
    {
        public string Version { get; set; }

        public string Body { get; set; }

        public string ListUnsubscribe { get; set; }

        public string PlainText { get; set; }

        public bool IsBulkEmail { get; set; }

        public bool UnsubscribeOneClick { get; set; }
    }
}
