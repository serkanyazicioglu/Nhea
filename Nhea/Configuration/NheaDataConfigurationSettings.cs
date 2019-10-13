namespace Nhea.Configuration
{
    public class NheaDataConfigurationSettings
    {
        /// <summary>
        /// Default Sql connection string for localization.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Used for default language id.
        /// </summary>
        public int DefaultLanguageId { get; set; }
    }
}
