using System;

namespace Nhea.Logging.LogPublisher
{
    internal interface IPublisher
    {
        #region Properties

        string Message { get; set; }
        string Source { get; set; }
        string UserName { get; set; }
        Exception Exception { get; set; }
        LogLevel LogLevel { get; set; }
        bool AutoInform { get; set; }

        #endregion

        #region Methods

        bool Publish();
        
        #endregion Methods
    }
}
