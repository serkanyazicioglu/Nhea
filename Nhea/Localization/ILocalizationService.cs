using System;

namespace Nhea.Localization
{
    public interface ILocalizationService
    {
        string GetLocalization(string key);

        string GetLocalization(string key, int languageId);

        string GetLocalization(string key, string culture);

        string GetLocalization(string key, Guid targetEntityId, int languageId);

        Guid? GetTargetEntityId(string key, string translation, string targetEntityName, int languageId);

        void SaveLocalization(string translation, string key);

        void SaveLocalization(string translation, string key, int languageId);

        void SaveLocalization(string translation, string key, Guid targetEntityId, string targetEntityName, int languageId);

        void DeleteLocalization(Guid targetEntityId);

        void DeleteLocalization(string key);

        void DeleteLocalization(string key, int languageId);

        void DeleteLocalization(Guid targetEntityId, string key, int languageId);
    }
}
