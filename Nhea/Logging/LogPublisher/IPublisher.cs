using Microsoft.Extensions.Logging;
using System;

namespace Nhea.Logging.LogPublisher
{
    internal interface IPublisher
    {
        string Message { get; set; }
        string Source { get; set; }
        string UserName { get; set; }
        Exception Exception { get; set; }
        LogLevel Level { get; set; }
        bool AutoInform { get; set; }

        bool Publish();
    }
}
