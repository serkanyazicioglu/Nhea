using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nhea.Communication
{
    internal enum MailStatus
    {
        Failed = -2,
        NotDelivered = -1,
        Sent = 1,
        Delivered = 2,
        Opened = 3
    }
}
