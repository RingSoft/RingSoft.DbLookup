namespace RingSoft.DbLookup.Lookup
{
    public abstract class LookupColumnDefinitionType<TColumnDefinition> : LookupColumnDefinitionBase
        where TColumnDefinition : LookupColumnDefinitionType<TColumnDefinition>
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

        public new TColumnDefinition HasSearchForHostId(int hostId)
        {
            base.HasSearchForHostId(hostId);
            return (TColumnDefinition) this;
        }

        public new TColumnDefinition HasLookupControlColumnId(int lookupControlColumnId)
        {
            base.HasLookupControlColumnId(lookupControlColumnId);
            return (TColumnDefinition) this;
        }

        public new TColumnDefinition HasContentTemplateId(int contentTemplateId)
        {
            base.HasContentTemplateId(contentTemplateId);
            return (TColumnDefinition) this;
        }

        public new TColumnDefinition HasKeepNullEmpty(bool value = true)
        {
            base.HasKeepNullEmpty(value);
            return (TColumnDefinition)this;
        }

        public new TColumnDefinition DoShowNegativeValuesInRed(bool value = true)
        {
            base.DoShowNegativeValuesInRed(value);
            return (TColumnDefinition)this;
        }

        public new TColumnDefinition DoShowPositiveValuesInGreen(bool value = true)
        {
            base.DoShowPositiveValuesInGreen(value);
            return (TColumnDefinition) this;
        }
    }
}
