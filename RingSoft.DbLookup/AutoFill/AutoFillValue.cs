namespace RingSoft.DbLookup.AutoFill
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

        public bool FromLookup { get; internal set; } = true;

        public AutoFillValue(PrimaryKeyValue primaryKeyValue, string text)
        {
            PrimaryKeyValue = primaryKeyValue;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
