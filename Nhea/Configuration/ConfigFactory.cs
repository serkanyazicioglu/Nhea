using System;
using System.Configuration;

namespace Nhea.Configuration
{
    internal class ConfigFactory
    {
        internal static T GetConfigurationSection<T>(string section) where T : ConfigurationSection, new()
        {
            object configurationSection = ConfigurationManager.GetSection(section);

            if (configurationSection != null)
            {
                return (T)configurationSection;
            }
            else
            {
                return new T();
            }
        }

        internal static ConfigBase GetConfigManager()
        {
            ConfigBase configBase = new XmlConfigHelper();
            string location = configBase.GetValue("location").ToString();
            configBase = Activator.CreateInstance(Type.GetType(location)) as ConfigBase;

            if (configBase == null)
            {
                throw new Exception(string.Format("Geçersiz config base türü. ConfigBase örneği alınamadı. Tip : {0}", location));
            }

            return configBase;
        }
    }
}