using System.Globalization;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Sets up the lookup default properties.  These properties should be set before any lookup definitions are constructed.
    /// </summary>
    public static class LookupDefaults
    {
        /// <summary>
        /// Gets the default number culture identifier.
        /// </summary>
        /// <value>
        /// The default number culture identifier.
        /// </value>
        public static string DefaultNumberCultureId { get; private set; } = CultureInfo.CurrentCulture.Name;

        /// <summary>
        /// Gets the default date culture identifier.
        /// </summary>
        /// <value>
        /// The default date culture identifier.
        /// </value>
        public static string DefaultDateCultureId { get; private set; } = CultureInfo.CurrentCulture.Name;

        /// <summary>
        /// Gets the default decimal count.
        /// </summary>
        /// <value>
        /// The default decimal count.
        /// </value>
        public static int DefaultDecimalCount { get; private set; } = 2;

        /// <summary>
        /// Sets the default number culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        public static void SetDefaultNumberCultureId(string cultureId)
        {
            var unused = new CultureInfo(cultureId);
            DefaultNumberCultureId = cultureId;
        }

        /// <summary>
        /// Sets the default date format identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        public static void SetDefaultDateFormatId(string cultureId)
        {
            var unused = new CultureInfo(cultureId);
            DefaultDateCultureId = cultureId;
        }

        /// <summary>
        /// Sets the default decimal count.
        /// </summary>
        /// <param name="decimalCount">The decimal count.</param>
        public static void SetDefaultDecimalCount(int decimalCount)
        {
            DefaultDecimalCount = decimalCount;
        }
    }
}
