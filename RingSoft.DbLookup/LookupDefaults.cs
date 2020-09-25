using System.Globalization;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Sets up the lookup default properties.  These properties should be set before any lookup definitions are constructed.
    /// </summary>
    public static class LookupDefaults
    {
        /// <summary>
        /// Gets the default number culture.
        /// </summary>
        /// <value>
        /// The default number culture.
        /// </value>
        public static CultureInfo DefaultNumberCulture { get; private set; }

        public static CultureInfo DefaultDateCulture { get; private set; }

        /// <summary>
        /// Gets the default decimal count.
        /// </summary>
        /// <value>
        /// The default decimal count.
        /// </value>
        public static int DefaultDecimalCount { get; private set; } = 2;

        static LookupDefaults()
        {
            DefaultNumberCulture = DefaultDateCulture = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Sets the default number culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        public static void SetDefaultNumberCultureId(string cultureId)
        {
            DefaultNumberCulture = new CultureInfo(cultureId);
        }

        /// <summary>
        /// Sets the default date format identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        public static void SetDefaultDateFormatId(string cultureId)
        {
            DefaultDateCulture = new CultureInfo(cultureId);
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
