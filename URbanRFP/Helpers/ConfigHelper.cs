using System.Configuration;

namespace UrbanRFP.Helpers
{
    public class ConfigHelper
    {
        /// <summary>
        /// Returns the value of the configuration setting called "DomainURL"
        /// from either web.config, or the Azure Role Environment.
        /// </summary>
        public static string DomainURL
        {
            get
            {
                return GetSettingAsString("DomainURL");
            }
        }

        /// <summary>
        /// Returns the value of the configuration setting called ”settingName”
        /// from either web.config, or the Azure Role Environment.
        /// </summary>
        public static string GetSettingAsString(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}