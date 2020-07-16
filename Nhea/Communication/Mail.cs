using System;
using System.Collections.Generic;

namespace Nhea.Communication
{
    public class Mail : MailParameters
    {
        protected internal Mail()
        {
            Attachments = new List<MailQueueAttachment>();
        }

        public Guid Id { get; set; }

        public int? MailProviderId { get; set; }

        public string From { get; set; }

        public string ToRecipient { get; set; }

        public string CcRecipients { get; set; }

        public string BccRecipients { get; set; }

        public string Subject { get; set; }

        public DateTime Priority { get; set; }

        public DateTime CreateDate { get; set; }

        public bool HasAttachment { get; set; }

        public List<MailQueueAttachment> Attachments { get; set; }
    }
}
