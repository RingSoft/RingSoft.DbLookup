using System;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Lookup
{
    public enum LookupColumnTypes
    {
        Field = 0,
        Formula = 1,
    }

    public enum LookupColumnAlignmentTypes
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    /// <summary>
    /// The lookup column definition base class.
    /// </summary>
    public abstract class LookupColumnDefinitionBase : IJoinParent
    {
        /// <summary>
        /// Gets the join query table alias.
        /// </summary>
        /// <value>
        /// The join query table alias.
        /// </value>
        public string JoinQueryTableAlias { get; set; }

        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the column.
        /// </value>
        public abstract LookupColumnTypes ColumnType { get; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public abstract FieldDataTypes DataType { get; }

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition { get; internal set; }


        /// <summary>
        /// Gets the select SQL alias.
        /// </summary>
        /// <value>
        /// The select SQL alias.
        /// </value>
        public abstract string SelectSqlAlias { get; }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption { get; internal set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// Gets the column's percent of the lookup's width.
        /// </summary>
        /// <value>
        /// The percent width of the column.
        /// </value>
        public double PercentWidth { get; internal set; }

        public int LookupControlColumnId { get; internal set; }

        public int? ContentTemplateId { get; internal set; }

        public bool KeepNullEmpty { get; internal set; }


        /// <summary>
        /// Gets the horizontal alignment type.
        /// </summary>
        /// <value>
        /// The horizontal alignment type.
        /// </value>
        public LookupColumnAlignmentTypes HorizontalAlignment { get; private set; } = LookupColumnAlignmentTypes.Left;

        public virtual int? SearchForHostId { get; internal set; }

        public bool ShowNegativeValuesInRed { get; internal set; }

        public bool ShowPositiveValuesInGreen { get; internal set; }

        protected internal void SetupColumn()
        {
            HorizontalAlignment = SetupDefaultHorizontalAlignment();
        }

        protected virtual LookupColumnAlignmentTypes SetupDefaultHorizontalAlignment()
        {
            switch (DataType)
            {
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return LookupColumnAlignmentTypes.Right;
                default:
                    return LookupColumnAlignmentTypes.Left;
            }
        }
        internal virtual void CopyFrom(LookupColumnDefinitionBase source)
        {
            Caption = source.Caption;
            PropertyName = source.PropertyName;
            PercentWidth = source.PercentWidth;
            HorizontalAlignment = source.HorizontalAlignment;
            SearchForHostId = source.SearchForHostId;
            LookupControlColumnId = source.LookupControlColumnId;
            ShowNegativeValuesInRed = source.ShowNegativeValuesInRed;
            ShowPositiveValuesInGreen = source.ShowPositiveValuesInGreen;
            ContentTemplateId = source.ContentTemplateId;
            JoinQueryTableAlias = source.JoinQueryTableAlias;
            ParentObject = source.ParentObject;
            ChildField = source.ChildField;
            ParentField = source.ParentField;
        }

        /// <summary>
        /// Formats the value to display in the lookup view.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        public abstract string FormatValue(string value);

        /// <summary>
        /// Sets the horizontal alignment type.
        /// </summary>
        /// <param name="alignmentType">The new horizontal alignment type.</param>
        public void HasHorizontalAlignmentType(LookupColumnAlignmentTypes alignmentType)
        {
            HorizontalAlignment = alignmentType;
        }

        public void HasSearchForHostId(int hostId)
        {
            SearchForHostId = hostId;
        }

        public void HasLookupControlColumnId(int lookupControlColumnId)
        {
            LookupControlColumnId = lookupControlColumnId;
        }

        public void HasContentTemplateId(int contentTemplateId)
        {
            if (DataType != FieldDataTypes.Integer)
                throw new Exception("Custom Content Template can only be set on Integer fields.");

            ContentTemplateId = contentTemplateId;
            if (LookupControlColumnId == LookupDefaults.TextColumnId)
                LookupControlColumnId = LookupDefaults.CustomContentColumnId;
        }

        public void HasKeepNullEmpty(bool value = true)
        {
            KeepNullEmpty = value;
        }

        public void DoShowNegativeValuesInRed(bool value = true)
        {
            ShowNegativeValuesInRed = value;
        }

        public void DoShowPositiveValuesInGreen(bool value = true)
        {
            ShowPositiveValuesInGreen = value;
        }

        public void UpdatePercentWidth(double newValue)
        {
            PercentWidth = newValue;
        }

        public LookupColumnDefinitionBase UpdateCaption(string value)
        {
            Caption = value;
            return this;
        }

        public IJoinParent ParentObject { get; set; }
        public FieldDefinition ChildField { get; set; }
        public FieldDefinition ParentField { get; set; }

        public LookupJoin MakeInclude(LookupDefinitionBase lookupDefinition, FieldDefinition childField = null)
        {
            throw new NotImplementedException();
        }

        public LookupColumnDefinitionBase AddVisibleColumnDefinitionField(string caption, FieldDefinition fieldDefinition,
            double percentWidth)
        {
            return null;
        }
    }
}
