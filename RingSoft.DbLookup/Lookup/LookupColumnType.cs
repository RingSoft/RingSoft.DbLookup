namespace RingSoft.DbLookup.Lookup
{
    public abstract class LookupColumnType<TColumnDefinition> : LookupColumnBase
        where TColumnDefinition : LookupColumnType<TColumnDefinition>
    {

        /// <summary>
        /// Sets the horizontal alignment type.
        /// </summary>
        /// <param name="alignmentType">The new horizontal alignment type.</param>
        /// <returns>This object for fluent coding.</returns>
        public new TColumnDefinition HasHorizontalAlignmentType(LookupColumnAlignmentTypes alignmentType)
        {
            base.HasHorizontalAlignmentType(alignmentType);
            return (TColumnDefinition) this;
        }
    }
}
