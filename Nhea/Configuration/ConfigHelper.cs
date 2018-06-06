using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nhea.Helper;

namespace Nhea.Configuration
{
    public class ConfigHelper
    {
        public static T GetValue<T>(string parameterName)
        {
            ConfigBase configBase = ConfigFactory.GetConfigManager();
            return ConvertionHelper.GetConvertedValue<T>(configBase.GetValue(parameterName));
        }
    }
}