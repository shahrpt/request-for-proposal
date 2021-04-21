namespace UrbanRFP.Infrastructure.Helpers
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Web;

    public static class Utility
    {
        public static Uri[] Address;
        /// <summary>
        /// Returns the value of the configuration setting called ”settingName”
        /// from either web.config, or the Azure Role Environment.
        /// </summary>
        public static string GetSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }

        #region HasItems(Extension Method)

        /// <summary>
        ///     Extension method to check the number of items in Collection
        /// </summary>
        /// <param name="item">the item which is to be checked</param>
        /// <returns>boolean - whether items are present or not</returns>
        public static bool HasItems(this ICollection item)
        {
            return (item.IsNotNull() && item.Count > 0);
        }

        #endregion

        #region HasRows

        /// <summary>
        ///     Extension method to check if datatable is null and if it has any rows
        /// </summary>
        /// <param name="dtSource">Datatable calling the function</param>
        /// <returns>Boolean</returns>
        public static bool HasRows(this DataTable dtSource)
        {
            return (dtSource.IsNotNull() && dtSource.Rows.Count > 0);
        }

        #endregion

        #region List Has Entiities
        /// <summary>
        /// Gets a value indicating whether the specified list contains any items.
        /// </summary>
        /// <param name="entities">A collection of objects.</param>
        /// <returns>True if the collection is not null and contains at least
        /// one item; otherwise false.</returns>
        public static bool HasEntities(this IList entities)
        {
            return (entities != null && entities.Count > 0);
        }

        #endregion

        #region ToLong

        /// <summary>
        ///     Extension method to check the long value
        /// </summary>
        /// <param name="source">the object calling the function</param>
        /// <returns>long</returns>
        public static long ToLong(this object source)
        {
            long result = 0;
            if (source.IsNotNull()) long.TryParse(source.ToString(), out result);
            return result;
        }

        #endregion

        #region CheckInt

        /// <summary>
        ///     Extension method to check if the int value is valid and return the value accordingly
        /// </summary>
        /// <param name="source">object calling the extension method</param>
        /// <returns>int value</returns>
        public static int ToInt(this object source)
        {
            int result = 0;
            if (source.IsNotNull()) int.TryParse(source.ToString(), out result);
            return result;
        }

        #endregion

        # region "Method to get the current domain url"

        /// <summary>
        ///     Method to get the current domain url
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomain()
        {
            try
            {
                return HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter +
                       HttpContext.Current.Request.Url.Host;
            }
            catch
            {
                throw;
            }
        }

        # endregion
    }
}
