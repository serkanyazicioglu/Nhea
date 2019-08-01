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
    }
}