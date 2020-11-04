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

    public enum LookupColumnSearchForControls
    {
        StringControl = LookupColumnDefinitionBase.SearchForStringControl,
        IntegerControl = LookupColumnDefinitionBase.SearchForIntegerControl,
        DecimalControl = LookupColumnDefinitionBase.SearchForDecimalControl,
        DateControl = LookupColumnDefinitionBase.SearchForDateControl
    }

    /// <summary>
    /// The lookup column definition base class.
    /// </summary>
    public abstract class LookupColumnDefinitionBase
    {
        public const int SearchForStringControl = 0;
        public const int SearchForIntegerControl = 1;
        public const int SearchForDecimalControl = 2;
        public const int SearchForDateControl = 3;

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

        /// <summary>
        /// Gets the horizontal alignment type.
        /// </summary>
        /// <value>
        /// The horizontal alignment type.
        /// </value>
        public LookupColumnAlignmentTypes HorizontalAlignment { get; private set; } = LookupColumnAlignmentTypes.Left;

        public int? SearchForControl { get; private set; } = null;

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
            SearchForControl = source.SearchForControl;
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

        public void HasSearchForControl(int controlId)
        {

        }
    }
}
