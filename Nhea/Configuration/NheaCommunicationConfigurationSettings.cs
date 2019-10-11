using Nhea.Configuration.GenericConfigSection.Communication;
using System.Collections.Generic;

namespace Nhea.Configuration
{
    public class NheaCommunicationConfigurationSettings
    {
        public string ConnectionString { get; set; }

        public IEnumerable<SmtpElement> SmtpSettings { get; set; }        
    }
}
