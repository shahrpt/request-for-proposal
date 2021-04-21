using System.Configuration;

namespace UrbanRFP.Helpers
{
    public class Configuration
    {
        /// <summary>
        /// Connection String
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["UrbanRFPDB"].ToString();
        }
    }
}