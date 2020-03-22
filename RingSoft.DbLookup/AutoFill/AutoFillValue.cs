namespace RingSoft.DbLookupCore.AutoFill
{
    /// <summary>
    /// An AutoFill results value.
    /// </summary>
    public class AutoFillValue
    {
        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>
        /// The primary key value.
        /// </value>
        public PrimaryKeyValue PrimaryKeyValue { get; }


        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; }

        public AutoFillValue(PrimaryKeyValue primaryKeyValue, string text)
        {
            PrimaryKeyValue = primaryKeyValue;
            Text = text;
        }

        /// <summary>
        /// Determines whether this AutoFill primary key value is equal to the specified compare to primary key value.
        /// </summary>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>
        ///   <c>true</c> if the primary key value is equal to the specified compare to primary key value; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEqualTo(AutoFillValue compareTo)
        {
            if (compareTo == null)
                return false;

            return PrimaryKeyValue.IsEqualTo(compareTo.PrimaryKeyValue);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
