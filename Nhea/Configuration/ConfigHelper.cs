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