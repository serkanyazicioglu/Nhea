using System.Configuration;

namespace Nhea.Configuration.GenericConfigSection.Communication
{
    [ConfigurationCollection(typeof(SmtpElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class SmtpSettingsElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SmtpElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as SmtpElement).From;
        }
    }
}
