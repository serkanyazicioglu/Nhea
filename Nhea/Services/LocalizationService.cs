using Microsoft.Extensions.Options;
using Nhea.Configuration;
using Nhea.Localization;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class LocalizationService : ILocalizationService
    {
        public LocalizationService(IOptions<NheaDataConfigurationSettings> nheaConfigurationSettings)
        {
            Settings.CurrentDataConfigurationSettings = nheaConfigurationSettings.Value;
        }

        public void DeleteLocalization(Guid targetEntityId)
        {
            LocalizationManager.DeleteLocalization(targetEntityId);
        }

        public void DeleteLocalization(string key)
        {
            LocalizationManager.DeleteLocalization(key, Settings.CurrentDataConfigurationSettings.DefaultLanguageId);
        }

        public void DeleteLocalization(string key, int languageId)
        {
            LocalizationManager.DeleteLocalization(key, languageId);
        }

        public void DeleteLocalization(Guid targetEntityId, string key, int languageId)
        {
            LocalizationManager.DeleteLocalization(targetEntityId, key, languageId);
        }

        public string GetLocalization(string key)
        {
            return LocalizationManager.GetLocalization(key, Settings.CurrentDataConfigurationSettings.DefaultLanguageId);
        }

        public string GetLocalization(string key, int languageId)
        {
            return LocalizationManager.GetLocalization(key, languageId);
        }

        public string GetLocalization(string key, string culture)
        {
            return LocalizationManager.GetLocalization(key, culture);
        }

        public string GetLocalization(string key, Guid targetEntityId, int languageId)
        {
            return LocalizationManager.GetLocalization(key, targetEntityId, languageId);
        }

        public Guid? GetTargetEntityId(string key, string translation, string targetEntityName, int languageId)
        {
            return LocalizationManager.GetTargetEntityId(key, translation, targetEntityName, languageId);
        }

        public void SaveLocalization(string translation, string key)
        {
            LocalizationManager.SaveLocalization(translation, key, Settings.CurrentDataConfigurationSettings.DefaultLanguageId);
        }

        public void SaveLocalization(string translation, string key, int languageId)
        {
            LocalizationManager.SaveLocalization(translation, key, languageId);
        }

        public void SaveLocalization(string translation, string key, Guid targetEntityId, string targetEntityName, int languageId)
        {
            LocalizationManager.SaveLocalization(translation, key, targetEntityId, targetEntityName, languageId);
        }
    }
}
