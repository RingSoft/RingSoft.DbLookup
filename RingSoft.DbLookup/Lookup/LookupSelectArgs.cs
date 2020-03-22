namespace RingSoft.DbLookupCore.Lookup
{
    /// <summary>
    /// Arguments sent by the LookupSelect event.
    /// </summary>
    public class LookupSelectArgs
    {
        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <value>
        /// The lookup data.
        /// </value>
        public LookupDataBase LookupData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupSelectArgs"/> class.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        public LookupSelectArgs(LookupDataBase lookupData)
        {
            LookupData = lookupData;
        }
    }
}
