using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Nhea.Configuration
{
    internal class XmlConfigHelper : ConfigBase
    {
        internal override object GetValue(string parameterName)
        {
            if (ConfigurationManager.AppSettings[parameterName] == null)
            {
                throw new Exception(string.Format("Invalid parameter name : {0}", parameterName));
            }

            return ConfigurationManager.AppSettings[parameterName];
        }
    }
}